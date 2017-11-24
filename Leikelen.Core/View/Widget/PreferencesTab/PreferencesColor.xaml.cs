using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.View.Widget.PreferencesTab
{
    /// <summary>
    /// Lógica de interacción para PreferencesColor.xaml
    /// </summary>
    public partial class PreferencesColor : TabItem, IPreference
    {
        public PreferencesColor()
        {
            InitializeComponent();
        }

        public void Apply()
        {
        }
    }
}
