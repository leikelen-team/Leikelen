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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.View.Widget.HomeTab.Widget
{
    /// <summary>
    /// Lógica de interacción para EventGraphControl.xaml
    /// </summary>
    public partial class EventGraphControl : UserControl
    {
        public EventGraphControl()
        {
            InitializeComponent();

            List<Person> persons = new List<Person>();
            foreach (var person in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                persons.Add(person.Person);
            }

            PersonCmbx.ItemsSource = persons;

            PersonCmbx.SelectionChanged += PersonCmbx_SelectionChanged;
            ModalCmbx.SelectionChanged += ModalCmbx_SelectionChanged;
            SubmodalCmbx.SelectionChanged += SubmodalCmbx_SelectionChanged;

            //FillGraph(intervalsTest);
            
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
                if (!ReferenceEquals(null, modal))
                {
                    var selPerson = PersonCmbx.SelectedItem as Person;
                    var selModal = ModalCmbx.SelectedItem as ModalType;
                    if (ReferenceEquals(null, selPerson) || ReferenceEquals(null, selModal))
                        return;
                    List<SubModalType> submodals = new List<SubModalType>();

                    foreach (var sm in modal.SubModalTypes)
                    {
                        var events = DataAccessFacade.Instance.GetEventAccess().GetAll(selPerson.PersonId,
                            selModal?.ModalTypeId,
                            sm?.SubModalTypeId);
                        if (!ReferenceEquals(null, events))
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
                if (e.AddedItems.Count == 1 && e.AddedItems[0].Equals(person.Person))
                {
                    foreach (var smPis in person.SubModalType_PersonInScenes)
                    {
                        var events = DataAccessFacade.Instance.GetEventAccess().GetAll(person.PersonId,
                            smPis.SubModalType.ModalTypeId,
                            smPis.SubModalTypeId);
                        if (!ReferenceEquals(null, events) && !modals.Contains(smPis.SubModalType.ModalType))
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
        public Func<double, string> Formatter { get; set; }

        private void Refresh()
        {
            var selPerson = PersonCmbx.SelectedItem as Person;
            var selModal = ModalCmbx.SelectedItem as ModalType;
            var selSubModal = SubmodalCmbx.SelectedItem as SubModalType;
            if (ReferenceEquals(null, selPerson) || ReferenceEquals(null, selModal) || ReferenceEquals(null, selSubModal))
                return;
            var events = DataAccessFacade.Instance.GetEventAccess().GetAll(selPerson.PersonId,
                selModal.ModalTypeId,
                selSubModal.SubModalTypeId);
            if (!ReferenceEquals(null, events))
            {
                FillGraph(events);
            }
        }

        private void FillGraph(List<Event> events)
        {
            var dayConfig = Mappers.Xy<TimeModel>()
                .X(dayModel => (double)dayModel.TimeSpan.Ticks)
                .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig);

            var values = new ChartValues<TimeModel>();
            
            foreach(var event_data in events)
            {
                if (event_data.Value.HasValue)
                {
                    values.Add(new TimeModel()
                    {
                        TimeSpan = event_data.EventTime,
                        Value = event_data.Value.Value
                    });
                }
            }
            SeriesCollection.Add(new LineSeries()
            {
                Values = values,
                Fill = Brushes.Transparent
            });

            MyChart.Series = SeriesCollection;
            MyChart.Update();
        }
    }

    public class TimeModel
    {
        public TimeSpan TimeSpan { get; set; }
        public double Value { get; set; }
    }
}
