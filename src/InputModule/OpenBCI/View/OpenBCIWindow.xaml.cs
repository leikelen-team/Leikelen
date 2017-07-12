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
using System.IO.Ports;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    /// <summary>
    /// Lógica de interacción para OpenBCIWindow.xaml
    /// </summary>
    public partial class OpenBCIWindow : Window
    {
        public OpenBCIWindow()
        {
            InitializeComponent();

            int i = 0;
            foreach (string s in SerialPort.GetPortNames())
            {
                PortsCmbx.Items.Add(s);
                if (i == 0)
                {
                    PortsCmbx.SelectedItem = s;
                    i++;
                }
            }
        }
    }
}
