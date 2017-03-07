using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace cl.uv.leikelen.src.Helpers
{
    public static class ImageFileUtils
    {
        public static void ToImageFile(this BitmapSource btmSource, string path)
        {
            BitmapEncoder encoder;
            if (path.EndsWith(".jpg") | path.EndsWith(".jpeg"))
                encoder = new JpegBitmapEncoder();
            else if (path.EndsWith(".png"))
                encoder = new PngBitmapEncoder();
            else
                throw new Exception("Only jpg and png files are valid");

            encoder.Frames.Add(BitmapFrame.Create(btmSource));
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fs);
            }

        }
    }
}
