using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.Helper
{
    public class WindowBuilder
    {
        private ICloneable _window;

        public WindowBuilder(ICloneable window)
        {
            _window = window;
        }

        public Window GetWindow()
        {
            return (Window) _window.Clone(); 
        }
    }
}
