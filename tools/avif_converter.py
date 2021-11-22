from Foundation import NSData, NSURL, NSMutableData
import Quartz
from Quartz import CIContext, CIImage, CIFilter

from _libavif_cffi import ffi
from _libavif_cffi import lib as avif

from typing import Tuple

import os
from pathlib import Path
from argparse import ArgumentParser

SUPPORTED_IMAGE_EXTENSIONS = [
    ".jpg", ".jpeg", ".tiff", ".png", ".bmp", ".heic",
]


class AVIFConverter():
    """
    Convert HEIC/JPEG/PNG/TIFF/BMP images to AVIF.
    The image reader uses macOS's system APIs,
    requres 'pyobjc-framework-Quartz' package for the bridging.
    """

    def __init__(self,
                 depth_bits: int = 8,
                 chroma_subsampling: str = "420",
                 lossless: bool = False,
                 min_q: int = 8,
                 max_q: int = 16,
                 max_size: int = 1920,
                 ignore_icc: bool = False) -> None:
        # Setup CIContext.
        options = {
            Quartz.kCIContextWorkingColorSpace: Quartz.CGColorSpaceCreateWithName(
                Quartz.kCGColorSpaceDisplayP3)
        }

        self.context = CIContext.contextWithOptions_(options)
        if not self.context:
            raise RuntimeError("Failed to create CIContext")

        self.depth_bits = depth_bits
        self.chroma_subsampling = avif.AVIF_PIXEL_FORMAT_YUV444 if lossless else self._get_avif_chroma_subsampling_format(
            chroma_subsampling)
        self.lossless = lossless
        self.min_q = 0 if lossless else min_q
        self.max_q = 0 if lossless else max_q

        self.max_size = max_size
        self.num_threads = os.cpu_count()
        self.ignore_icc = ignore_icc

    def convert(self, input_path: Path, output_path: Path) -> int:
        # Load CIImage from file.
        url = NSURL.fileURLWithPath_(str(input_path))
        options = {
            "kCIImageApplyOrientationProperty": True,
            "kCIImageCacheHint": False,
            "kCIImageCacheImmediately": False,
            "kCIImageAVDepthData": None,
        }
        ci_image = CIImage.imageWithContentsOfURL_options_(url, options)

        w = int(ci_image.extent().size.width)
        h = int(ci_image.extent().size.height)
        colorspace = ci_image.colorSpace()
        properties = ci_image.properties()

        has_alpha = Quartz.kCGImagePropertyHasAlpha in properties and properties[
            Quartz.kCGImagePropertyHasAlpha]

        # Resize if original image is too large.
        if max(w, h) > self.max_size:
            scale = self.max_size / max(w, h)
            resize_filter = CIFilter.filterWithName_("CILanczosScaleTransform")
            resize_filter.setInputImage_(ci_image)
            resize_filter.setInputScale_(scale)
            resize_filter.setInputAspectRatio_(1.0)
            ci_image = resize_filter.outputImage()
            w = int(ci_image.extent().size.width)
            h = int(ci_image.extent().size.height)

        # Create pixel buffer.
        row_bytes = 4 * w * (2 if self.depth_bits > 8 else 1)
        pixel_buffer = NSMutableData.dataWithLength_(h * row_bytes)

        ci_format = Quartz.kCIFormatRGBA16 if self.depth_bits > 8 else Quartz.kCIFormatRGBA8

        # Render CIImage into pixel buffer.
        self.context.render_toBitmap_rowBytes_bounds_format_colorSpace_(
            ci_image,
            pixel_buffer,
            row_bytes,
            ci_image.extent(),
            ci_format,
            colorspace
        )

        # Create an AVIF image.
        avif_img = avif.avifImageCreate(
            w, h, self.depth_bits, self.chroma_subsampling)

        # Specify colorspace.
        cp, tc, mc = self._get_cicp_tuple(colorspace)

        # Set CICP profile.
        avif_img.colorPrimaries = cp
        avif_img.transferCharacteristics = tc
        avif_img.matrixCoefficients = avif.AVIF_MATRIX_COEFFICIENTS_IDENTITY if self.lossless else mc

        if not self.ignore_icc:
            # Set ICC profile.
            icc_profile = Quartz.CGColorSpaceCopyICCProfile(colorspace)
            p_icc_profile = ffi.from_buffer(icc_profile.bytes())
            avif.avifImageSetProfileICC(
                avif_img, p_icc_profile, icc_profile.length())

        avif_img.yuvRange = avif.AVIF_RANGE_FULL
        avif_img.alphaRange = avif.AVIF_RANGE_FULL
        avif_img.alphaPremultiplied = False

        # Create rgb image from pixel buffer
        rgb_img = ffi.new("avifRGBImage*")
        avif.avifRGBImageSetDefaults(rgb_img, avif_img)
        rgb_img.ignoreAlpha = not has_alpha
        rgb_img.pixels = ffi.from_buffer(pixel_buffer)
        rgb_img.rowBytes = row_bytes
        rgb_img.depth = 16 if self.depth_bits > 8 else 8

        # RGB -> YUV conversion.
        err = avif.avifImageRGBToYUV(avif_img, rgb_img)
        assert err == 0, f"Failed to do RGB -> YUV conversion: {ffi.string(avif.avifResultToString(err))}."

        # Create encoder.
        encoder = avif.avifEncoderCreate()
        encoder.maxThreads = self.num_threads
        encoder.minQuantizer = self.min_q
        encoder.maxQuantizer = self.max_q
        encoder.minQuantizerAlpha = self.min_q
        encoder.maxQuantizerAlpha = self.max_q
        encoder.speed = avif.AVIF_SPEED_DEFAULT

        # Add single image.
        err = avif.avifEncoderAddImage(
            encoder, avif_img, 1, avif.AVIF_ADD_IMAGE_FLAG_SINGLE)
        assert err == 0, f"Failed to add image to encoder: {ffi.string(avif.avifResultToString(err))}."

        # Wait for encoder to finish.
        encoded_data = ffi.new("avifRWData*")
        err = avif.avifEncoderFinish(encoder, encoded_data)
        assert err == 0, f"Failed to encode: {ffi.string(avif.avifResultToString(err))}."

        size_output = encoded_data.size

        # Write to AVIF file.
        with open(output_path, "wb") as f:
            output_buffer = ffi.buffer(encoded_data.data, encoded_data.size)
            f.write(output_buffer)

        # Release resources.
        avif.avifImageDestroy(avif_img)
        avif.avifEncoderDestroy(encoder)
        avif.avifRWDataFree(encoded_data)

        # Output AVIF file size in bytes.
        return size_output

    @staticmethod
    def _get_avif_chroma_subsampling_format(chroma_subsampling: str) -> int:
        if chroma_subsampling == "444":
            return avif.AVIF_PIXEL_FORMAT_YUV444
        if chroma_subsampling == "422":
            return avif.AVIF_PIXEL_FORMAT_YUV422
        if chroma_subsampling == "420":
            return avif.AVIF_PIXEL_FORMAT_YUV420
        if chroma_subsampling == "400":
            return avif.AVIF_PIXEL_FORMAT_YUV400
        return avif.AVIF_PIXEL_FORMAT_NONE

    @staticmethod
    def _get_cicp_tuple(colorspace) -> Tuple[int, int, int]:
        key = Quartz.CGColorSpaceGetName(colorspace)
        if key == Quartz.kCGColorSpaceDisplayP3:
            return (
                avif.AVIF_COLOR_PRIMARIES_SMPTE432,
                avif.AVIF_TRANSFER_CHARACTERISTICS_SRGB,
                avif.AVIF_MATRIX_COEFFICIENTS_BT709
            )
        if key == Quartz.kCGColorSpaceDisplayP3_HLG:
            return (
                avif.AVIF_COLOR_PRIMARIES_SMPTE432,
                avif.AVIF_TRANSFER_CHARACTERISTICS_HLG,
                avif.AVIF_MATRIX_COEFFICIENTS_BT709
            )
        if key == Quartz.kCGColorSpaceExtendedLinearDisplayP3:
            return (
                avif.AVIF_COLOR_PRIMARIES_SMPTE432,
                avif.AVIF_TRANSFER_CHARACTERISTICS_LINEAR,
                avif.AVIF_MATRIX_COEFFICIENTS_BT709
            )
        if key == Quartz.kCGColorSpaceSRGB:
            return (
                avif.AVIF_COLOR_PRIMARIES_BT709,
                avif.AVIF_TRANSFER_CHARACTERISTICS_SRGB,
                avif.AVIF_MATRIX_COEFFICIENTS_BT709
            )
        if key == Quartz.kCGColorSpaceDCIP3:
            return (
                avif.AVIF_COLOR_PRIMARIES_SMPTE432,
                avif.AVIF_TRANSFER_CHARACTERISTICS_SMPTE2084,
                avif.AVIF_MATRIX_COEFFICIENTS_BT709
            )
        if key == Quartz.kCGColorSpaceITUR_2020:
            return (
                avif.AVIF_COLOR_PRIMARIES_BT2020,
                avif.AVIF_TRANSFER_CHARACTERISTICS_BT2020_10BIT,
                avif.AVIF_MATRIX_COEFFICIENTS_BT2020_NCL
            )
        return (
            avif.AVIF_COLOR_PRIMARIES_UNSPECIFIED,
            avif.AVIF_TRANSFER_CHARACTERISTICS_UNSPECIFIED,
            avif.AVIF_MATRIX_COEFFICIENTS_BT709
        )


