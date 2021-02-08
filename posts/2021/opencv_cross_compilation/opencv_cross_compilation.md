# 为 Rockchip 嵌入式 Linux 平台交叉编译 OpenCV

## 简介
使用 LLVM 工具链为 Rockchip ARM Linux 嵌入式平台进行 OpenCV 的交叉编译。

## 环境
### Host:
- Platform: `x86_64`
- OS: `macOS 10.15` ~~特卡琳娜~~
- Toolchain: `LLVM 11.0`
- Builder: `CMake 3.19.x`

### Target: 
- Platform: `AArch64`
- OS: `Arch Linux ARM`
- Dependencies: `pkg-config`, `openblas`, `python3`

## 配置文件

### CMake 工具链文件

```cmake
# Specify target environment
set(CMAKE_SYSTEM_NAME Linux)
set(CMAKE_SYSTEM_PROCESSOR aarch64)

# Specify LLVM as cross-compilation toolchain
set(LLVM_ROOT "/usr/local/opt/llvm")
set(CLANG_TARGET_TRIPLE aarch64-linux-gnu)

set(CMAKE_C_COMPILER ${LLVM_ROOT}/bin/clang)
set(CMAKE_C_COMPILER_TARGET ${CLANG_TARGET_TRIPLE})

set(CMAKE_CXX_COMPILER ${LLVM_ROOT}/bin/clang++)
set(CMAKE_CXX_COMPILER_TARGET ${CLANG_TARGET_TRIPLE})

set(CMAKE_ASM_COMPILER ${LLVM_ROOT}/bin/clang)
set(CMAKE_ASM_COMPILER_TARGET ${CLANG_TARGET_TRIPLE})

set(CMAKE_POSITION_INDEPENDENT_CODE ON)

# Use LLVM's lld linker
set(CMAKE_EXE_LINKER_FLAGS "-fuse-ld=lld")
set(CMAKE_SHARED_LINKER_FLAGS "-fuse-ld=lld")

# Don't run the linker on compiler check
set(CMAKE_TRY_COMPILE_TARGET_TYPE STATIC_LIBRARY)

# Specify sysroot of target platform
set(SYSROOT "/Users/goose_bomb/Documents/DIG/embedded/sysroot")
set(CMAKE_SYSROOT ${SYSROOT})

# Specify pkg-config search path
set(ENV{PKG_CONFIG_PATH} ${SYSROOT}/usr/lib/pkgconfig)
set(ENV{PKG_CONFIG_LIBDIR} ${SYSROOT}/usr/lib/pkgconfig)
set(ENV{PKG_CONFIG_SYSROOT_DIR} ${CMAKE_SYSROOT})

# Specify cmake search path
set(CMAKE_FIND_ROOT_PATH ${SYSROOT})
set(CMAKE_FIND_ROOT_PATH_MODE_PROGRAM NEVER)
set(CMAKE_FIND_ROOT_PATH_MODE_LIBRARY ONLY)
set(CMAKE_FIND_ROOT_PATH_MODE_INCLUDE ONLY)
set(CMAKE_FIND_ROOT_PATH_MODE_PACKAGE ONLY)
```

### 编译命令行选项

```shell
cmake .. \
-G "Unix Makefiles" \
-D CMAKE_TOOLCHAIN_FILE=toolchain.cmake \
-D CMAKE_BUILD_TYPE=RELEASE \
-D BUILD_SHARED_LIBS=ON \
-D CMAKE_INSTALL_PREFIX="/usr" \
-D BUILD_opencv_python3=ON \
-D BUILD_opencv_python2=OFF \
-D CPU_BASELINE=NEON \
-D OPENCV_ENABLE_NONFREE=OFF \
-D WITH_LAPACK=ON \
-D BUILD_JAVA=OFF \
-D WITH_GTK=OFF \
-D WITH_GTK_2_X=OFF \
-D WITH_ITT=ON \
-D WITH_TBB=ON \
-D WITH_CUDA=OFF \
-D WITH_OPENCL=ON \
-D WITH_OPENCLAMDFFT=OFF \
-D WITH_OPENCLAMDBLAS=OFF \
-D WITH_QUIRC=ON \
-D WITH_TENGINE=ON \
-D BUILD_opencv_legacy=OFF \
-D BUILD_opencv_js=OFF \
-D BUILD_opencv_ts=OFF \
-D BUILD_opencv_apps=OFF \
-D BUILD_opencv_ts=OFF \
-D BUILD_opencv_objdetect=OFF \
-D BUILD_opencv_shape=OFF \
-D BUILD_opencv_photo=OFF \
-D BUILD_opencv_highgui=OFF \
-D BUILD_opencv_stitching=OFF \
-D BUILD_opencv_calib3d=ON \
-D BUILD_opencv_videostab=OFF \
-D BUILD_opencv_videoio=ON \
-D BUILD_opencv_apps=OFF \
-D WITH_GSTREAMER=OFF \
-D WITH_FFMEPG=ON \
-D WITH_V4L=ON \
-D WITH_PROTOBUF=ON \
-D WITH_OPENEXR=OFF \
-D BUILD_OPENEXR=OFF \
-D WITH_OPENJPEG=OFF \
-D BUILD_OPENJPEG=OFF \
-D WITH_IMGCODEC_SUNRASTER=OFF \
-D WITH_1394=OFF \
-D WITH_JASPER=OFF \
-D BUILD_JASPER=OFF \
-D WITH_WEBP=OFF \
-D WITH_TIFF=OFF \
-D BUILD_TIFF=OFF \
-D BUILD_TESTS=OFF \
-D BUILD_PERF_TESTS=OFF \
-D BUILD_EXAMPLES=OFF \
-D INSTALL_C_EXAMPLES=OFF \
-D INSTALL_PYTHON_EXAMPLES=OFF
```

## 坑 
需要在 CMake 生成的 `link.txt` 与 `relink.txt` 中在有`-lopenblas` 的地方手动添加链接选项: `-lcblas` `-llapack`，不然运行时找不到一些符号！