using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    static class Utils
    {



        public static string ToShortForm(this TimeSpan t)
        {
            string shortForm = "";
            if (t.Hours > 0)
            {
                shortForm += t.Hours.ToString("00") + ":";
            }
            //if (t.Minutes > 0 || t.Hours > 0)
            //{
                shortForm += t.Minutes.ToString("00") + ":";
            //}
            //if (t.Seconds > 0)
            //{
            shortForm += t.Seconds.ToString("00");
            //}
            return shortForm;
            //return t.ToString(@"hh\:mm\:ss");
        }

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
            //System.Drawing.Bitmap btm = null;
            //btm.Save();
            var image = new Image { Source = drawingImage };
            //int width = (int)drawingImage.Width;
            //int height = (int)drawingImage.Height;
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

        //public static void SaveDrawingToFile(Drawing drawing, string fileName, double scale)
        //{
        //    var drawingImage = new Image { Source = new DrawingImage(drawing) };
        //    var width = drawing.Bounds.Width * scale;
        //    var height = drawing.Bounds.Height * scale;
        //    drawingImage.Arrange(new Rect(0, 0, width, height));

        //    var bitmap = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
        //    bitmap.Render(drawingImage);

        //    var encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bitmap));

        //    using (var stream = new FileStream(fileName, FileMode.Create))
        //    {
        //        encoder.Save(stream);
        //    }
        //}

        //public static void ToImageFile(this DrawingImage drawingImage, string path)
        //{
        //    var image = new Image { Source = drawingImage };
        //    //int width = (int)drawingImage.Width;
        //    //int height = (int)drawingImage.Height;
        //    var bitmap = new RenderTargetBitmap(1920, 1080, 96, 96, PixelFormats.Pbgra32);
        //    image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height));
        //    bitmap.Render(image);


        //    //String time = System.DateTime.Now.ToString("HH-mm-ss-fff",
        //    //            System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        //    //String path = "RecImages/body_" + time + ".jpg";

        //    var encoder = new PngBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(bitmap));
        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        encoder.Save(stream);
        //    }
        //}



        public static void ToImageFile(this BitmapSource btmSource, string path)
        {
            BitmapEncoder encoder;
            if (path.EndsWith(".jpg") | path.EndsWith(".jpeg"))
                encoder = new JpegBitmapEncoder();
            else if(path.EndsWith(".png"))
                encoder = new PngBitmapEncoder();
            else
                throw new Exception("Only jpg and png files are valid");

            encoder.Frames.Add(BitmapFrame.Create(btmSource));
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fs);
            }
            
        }

        public static string GetStringTime()
        {
            return System.DateTime.Now.ToString("HH-mm-ss-fff", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }

        public static string GetStringTime(this DateTime dateTime) {
            return dateTime.ToString("HH-mm-ss-fff", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }
        public static string GetStringTime(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format, System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }


        public async static void RunActionAfter(this Action<BitmapSource> action, BitmapSource bitmap, TimeSpan timeSpan)
        {
            await Task.Delay((int)timeSpan.TotalMilliseconds);
            action(bitmap);
        }

        public static System.Windows.Forms.Control GetControl(this Form form, string name)
        {
            return form.Controls.Find(name, true).FirstOrDefault();
        }

    }
}