def parse_args():
    parser = ArgumentParser()

    parser.add_argument(
        "--input", "-i",
        type=Path,
        required=True,
        help="Path to the file/dir to bec onverted."
    )
    parser.add_argument(
        "--output_dir", "-o",
        nargs="?",
        type=Path,
        help="Path to the output dir."
    )
    parser.add_argument(
        "--depth", "-d",
        type=int,
        default=10,
        help="Colordepth bits [8, 10, 12]."
    )
    parser.add_argument(
        "--lossless", "-L",
        action="store_true",
        help="Enable lossless compression."
    )
    parser.add_argument(
        "--chroma", "-c",
        type=str,
        default="420",
        help="Chroma subsampling mode [400, 420, 422, 444]."
    )
    parser.add_argument(
        "--min_q",
        type=int,
        default=8,
        help="Min quantization level (0-63)."
    )
    parser.add_argument(
        "--max_q", "-Q",
        type=int,
        default=32,
        help="Max quantization level (0-63)."
    )
    parser.add_argument(
        "--max_size", "-s",
        type=int,
        default=2048,
        help="Max longside length."
    )
    parser.add_argument(
        "--ignore_icc", "--no_icc",
        action="store_true",
        help="Do not include ICC profile in output image."
    )

    return parser.parse_args()


