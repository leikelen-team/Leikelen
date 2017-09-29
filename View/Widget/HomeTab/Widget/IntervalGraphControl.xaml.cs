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
    /// Lógica de interacción para IntervalGraphControl.xaml
    /// </summary>
    public partial class IntervalGraphControl : UserControl
    {
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

            //FillGraph(intervalsTest);

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
            if (e.AddedItems.Count == 1)
            {
                var modal = e.AddedItems[0] as ModalType;
                if(!ReferenceEquals(null, modal))
                {
                    var selPerson = PersonCmbx.SelectedItem as Person;
                    var selModal = ModalCmbx.SelectedItem as ModalType;
                    if (ReferenceEquals(null, selPerson) || ReferenceEquals(null, selModal))
                        return;
                    List<SubModalType> submodals = new List<SubModalType>();

                    foreach (var sm in modal.SubModalTypes)
                    {
                        var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(selPerson.PersonId,
                            selModal?.ModalTypeId,
                            sm?.SubModalTypeId);
                        if (!ReferenceEquals(null, intervals))
                        {
                            submodals.Add(sm);
                        }
                    }
                    SubmodalCmbx.ItemsSource = submodals;
                }
            }
            Refresh();
        }

        private void PersonCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ModalType> modals = new List<ModalType>();
            
            foreach (var person in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                if(e.AddedItems.Count == 1 && e.AddedItems[0].Equals(person.Person))
                {
                    foreach(var smPis in person.SubModalType_PersonInScenes)
                    {
                        var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(person.PersonId,
                            smPis.SubModalType.ModalTypeId,
                            smPis.SubModalTypeId);
                        if (!ReferenceEquals(null, intervals) && !modals.Contains(smPis.SubModalType.ModalType))
                        {
                            modals.Add(smPis.SubModalType.ModalType);
                        }
                    }
                }
            }
            ModalCmbx.SelectedIndex = -1;
            SubmodalCmbx.SelectedIndex = -1;
            ModalCmbx.ItemsSource = modals;

            MyChart.Series = null;
            MyChart.Update();
            
            Refresh();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void Refresh()
        {
            var selPerson = PersonCmbx.SelectedItem as Person;
            var selModal = ModalCmbx.SelectedItem as ModalType;
            var selSubModal = SubmodalCmbx.SelectedItem as SubModalType;
            if (ReferenceEquals(null, selPerson) || ReferenceEquals(null, selModal) || ReferenceEquals(null, selSubModal))
                return;
            var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(selPerson.PersonId,
                selModal.ModalTypeId,
                selSubModal.SubModalTypeId);
            if (!ReferenceEquals(null, intervals))
            {
                FillGraph(intervals);
            }
        }

        private void FillGraph(List<Interval> intervals)
        {
            SeriesCollection = new SeriesCollection();
            double last = 0;
            foreach (var interval in intervals)
            {
                if (interval.StartTime.Ticks - last > 0)
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
                SeriesCollection.Add(new StackedRowSeries
                {
                    Values = new ChartValues<double> { interval.EndTime.TotalSeconds - interval.StartTime.TotalSeconds },
                    StackMode = StackMode.Values,
                    Fill = Brushes.Blue,
                    DataLabels = true,
                    LabelPoint = p => p.X.ToString("F0")
                });
                Console.WriteLine($"Añadido blue de {interval.EndTime.TotalSeconds - interval.StartTime.TotalSeconds}");
                last = interval.EndTime.TotalSeconds;
            }
            SeriesCollection.Add(new StackedRowSeries
            {
                Values = new ChartValues<double> { DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().Duration.TotalSeconds - last },
                StackMode = StackMode.Values,
                Fill = Brushes.White,
                DataLabels = false,
                LabelPoint = p => p.X.ToString()
            });

            MyChart.Series = SeriesCollection;
            MyChart.Update();
        }
    }
}
