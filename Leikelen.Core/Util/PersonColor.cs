using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cl.uv.leikelen.Util
{
    public class PersonColor
    {
        public Color MainColor { get; private set; }
        public Color SecondaryColor { get; private set; }

        public int PersonId { get; private set; }
        public ulong TrackingId { get; private set; }

        public PersonColor(int personId, ulong trackingId, int indexColor)
        {
            PersonId = personId;
            TrackingId = trackingId;

            MainColor = _mainColors[indexColor % _mainColors.Length];
            SecondaryColor = _secColors[indexColor % _secColors.Length];
        }

        private readonly static Color[] _mainColors = new Color[]
        {
            Colors.Blue,
            Colors.Green,
            Colors.Gray,
            Colors.Yellow,
            Colors.Orange,
            Colors.Red,
            Colors.Pink,
            Colors.Lime,
            Colors.Salmon,
            Colors.SeaGreen,
            Colors.Turquoise,
            Colors.Violet,
            Colors.SlateBlue,
            Colors.SkyBlue,
            Colors.SlateGray,
            Colors.Orchid,
            Colors.Khaki,
            Colors.Goldenrod,
            Colors.Olive,
            Colors.Magenta,
            Colors.DarkCyan
        };

        private readonly static Color[] _secColors = new Color[]
        {
            Colors.DarkBlue,
            Colors.DarkGreen,
            Colors.DarkGray,
            Colors.YellowGreen,
            Colors.DarkOrange,
            Colors.DarkRed,
            Colors.DeepPink,
            Colors.LimeGreen,
            Colors.DarkSalmon,
            Colors.DarkSeaGreen,
            Colors.DarkTurquoise,
            Colors.DarkViolet,
            Colors.DarkSlateBlue,
            Colors.DeepSkyBlue,
            Colors.DarkSlateGray,
            Colors.DarkOrchid,
            Colors.DarkKhaki,
            Colors.DarkGoldenrod,
            Colors.DarkOliveGreen,
            Colors.DarkMagenta,
            Colors.DarkCyan
        };

    }
}
