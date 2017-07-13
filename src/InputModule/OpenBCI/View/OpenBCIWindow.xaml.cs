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
using cl.uv.leikelen.src.API.InputModule;

namespace cl.uv.leikelen.src.InputModule.OpenBCI.View
{
    /// <summary>
    /// Lógica de interacción para OpenBCIWindow.xaml
    /// </summary>
    public partial class OpenBCIWindow : Window
    {
        private IMonitor _monitor;
        public OpenBCIWindow(IMonitor monitor)
        {
            InitializeComponent();
            _monitor = monitor;
            RenewPorts();

            PortsRenewBtn.Click += PortsRenewBtn_Click;
            TestBtn.Click += TestBtn_Click;
            AcceptBtn.Click += AcceptBtn_Click;
            CancelBtn.Click += CancelBtn_Click;

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PortsCmbx.SelectedItem != null)
            {
                OpenBCI_Settings.Instance.Notch.Write(NotchCmbx.SelectedIndex);
                _monitor.OpenPort(PortsCmbx.SelectedItem as string);
            }
            this.Close();
        }

        private async void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isConnected = false;
            if(PortsCmbx.SelectedItem != null)
            {
                OpenBCI_Settings.Instance.Notch.Write(NotchCmbx.SelectedIndex);
                await _monitor.OpenPort(PortsCmbx.SelectedItem as string);
                if (_monitor.GetStatus() == API.InputModule.InputStatus.Connected)
                {
                    TestIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;
                    StatusLabel.Content = Properties.OpenBCI.Connected;
                    isConnected = true;
                }
                 //await _monitor.Close();
            }
            if(isConnected == false)
            {
                TestIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                StatusLabel.Content = Properties.OpenBCI.NotConnected;
            }
        }

        private void PortsRenewBtn_Click(object sender, RoutedEventArgs e)
        {
            RenewPorts();
        }

        private void RenewPorts()
        {
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
