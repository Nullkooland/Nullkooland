---
type: "technical"
date: "2021-03-04T23:07:13+08:00"
title: "Use libheif to read HEIF Images"
brief: "早就看那一堆 png 测试图片不爽了！"
headerImage: "bathing_moonbear.avif"
tags: ["OpenCV", "HEIF"]
---
# Use `libheif` to read HEIF Images
## Why?
Because the photos I've taken using my 林檎爪机（大嘘）are all in `HEIF/HEIC` format, and I am more than bothered to convert them into antiquated `JPEG` or huge `PNG` formats when I feel like taking some of them as the test images in my computer vision algorithms demos.

## How?
I found a library to decode/encode `HEIF` images here: [libheif](https://github.com/strukturag/libheif) here, great.

On macOS, it is readily available in `homebrew` 🍺
```shell
brew install libheif
```

### Use with Python
Install the `pyheif` binding package using `pip`
```shell
pip install pyheif
```

Sadly this package doesn't provide a method to directly convert the decoded raw pixels into numpy array, so I add a helper method in `site-packages/pyheif/reader.py`:
```python
import numpy as np
from PIL import Image

# ...

def read_as_numpy(fp, apply_transformations=True, convert_hdr_to_8bit=True):
    heif_file = read(fp, apply_transformations, convert_hdr_to_8bit)
    img = Image.frombytes(
        heif_file.mode,
        heif_file.size,
        heif_file.data,
        "raw",
        heif_file.mode,
        heif_file.stride,
    )
    return np.array(img)
```
Yeah, I never liked `Pillow` anyway...

Now we can use it with `OpenCV` or `matplotlib` happily!
```python
import cv2
import matplotlib.pyplot as plt
import pyheif

img_rgb = pyheif.read_as_numpy("./images/nullko_the_moonbear.heic")
img_bgr = cv2.cvtColor(img_rgb, cv2.COLOR_RGB2BGR)

plt.imshow(img_rgb)
plt.show()

cv2.imshow("Hello HEIF", img_bgr)
cv2.waitKey()
```

### Use with C++
In `CMakeLists.txt` of the `CMake` project, add
```cmake
find_package(PkgConfig REQUIRED)
pkg_search_module(HEIF REQUIRED libheif)

# ...

target_include_directories(${target_name} PRIVATE 
    # ...
    ${HEIF_INCLUDE_DIRS}
)

target_link_libraries(${target_name} PRIVATE 
    # ...
    ${HEIF_LINK_LIBRARIES}
)
```
Then we can use `libheif` in our c++ files:
```cpp
#include "libheif/heif.h"
#include "libheif/heif_cxx.h"
// ...

auto heifContext = heif::Context();
heifContext.read_from_file("./images/snow_leopard.heic");
auto heifHandle = heifContext.get_primary_image_handle();
auto heifImage = heifHandle.decode_image(heif_colorspace_RGB,
                                         heif_chroma_interleaved_RGB);

int width = heifImage.get_width(heif_channel_interleaved);
int height = heifImage.get_height(heif_channel_interleaved);

int stride;
uint8_t* pixels = heifImage.get_plane(heif_channel_interleaved, &stride);

// Wrap the pixel buffer into cv::Mat 
auto img_src = cv::Mat(height, width, CV_8UC3, pixels, stride);

// OpenCV just prefer BGR over RGB, what a spoiled kid!
cv::cvtColor(img_src, img_src, cv::COLOR_RGB2BGR);
cv::imshow("HEIF (AVIF: XD) rocks and JPEG sucks", src_img);
```

### Command line tool
The `libheif` also ships with a handy command line tool to convert images in other formats into `HEIF` or `AVIF` images.
```shell
heif-enc kingfisher.png -q 80 -o emperorfisher.heic
heif-enc stone_bunny.png -q 80 --avif -o cool_bunny.avif 
heif-enc yunshan_angry.png --avif --lossless -p chroma=444 --matrix_coefficients=0 -o yunshan_euphoric.avif
```

Sadly the `AVIF` encoding is slow as 💩 now...

(I'm using an `AVIF` image as the header image of this post, if your browser cannot display it, well, that means your browser sucks and should be deprecated（暴言） XD.)