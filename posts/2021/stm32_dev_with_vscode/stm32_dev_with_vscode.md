---
type: "technical"
date: "2021-05-21T18:31:00+08:00"
title: "STM32 Embedded Development with VS Code"
brief: "With STM32CubeMX + ST-Link + CMake"
headerImage: "dusty_nucleo_h743zi_devboard.avif"
tags: ["STM32", "vscode", "Embedded"]
---

## 开场先阴阳怪气一波

不会吧不会吧不会有人还在用 Keil 撸 STM32 吧！这垃圾玩意儿能用？我的妈，本科第一次接触嵌入式就是那这个屑 IDE 开发 8051 单片机，好家伙，这简直就是上世纪的软件，UI 都是糊的，更别想什么好用的代码智能提示功能了。还好后来写 STM32 用的 ~~学习版的~~ VisualGDB （一个 Visual Studio 的嵌入式开发插件），是还不错了，可惜只能在 Windows 下使，但是 Windows 就很垃圾，各种驱动啊路径啊烦死了！现在主力开发环境是 macOS 或者 Linux，所以还是折腾一下宇宙第一的~~代码编辑器~~ vscode。

反正没事儿，就是玩儿。

## Environment preparation

### VSCode Extensions

These extensions are needed, intall them in VS Code:

- Cortex-Debug
- CMake & CMake Tools
- Clangd
- ARM

### STM32CubeMX

We use the software for MCU packages management, clock and peripherals configuration and code generation.

![MCU packages](images/stm32cubemx_mcu_package.avif)

Install the required MCU and software packages, they will be installed to `$HOME/STM32Cube/Repository/`

Now we can start a new project, configure the clock tree, pinout, peripherals, then generate the code somewhere.

### ST-Link

The ST-Link tools are readily available in Homebrew on macOS, just

```shell
brew install stlink
```

Man, Windows just sucks.

Now plug in the ST-Link debugger via USB, then check if it is properly connected.

```shell
st-info --probe

Found 1 stlink programmers
  version:    V2J29S18
  serial:     066EFF545257717867101452
  flash:      2097152 (pagesize: 131072)
  sram:       131072
  chipid:     0x0450
  descr:      H74x/H75x
```

### Toolchain

GCC embedded toolchain is also available in Homebrew, neat!

```shell
brew install --cask gcc-arm-embedded
```

It will be installed in `/usr/local/Caskroom/gcc-arm-embedded/${VERSION}/gcc-arm-none-eabi-${VERSION}/`, we need this path when setting up the cmake later.

### CMake

We use cmake as the backbone of our project. Sadly, The **STM32CubeMX** cannot generate a CMake-based project, so we need some outside help.

