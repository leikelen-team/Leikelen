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
using cl.uv.leikelen.API.Module.Input;

namespace cl.uv.leikelen.Module.Input.OpenBCI.View
{
    /// <summary>
    /// Lógica de interacción para OpenBCIWindow.xaml
    /// </summary>
    public partial class OpenBCIWindow : Window, ICloneable
    {
        public IMonitor Monitor { get; }
        public OpenBCIWindow(IMonitor monitor)
        {
            InitializeComponent();
            Monitor = monitor;
            RenewPorts();

            PortsRenewBtn.Click += PortsRenewBtn_Click;
            TestBtn.Click += TestBtn_Click;
            NextBtn.Click += NextBtn_Click;
            CancelBtn.Click += CancelBtn_Click;

        }

        public OpenBCIWindow(OpenBCIWindow openBCIwindow)
        {
            InitializeComponent();
            Monitor = openBCIwindow.Monitor;
            RenewPorts();

            PortsRenewBtn.Click += PortsRenewBtn_Click;
            TestBtn.Click += TestBtn_Click;
            NextBtn.Click += NextBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            var confPosition = new ConfigurePositions(this);
            if (!ReferenceEquals(null, PortsCmbx.SelectedItem))
            {
                OpenBCISettings.Instance.Notch.Write(NotchCmbx.SelectedIndex);
                OpenBCISettings.Instance.Filter.Write(FilterCmbx.SelectedIndex);
                Monitor.OpenPort(PortsCmbx.SelectedItem as string);

                //var confPosition = new ConfigurePositions(this);
                confPosition.Show();
                Hide();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(Properties.OpenBCI.NotSelectedPort, Properties.OpenBCI.Error, MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            //var confPosition = new ConfigurePositions();
            //confPosition.Show();
            //Hide();

        }

        private async void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isConnected = false;
            if(!ReferenceEquals(null, PortsCmbx.SelectedItem))
            {
                OpenBCISettings.Instance.Notch.Write(NotchCmbx.SelectedIndex);
                await Monitor.OpenPort(PortsCmbx.SelectedItem as string);
                if (Monitor.GetStatus() == InputStatus.Connected)
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
            PortsCmbx.Items.Clear();
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

        public object Clone()
        {
            return new OpenBCIWindow(this);
        }
    }
}
