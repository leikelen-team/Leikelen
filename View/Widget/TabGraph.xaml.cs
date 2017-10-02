using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace cl.uv.leikelen.View.Widget
{
    /// <summary>
    /// Lógica de interacción para TabGraph.xaml
    /// </summary>
    public partial class TabGraph : TabItem, INotifyPropertyChanged
    {
        private bool _SeatedSeriesVisibility;
        private bool _HandOpenSeriesVisibility;
        private bool _HandDownSeriesVisibility;
        private bool _PointSeriesVisibility;
        private bool _StandedSeriesVisibility;
        private bool _ArmCrossSeriesVisibility;

        public TabGraph()
        {
            InitializeComponent();

            SeatedSeriesVisibility = true;
            HandOpenSeriesVisibility = true;
            HandDownSeriesVisibility = false;
            PointSeriesVisibility = false;
            StandedSeriesVisibility = false;
            ArmCrossSeriesVisibility = false;

            DataContext = this;
        }
        public bool SeatedSeriesVisibility
        {
            get { return _SeatedSeriesVisibility; }
            set
            {
                _SeatedSeriesVisibility = value;
                OnPropertyChanged("SeatedSeriesVisibility");
            }
        }

        public bool HandOpenSeriesVisibility
        {
            get { return _HandOpenSeriesVisibility; }
            set
            {
                _HandOpenSeriesVisibility = value;
                OnPropertyChanged("HandOpenSeriesVisibility");
            }
        }

        public bool HandDownSeriesVisibility
        {
            get { return _HandDownSeriesVisibility; }
            set
            {
                _HandDownSeriesVisibility = value;
                OnPropertyChanged("HandDownSeriesVisibility");
            }
        }

        public bool PointSeriesVisibility
        {
            get { return _PointSeriesVisibility; }
            set
            {
                _PointSeriesVisibility = value;
                OnPropertyChanged("PointSeriesVisibility");
            }
        }

        public bool StandedSeriesVisibility
        {
            get { return _StandedSeriesVisibility; }
            set
            {
                _StandedSeriesVisibility = value;
                OnPropertyChanged("StandedSeriesVisibility");
            }
        }

        public bool ArmCrossSeriesVisibility
        {
            get { return _ArmCrossSeriesVisibility; }
            set
            {
                _ArmCrossSeriesVisibility = value;
                OnPropertyChanged("ArmCrossSeriesVisibility");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
