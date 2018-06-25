using cl.uv.leikelen.API.FrameProvider.EEG;
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
    /// Interaction logic for LiveGraphTab.xaml
    /// </summary>
    public partial class LiveGraphTab : TabItem, ITab
    {
        private string[] _positions;
        private Util.Filter _filter;
        private NotchType _notchType;
        private FilterType _filterType;
        GraphControl[] graphs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveGraphTab"/> class.
        /// </summary>
        /// <param name="color">The color of the graph.</param>
        public LiveGraphTab(Color color)
        {
            InitializeComponent();
            Header = Properties.OpenBCI.SensorName;

            graphs = new GraphControl[12];
            _notchType = NotchType.None;
            _filterType = FilterType.None;

            graphs[0] = grc0;
            graphs[1] = grc1;
            graphs[2] = grc2;
            graphs[3] = grc3;
            graphs[4] = grc4;
            graphs[5] = grc5;
            graphs[6] = grc6;
            graphs[7] = grc7;
            graphs[8] = grc8;
            graphs[9] = grc9;
            graphs[10] = grc10;
            graphs[11] = grc11;

            for (int i = 0; i < 12; i++)
            {
                graphs[i].ActivateCheckbox.IsChecked = true;
                graphs[i].AddColor(color);
            }
        }

        void ITab.Fill()
        {

        }

        void ITab.Reset()
        {

        }

        /// <summary>
        /// Sets the position names for each graph.
        /// </summary>
        /// <param name="positions">The positions names.</param>
        public void SetPositions(string[] positions)
        {
            _positions = positions;
            for (int i = 1; i < 9; i++)
            {
                if (graphs[i] is GraphControl graph && _positions?.Length >= i)
                {
                    graph.NameLabel.Content = _positions[i];
                }
            }
        }

        /// <summary>
        /// Enqueues the specified data to graph.
        /// </summary>
        /// <param name="dataToGraph">The data to graph.</param>
        public void Enqueue(double[] dataToGraph)
        {
            if ((_notchType != NotchType.None || _filterType != FilterType.None) && dataToGraph?.Length >= 9)
            {
                for (int j = 1; j < 9; j++)
                {
                    dataToGraph[j] = _filter.FiltersSelect(_filterType,
                                        _notchType, dataToGraph[j], j - 1);
                }
            }
            int i = 0;
            foreach (GraphControl graph in graphs)
            {
                if (i < 12)
                    graph.EnqueueData(dataToGraph[i]);
                i++;
            }
        }
    }
}