Download the [STM32-CMake](https://github.com/ObKo/stm32-cmake) and extract the `cmake` folder into the project directory generated by the STM32CubeMX.

### CMSIS-SVD

The System View Description (SVD) files are used by the **Cortex-Debug** extension to identify peripheral registers. Just download the `.svd` file of our MCU type [here](https://github.com/posborne/cmsis-svd/tree/master/data/STMicro) and put it into the project directory.

## Setup the project

First setup the **CMake Kit**. Open the VS Code command palette with `Shift⇧ + Cmd⌘ + P` and type `CMake: Edit User-Local CMake Kits`, which will bring you to `$HOME/.local/share/CMakeTools/cmake-tools-kits.json`, Add a kit configuration：

```json
{
    "name": "GCC arm-none-eabi",
    "toolchainFile": "${workspaceFolder}/cmake/stm32_gcc.cmake"
}
```

Then select the newly added kit with `CMake: Select a Kit` or use the shortcut button below.

This will use the `cmake/stm32_gcc.cmake` in the project directory as the `CMAKE_TOOLCHAIN_FILE`. We also need to specify the toolchain path, add these at the top of `stm32_gcc.cmake`:

```cmake
set(STM32_TOOLCHAIN_PATH "/usr/local/Caskroom/gcc-arm-embedded/${VERSION}/gcc-arm-none-eabi-${VERSION}")
set(TARGET_TRIPLET "arm-none-eabi")
```

This is for the cmake to properly generate `--sysroot` arguments in the compile commands.

The compile commands are also saved to `build/compile_commands.json`. We also need to tell **Clangd** extension where to find this file so it can provide correct code linting and completions. Just add `--compile-commands-dir=${workspaceFolder}/build` into `Clangd: Arguments` in VS Code settings.

![Clangd args](images/clangd_args.avif)

### Exclude template source files

**STM32-CMake** will include the template `system_stm32${FAMILY}xx.c` and `startup_stm32${TYPE}xx.s` into source files, how inane! Since **STM32CubeMX** will generate these files in our project directory, we have to exclude the template versions in case of linker errors.

To do so, we need to modify the `cmake/findCMSIS.cmake`.

At around line `117`, remove these nonsense:

```cmake
find_file(CMSIS_${FAMILY}${CORE_U}_SOURCE
        NAMES system_stm32${FAMILY_L}xx.c
        PATHS "${CMSIS_${FAMILY}${CORE_U}_PATH}/Source/Templates"
        NO_DEFAULT_PATH
    )
    list(APPEND CMSIS_SOURCES "${CMSIS_${FAMILY}${CORE_U}_SOURCE}")
    
    if (NOT CMSIS_${FAMILY}${CORE_U}_SOURCE)
        continue()
    endif()
```

At around line `147`, change the `PATHS` from `${CMSIS_${FAMILY}${CORE_U}_PATH}/Source/Templates/gcc` to `${CMAKE_CURRENT_SOURCE_DIR}`

```cmake
find_file(CMSIS_${FAMILY}${CORE_U}_${TYPE}_STARTUP
            NAMES startup_stm32${TYPE_L}.s
            PATHS "${CMSIS_${FAMILY}${CORE_U}_PATH}/Source/Templates/gcc"
            NO_DEFAULT_PATH
        )
```

### CMakeLists.txt

Refer to the [examples](https://github.com/ObKo/stm32-cmake/tree/master/examples), we can write the CMakeLists.txt for out project.

```cmake
cmake_minimum_required(VERSION 3.15)

# Set project name
project("hello-nucleo-h743zi" C ASM)

# Specify MCU package path
set(STM32_CUBE_H7_PATH "$ENV{HOME}/STM32Cube/Repository/STM32Cube_FW_H7_V1.9.0")

# Find CMSIS and HAL/LL libraries
find_package(CMSIS COMPONENTS STM32H743ZI_M7 REQUIRED)
find_package(HAL COMPONENTS STM32H743ZI_M7 REQUIRED)

# Enumerate source files
file(GLOB PROJECT_SOURCES "Core/Src/*.c")

# Compile flashable binary
add_executable(${CMAKE_PROJECT_NAME} ${PROJECT_SOURCES})

# Link libraries
# Should be consistent with STM32CubeMX configurations
# See the generated "stm32h7xx_hal_conf.h"
target_link_libraries(${CMAKE_PROJECT_NAME} PRIVATE
    CMSIS::STM32::H743ZI::M7 
    HAL::STM32::H7::M7::CORTEX
    HAL::STM32::H7::M7::RCC
    HAL::STM32::H7::M7::RCCEx
    HAL::STM32::H7::M7::PWR
    HAL::STM32::H7::M7::PWREx
    HAL::STM32::H7::M7::GPIO
    HAL::STM32::H7::M7::EXTI
    HAL::STM32::H7::M7::UART
    HAL::STM32::H7::M7::UARTEx
    HAL::STM32::H7::M7::PCD
    HAL::STM32::H7::M7::PCDEx
    HAL::STM32::H7::M7::LL_USB
    STM32::NoSys
)

# Add include dir
target_include_directories(${CMAKE_PROJECT_NAME} PRIVATE
    Core/Inc
)
```

Note that the linked libraires should be consistent with the **Module Selection** section in the `stm32${device}_hal_conf.h`

### launch.json

Refer to the [Cortex-Debug docs](https://github.com/Marus/cortex-debug/wiki/ST-Link-(st-util)-Specific-Configuration) we can setup the `launch.json` file used to launch debug session.

We need to set the `executable` to `${command:cmake.launchTargetPath}` so the debug task will automatically trigger cmake build and locate the compiled flashable binary. Also we need to specify which MCU type we are using in `device`. Finally, provide the path to CMSIS-SVD file in `svdFile`.

Here is the complete configuration file:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "type": "cortex-debug",
            "request": "launch",
            "servertype": "stutil",
            "cwd": "${workspaceRoot}",
            // This will trigger cmake build
            "executable": "${command:cmake.launchTargetPath}",
            // CMSIS-SVD file
            "svdFile": "${workspaceRoot}/STM32H743x.svd",
            "name": "Debug (ST-Util)",
            // MCU device type
            "device": "STM32H743ZI",
            "v1": false,
            // Break at main
            "runToMain": true,
            // Variables inspection in HEX format
            "postLaunchCommands": ["set output-radix 16"]
        }
    ]
}
```

### Start debugging

Simply press `F5` and it just works!

![Run](images/run.avif)

We can also use `Cortex-Debug: View Disassembly (Function)` command to inspect the assembly code of a function.

## Limitations

If we want to use middlewares like RTOS or FATFS, there's no cmake scripts to locate them so we need to add them to our project manually.