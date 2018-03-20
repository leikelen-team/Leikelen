using cl.uv.leikelen.API.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cl.uv.leikelen.Module.Input.OpenBCI.View
{
    /// <summary>
    /// Lógica de interacción para LiveGraphTab.xaml
    /// </summary>
    public partial class LiveGraphTab : TabItem, ITab
    {
        private GraphForm _graphForm;
        public LiveGraphTab()
        {
            InitializeComponent();

            Header = Properties.OpenBCI.SensorName;
            _graphForm = new GraphForm();
            GraphFormContainer.Child = _graphForm;
        }

        public void Fill()
        {

        }

        public void Reset()
        {

        }

        public void Enqueue(double[] dataToGraph)
        {
            _graphForm.driver.Enqueue(dataToGraph);
        }
    }
}
