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
using System.Windows.Shapes;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Util;

namespace cl.uv.leikelen.View.Widget.HomeTab
{
    /// <summary>
    /// Interaction logic for TabScene.xaml
    /// </summary>
    public partial class TabScene : TabItem, ITab
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabScene"/> class.
        /// </summary>
        public TabScene()
        {
            InitializeComponent();
        }

        void API.Helper.ITab.Fill()
        {
            ScrollWithContent.Visibility = Visibility.Visible;

            var datas = GetEventsAndIntervals();
            
            IntervalDataGrid.ItemsSource = datas.Item1;
            IntervalDataGrid.Items.Refresh();

            EventDataGrid.ItemsSource = datas.Item2;
            EventDataGrid.Items.Refresh();
        }
        void API.Helper.ITab.Reset()
        {
            EventDataGrid.ItemsSource = null;
            EventDataGrid.Items.Refresh();

            IntervalDataGrid.ItemsSource = null;
            IntervalDataGrid.Items.Refresh();

            ScrollWithContent.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Gets the data of events and intervals in the scene in use.
        /// </summary>
        /// <param name="makeIntervals">if set to <c>true</c> [make intervals].</param>
        /// <param name="makeEvents">if set to <c>true</c> [make events].</param>
        /// <param name="persons">The persons.</param>
        /// <returns>A tuple with the intervals and events.</returns>
        public static Tuple<List<View.Widget.HomeTab.IntervalDataGrid>, List<View.Widget.HomeTab.EventDataGrid>> GetEventsAndIntervals(bool makeIntervals = true, bool makeEvents = true, List<Data.Model.Person> persons = null)
        {
            List<IntervalDataGrid> intervalData = new List<IntervalDataGrid>();
            List<EventDataGrid> eventData = new List<EventDataGrid>();
            var personsInScene = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene;
            foreach (var pis in personsInScene)
            {
                if (persons != null && !persons.Contains(pis.Person))
                    continue;
                foreach (var subModal in pis.SubModalType_PersonInScenes)
                {
                    List<API.DataAccess.Interval> intervals = null;
                    List<API.DataAccess.Event> events = null;
                    if(makeIntervals)
                        intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                    if(makeEvents)
                        events = DataAccessFacade.Instance.GetEventAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                    //var timelesses = DataAccessFacade.Instance.GetTimelessAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);

                    if (makeIntervals && !ReferenceEquals(null, intervals) && intervals.Count > 0)
                    {
                        var intervalRow = new IntervalDataGrid()
                        {
                            PersonName = pis.Person.Name,
                            ModalName = subModal.ModalTypeId,
                            SubModalName = subModal.SubModalTypeId,
                            TotalDuration = "",
                            AverageDuration = "",
                            StdDuration = ""
                        };
                        TimeSpan totalDuration = new TimeSpan(0);
                        double stdDev = 0;
                        foreach (var interval in intervals)
                        {
                            var duration = interval.EndTime.Subtract(interval.StartTime);
                            totalDuration = totalDuration.Add(duration);
                        }
                        intervalRow.TotalDuration = totalDuration.ToString(@"hh\:mm\:ss");
                        double secsAverage = totalDuration.TotalSeconds / intervals.Count;
                        intervalRow.AverageDuration = $"{StringUtil.DoubleAsString(secsAverage, 3)} {Properties.GUI.seconds}";
                        foreach (var interval in intervals)
                        {
                            var duration = interval.EndTime.Subtract(interval.StartTime);
                            stdDev += Math.Pow(duration.TotalSeconds - secsAverage, 2);
                        }
                        intervalRow.StdDuration = $"{StringUtil.DoubleAsString(Math.Sqrt(stdDev / (intervals.Count - 1)), 3)}";
                        intervalData.Add(intervalRow);
                    }

                    if (makeEvents && !ReferenceEquals(null, events) && events.Count > 0)
                    {
                        var eventRow = new EventDataGrid()
                        {
                            PersonName = pis.Person.Name,
                            ModalName = subModal.ModalTypeId,
                            SubModalName = subModal.SubModalTypeId,
                            AverageValue = "",
                            StdValue = "",
                            AverageTime = "",
                            StdTime = ""
                        };
                        double stdValue = 0;
                        TimeSpan totalTime = new TimeSpan(0);
                        double stdTime = 0;
                        double totalValue = 0;
                        foreach (var event_data in events)
                        {
                            totalTime = totalTime.Add(event_data.EventTime);
                            if (event_data.Value.HasValue)
                                totalValue += event_data.Value.Value;
                        }
                        double avgValue = totalValue / events.Count;

                        double secsAverage = totalTime.TotalSeconds / events.Count;
                        foreach (var event_data in events)
                        {
                            stdTime += Math.Pow(event_data.EventTime.TotalSeconds - secsAverage, 2);
                            if (event_data.Value.HasValue)
                                stdValue += Math.Pow(event_data.Value.Value - avgValue, 2);
                        }
                        if (avgValue != 0)
                        {
                            eventRow.AverageValue = $"{StringUtil.DoubleAsString(avgValue, 3)}";
                            eventRow.StdValue = $"{StringUtil.DoubleAsString(Math.Sqrt(stdValue / (events.Count - 1)), 3)}";
                        }
                        eventRow.AverageTime = $"{StringUtil.DoubleAsString(secsAverage, 3)} {Properties.GUI.seconds}";
                        eventRow.StdTime = $"{StringUtil.DoubleAsString(Math.Sqrt(stdTime / (events.Count - 1)), 3)}";
                        eventData.Add(eventRow);
                    }
                }
            }
            return new Tuple<List<IntervalDataGrid>, List<EventDataGrid>>(intervalData, eventData);
        }
    }

