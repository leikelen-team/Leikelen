﻿using System;
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
using LiveCharts;
using LiveCharts.Wpf;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.View.Widget.HomeTab.Widget
{
    /// <summary>
    /// Interaction logic for IntervalGraphControl.xaml
    /// </summary>
    public partial class IntervalGraphControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalGraphControl"/> class.
        /// </summary>
        public IntervalGraphControl()
        {
            InitializeComponent();
            
            List<Person> persons = new List<Person>();
            foreach(var person in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                persons.Add(person.Person);
            }

            PersonCmbx.ItemsSource = persons;

            PersonCmbx.SelectionChanged += PersonCmbx_SelectionChanged;
            ModalCmbx.SelectionChanged += ModalCmbx_SelectionChanged;
            SubmodalCmbx.SelectionChanged += SubmodalCmbx_SelectionChanged;
            
            Labels = new[] { "" };
            Formatter = val =>
            {
                return TimeSpan.FromSeconds(val).ToString(@"mm\:ss");
            };

            DataContext = this;
        }

        private void SubmodalCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void ModalCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModalCmbx.SelectedItem is ModalType selModal && PersonCmbx.SelectedItem is Person selPerson)
            {
                List<SubModalType> submodals = new List<SubModalType>();

                foreach (var sm in selModal.SubModalTypes)
                {
                    var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(selPerson,
                        selModal?.ModalTypeId,
                        sm?.SubModalTypeId);
                    if (!ReferenceEquals(null, intervals) && intervals.Count > 0)
                    {
                        submodals.Add(sm);
                    }
                }
                SubmodalCmbx.ItemsSource = submodals;
            }
            //Refresh();
        }

        private void PersonCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ModalType> modals = new List<ModalType>();
            
            foreach (var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                if(e.AddedItems.Count == 1 && e.AddedItems[0].Equals(pis.Person))
                {
                    foreach(var smPis in pis.SubModalType_PersonInScenes)
                    {
                        var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(pis.Person,
                            smPis.SubModalType.ModalTypeId,
                            smPis.SubModalTypeId);
                        if (!ReferenceEquals(null, intervals)
                            && intervals.Count > 0
                            && !modals.Contains(smPis.SubModalType.ModalType))
                        {
                            modals.Add(smPis.SubModalType.ModalType);
                        }
                    }
                }
            }
            ModalCmbx.SelectedIndex = -1;
            SubmodalCmbx.SelectedIndex = -1;
            ModalCmbx.ItemsSource = modals;

            //SeriesCollection = null;

            MyChart.Series = null;
            MyChart.Update();
            
            //Refresh();
        }

        /// <summary>
        /// Gets or sets the series collection of the graph.
        /// </summary>
        /// <value>
        /// The series collection of the graph.
        /// </value>
        public SeriesCollection SeriesCollection { get; set; }
        /// <summary>
        /// Gets or sets the labels.
        /// </summary>
        /// <value>
        /// The labels.
        /// </value>
        public string[] Labels { get; set; }
        /// <summary>
        /// Gets or sets the formatter that maps from milliseconds to time string.
        /// </summary>
        /// <value>
        /// The formatter that maps from milliseconds to time string.
        /// </value>
        public Func<double, string> Formatter { get; set; }

        private void Refresh()
        {
            if (PersonCmbx.SelectedItem is Person selPerson &&
                ModalCmbx.SelectedItem is ModalType selModal &&
                SubmodalCmbx.SelectedItem is SubModalType selSubModal)
            {
                var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(selPerson,
                selModal.ModalTypeId,
                selSubModal.SubModalTypeId);
                if (!ReferenceEquals(null, intervals) && intervals.Count > 0)
                {
                    FillGraph(intervals, selPerson.MainColor);
                }
            }
        }

        private void FillGraph(List<API.DataAccess.Interval> intervals, Color color)
        {
            SeriesCollection = new SeriesCollection();
            double last = 0;
            foreach (var interval in intervals)
            {
                if (interval.StartTime.TotalSeconds - last > 0)
                {
                    SeriesCollection.Add(new StackedRowSeries
                    {
                        Values = new ChartValues<double> { interval.StartTime.TotalSeconds - last },
                        StackMode = StackMode.Values,
                        Fill = Brushes.White,
                        DataLabels = false,
                        LabelPoint = p => p.X.ToString()
                    });
                    Console.WriteLine($"Añadido white de {interval.StartTime.TotalSeconds - last}");
                }
                if (interval.EndTime.TotalSeconds - interval.StartTime.TotalSeconds > 0)
                {
                    SeriesCollection.Add(new StackedRowSeries
                    {
                        Values = new ChartValues<double> { interval.EndTime.TotalSeconds - interval.StartTime.TotalSeconds },
                        StackMode = StackMode.Values,
                        Fill = new SolidColorBrush(color),
                        DataLabels = true,
                        LabelPoint = p => p.X.ToString("F0")
                    });
                    Console.WriteLine($"Añadido blue de {interval.EndTime.TotalSeconds - interval.StartTime.TotalSeconds}");
                }
                last = interval.EndTime.TotalSeconds;
            }
            if(DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().Duration.TotalSeconds - last > 0)
            {
                SeriesCollection.Add(new StackedRowSeries
                {
                    Values = new ChartValues<double> { DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().Duration.TotalSeconds - last },
                    StackMode = StackMode.Values,
                    Fill = Brushes.White,
                    DataLabels = false,
                    LabelPoint = p => p.X.ToString()
                });
            }

            MyChart.Series = SeriesCollection;
            MyChart.Update();
        }
    }
}
