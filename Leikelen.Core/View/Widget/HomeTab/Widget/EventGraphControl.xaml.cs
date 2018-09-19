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

/// <summary>
/// Widgets for the hometabs.
/// </summary>
namespace cl.uv.leikelen.View.Widget.HomeTab.Widget
{
    /// <summary>
    /// Interaction logic for EventGraphControl.xaml
    /// </summary>
    public partial class EventGraphControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventGraphControl"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the series collection of the graph.
        /// </summary>
        /// <value>
        /// The series collection of the graph.
        /// </value>
        public SeriesCollection SeriesCollection { get; set; }
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
                var events = DataAccessFacade.Instance.GetEventAccess().GetAll(selPerson,
                selModal.ModalTypeId,
                selSubModal.SubModalTypeId);
                if (!ReferenceEquals(null, events) && events.Count > 0)
                {
                    FillGraph(events, selPerson.MainColor);
                }
            }
        }

        private void FillGraph(List<API.DataAccess.Event> events, Color color)
        {
            var dayConfig = Mappers.Xy<TimeModel>()
                .X(dayModel => (double)dayModel.TimeSpan.TotalSeconds)
                .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig);

            var values = new ChartValues<TimeModel>();

            int decimationFactor = events.Count / GeneralSettings.Instance.MaxNumberPointsInEventGraph.Value;
            int decimationActual = 0;
            var decimationArray = new TimeModel[decimationFactor];
            
            foreach (var event_data in events)
            {
                decimationArray[decimationActual] = new TimeModel()
                {
                    TimeSpan = event_data.EventTime,
                    Value = event_data.Value ?? 1
                };
                decimationActual++;
                if (decimationActual == decimationFactor || decimationActual == events.Count - 1)
                {
                    //Add to values
                    double valueMean = 0;
                    foreach(var tModelDecimation in decimationArray)
                    {
                        if(decimationActual == decimationFactor)
                        {
                            valueMean += tModelDecimation.Value;
                        }
                        else
                        {
                            if (tModelDecimation != null)
                                valueMean += tModelDecimation.Value;
                        }
                    }
                    valueMean = valueMean / decimationArray.Count();
                    values.Add(new TimeModel()
                    {
                        TimeSpan = event_data.EventTime,
                        Value = valueMean
                    });
                    decimationActual = 0;
                    decimationArray = new TimeModel[decimationFactor];
                }
            }

            SeriesCollection.Add(new LineSeries()
            {
                Values = values,
                Fill = Brushes.Transparent
            });

            MyChart.Series = SeriesCollection;
            var myCC = new ColorsCollection();
            myCC.Add(color);
            MyChart.SeriesColors = myCC;
            MyChart.Update();
        }
    }

    /// <summary>
    /// Class for a model of data for graph with time of events and its value.
    /// </summary>
    public class TimeModel
    {
        /// <summary>
        /// Gets or sets the time of the event.
        /// </summary>
        /// <value>
        /// The time of the event.
        /// </value>
        public TimeSpan TimeSpan { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }
    }
}
