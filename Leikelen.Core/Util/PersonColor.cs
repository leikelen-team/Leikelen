using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace cl.uv.leikelen.Util
{
    public static class PersonColor
    {
        private static int _index = -1;

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

        public static Tuple<Color, Color> GetNewColors()
        {
            _index++;
            return new Tuple<Color, Color>(_mainColors[_index % _mainColors.Length], _secColors[_index % _secColors.Length]);
        }

        public static void Reset()
        {
            _index = -1;
        }

    }
}
