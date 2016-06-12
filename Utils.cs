using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    static class Utils
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

        public static void SaveDrawingToFile(Drawing drawing, string fileName, double scale)
        {
            var drawingImage = new Image { Source = new DrawingImage(drawing) };
            var width = drawing.Bounds.Width * scale;
            var height = drawing.Bounds.Height * scale;
            drawingImage.Arrange(new Rect(0, 0, width, height));

            var bitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(drawingImage);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        public static void ToImageFile(this DrawingImage drawingImage, string path)
        {
            var image = new Image { Source = drawingImage };
            int width = (int)drawingImage.Width;
            int height = (int)drawingImage.Height;
            var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height));
            bitmap.Render(image);

            //String time = System.DateTime.Now.ToString("HH-mm-ss-fff",
            //            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
            //String path = "RecImages/body_" + time + ".jpg";

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

    }
}
