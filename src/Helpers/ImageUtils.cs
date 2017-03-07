using Microsoft.Kinect;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cl.uv.leikelen.src.Helpers
{
    public static class ImageUtils
    {
        public static BitmapSource ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static RenderTargetBitmap ToBitmap(this DrawingImage drawingImage)
        {
            var image = new Image { Source = drawingImage };
            var bitmap = new RenderTargetBitmap(1920, 1080, 96, 96, PixelFormats.Pbgra32);
            image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height));
            bitmap.Render(image);
            return bitmap;
        }

        public static DrawingImage ToDrawingImage(this BitmapSource source)
        {
            Rect imageRect = new Rect(0, 0, source.PixelWidth, source.PixelHeight);
            ImageDrawing drawing = new ImageDrawing(source, imageRect);
            return new DrawingImage(drawing);
        }

        public async static void RunActionAfter(this Action<BitmapSource> action, BitmapSource bitmap, TimeSpan timeSpan)
        {
            await Task.Delay((int)timeSpan.TotalMilliseconds);
            action(bitmap);
        }
    }
}
