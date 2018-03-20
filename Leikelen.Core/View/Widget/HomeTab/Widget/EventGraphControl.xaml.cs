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
                    var events = DataAccessFacade.Instance.GetEventAccess().GetAll(selPerson,
                        selModal?.ModalTypeId,
                        sm?.SubModalTypeId);
                    if (!ReferenceEquals(null, events) && events.Count > 0)
                    {
                        submodals.Add(sm);
                    }
                }
                SubmodalCmbx.ItemsSource = submodals;
            }
            Refresh();
        }

        private void PersonCmbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ModalType> modals = new List<ModalType>();

            foreach (var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                if (e.AddedItems.Count == 1 && e.AddedItems[0].Equals(pis.Person))
                {
                    foreach (var smPis in pis.SubModalType_PersonInScenes)
                    {
                        var events = DataAccessFacade.Instance.GetEventAccess().GetAll(pis.Person,
                            smPis.SubModalType.ModalTypeId,
                            smPis.SubModalTypeId);
                        if (!ReferenceEquals(null, events) 
                            && events.Count > 0 
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

            MyChart.Series = null;
            MyChart.Update();

            Refresh();
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void Refresh()
        {
            if (PersonCmbx.SelectedItem is Person selPerson &&
                ModalCmbx.SelectedItem is ModalType selModal &&
                SubmodalCmbx.SelectedItem is SubModalType selSubModal)
            {
                var events = DataAccessFacade.Instance.GetEventAccess().GetAll(selPerson,
                selModal.ModalTypeId,
                selSubModal.SubModalTypeId);
                if (!ReferenceEquals(null, events) && events.Count > 0)
                {
                    FillGraph(events);
                }
            }
        }

        private void FillGraph(List<Event> events)
        {
            var dayConfig = Mappers.Xy<TimeModel>()
                .X(dayModel => (double)dayModel.TimeSpan.TotalSeconds)
                .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig);

            var values = new ChartValues<TimeModel>();
            
            foreach(var event_data in events)
            {
                values.Add(new TimeModel()
                {
                    TimeSpan = event_data.EventTime,
                    Value = event_data.Value ?? 1
                });
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
