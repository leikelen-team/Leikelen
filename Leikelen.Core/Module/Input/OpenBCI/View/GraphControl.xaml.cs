using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Configurations;

namespace cl.uv.leikelen.Module.Input.OpenBCI.View
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;

        private int _frequency;
        private int _secondsToGraph;
        private int _milliDelay;

        private Queue<Tuple<DateTime, double>> _dataQueue;

        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        DateTime _startTime;

        public GraphControl()
        {
            InitializeComponent();

            _dataQueue = new Queue<Tuple<DateTime, double>>();
            _frequency = 256;
            _secondsToGraph = 5;
            _startTime = DateTime.Now;
            _milliDelay = 100;

            NameLabel.Content = "";

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value =>
            {
                if (value > 0)
                    return new DateTime((long)value).ToString("mm:ss");
                else
                    return "00:00";
            };

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

            _isActive = false;

            DataContext = this;
        }

        public GraphControl(string name, int frequency, int secondsToGraph, int secondsStep, int delayInMilliseconds)
        {
            InitializeComponent();

            _dataQueue = new Queue<Tuple<DateTime, double>>();
            _frequency = frequency;
            _secondsToGraph = secondsToGraph;
            _startTime = DateTime.Now;
            _milliDelay = delayInMilliseconds;

            NameLabel.Content = name;

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value =>
            {
                if (value > 0)
                    return new DateTime((long)value).ToString("mm:ss");
                else
                    return "00:00";
            };

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(secondsStep).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

            _isActive = false;

            DataContext = this;

        }

        public void Activate()
        {
            if (!_isActive)
            {
                _isActive = true;
                Task.Factory.StartNew(Read);
            }
        }

        public void Deactivate()
        {
            if (_isActive)
            {
                _isActive = false;
                Task.Factory.StartNew(FillEmpty);
            }
        }

        public void EnqueueData(double data)
        {
            _dataQueue.Enqueue(new Tuple<DateTime, double>(new DateTime(DateTime.Now.Ticks - _startTime.Ticks), data));
        }


        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(0).Ticks - _startTime.Ticks; // lets force the axis to be 0 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(_secondsToGraph).Ticks - _startTime.Ticks; // and x seconds behind
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public bool _isActive { get; set; }

        private void Read()
        {
            while (_isActive)
            {
                Thread.Sleep(_milliDelay);
                while (_dataQueue.Count > 0)
                {
                    Tuple<DateTime, double> data = _dataQueue.Dequeue();
                    if (!ReferenceEquals(data, null) && data.Item1.Ticks > 0)
                    {
                        ChartValues.Add(new MeasureModel
                        {
                            DateTime = data.Item1,
                            Value = data.Item2
                        });
                        //lets only use the last values
                        if (ChartValues.Count > (_frequency * _secondsToGraph)) ChartValues.RemoveAt(0);
                    }

                }
                SetAxisLimits(DateTime.Now);
            }
        }

        private void FillEmpty()
        {
            while (!_isActive)
            {
                Thread.Sleep(_milliDelay);
                _dataQueue.Clear();
                for (int i = 0; i < _frequency * (_milliDelay / 1000); i++)
                {
                    ChartValues.RemoveAt(0);
                }
                SetAxisLimits(DateTime.Now);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Activate();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Deactivate();
        }
    }

    public class MeasureModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