if __name__ == "__main__":
    args = parse_args()
    converter = AVIFConverter(
        depth_bits=args.depth,
        chroma_subsampling=args.chroma,
        lossless=args.lossless,
        min_q=args.min_q,
        max_q=args.max_q,
        max_size=args.max_size,
        ignore_icc=args.ignore_icc,
    )

    input_path: Path = args.input
    if input_path.is_dir():
        for d in input_path.iterdir():
            # Filter non-image entries.
            if not (d.is_file() and d.suffix.lower() in SUPPORTED_IMAGE_EXTENSIONS):
                continue

            output_path = d.with_suffix(".avif")
            if args.output_dir:
                output_path = args.output_dir / output_path.name

            size = converter.convert(d, output_path)
            print(f"[Converted] {output_path}, {size // 1024} KB.")
    elif input_path.is_file() and input_path.suffix.lower() in SUPPORTED_IMAGE_EXTENSIONS:
        output_path = input_path.with_suffix(".avif")
        if args.output_dir:
            output_path = args.output_dir / output_path.name

        size = converter.convert(input_path, output_path)
        print(f"[Converted] {output_path}, {size // 1024} KB.")
    else:
        raise ValueError(f"蚌埠住了😅，自己看看 {input_path} 是个啥玩意儿好吗！")

    print("[All done! 🦦]")