    /// <summary>
    /// Base class for data grid.
    /// </summary>
    public class SceneTabDataGrid
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        /// <value>
        /// The name of the person.
        /// </value>
        public string PersonName { get; set; }
        /// <summary>
        /// Gets or sets the name of the modal type.
        /// </summary>
        /// <value>
        /// The name of the modal type.
        /// </value>
        public string ModalName { get; set; }
        /// <summary>
        /// Gets or sets the name of the sub modal type.
        /// </summary>
        /// <value>
        /// The name of the sub modal type.
        /// </value>
        public string SubModalName { get; set; }
    }

    /// <summary>
    /// Data grid class for interval data
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.View.Widget.HomeTab.SceneTabDataGrid" />
    public class IntervalDataGrid : SceneTabDataGrid
    {
        /// <summary>
        /// Gets or sets the total duration of all intervals.
        /// </summary>
        /// <value>
        /// The total duration of all intervals.
        /// </value>
        public string TotalDuration { get; set; }
        /// <summary>
        /// Gets or sets the average duration of intervals.
        /// </summary>
        /// <value>
        /// The average duration of intervals.
        /// </value>
        public string AverageDuration { get; set; }
        /// <summary>
        /// Gets or sets the standard deviation of the duration of the intervals.
        /// </summary>
        /// <value>
        /// The standard deviation of the duration of the intervals.
        /// </value>
        public string StdDuration { get; set; }
    }

    /// <summary>
    /// Data grid class for event data.
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.View.Widget.HomeTab.SceneTabDataGrid" />
    public class EventDataGrid : SceneTabDataGrid
    {
        /// <summary>
        /// Gets or sets the average value of the events.
        /// </summary>
        /// <value>
        /// The average value of the events.
        /// </value>
        public string AverageValue { get; set; }
        /// <summary>
        /// Gets or sets the standard deviation of the values of the events.
        /// </summary>
        /// <value>
        /// The standard deviation of the values of the events.
        /// </value>
        public string StdValue { get; set; }
        /// <summary>
        /// Gets or sets the average time of the events.
        /// </summary>
        /// <value>
        /// The average time of the events.
        /// </value>
        public string AverageTime { get; set; }
        /// <summary>
        /// Gets or sets the standard deviation of the time of the events.
        /// </summary>
        /// <value>
        /// The standard deviation of the time of the events.
        /// </value>
        public string StdTime { get; set; }
    }
}
