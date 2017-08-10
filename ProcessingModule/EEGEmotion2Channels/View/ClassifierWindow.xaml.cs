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

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.View
{
    /// <summary>
    /// Lógica de interacción para ClassifierWindow.xaml
    /// </summary>
    public partial class ClassifierWindow : Window, ICloneable
    {
        public ClassifierWindow()
        {
            InitializeComponent();
        }

        public object Clone()
        {
            return new ClassifierWindow();
        }
    }
}
