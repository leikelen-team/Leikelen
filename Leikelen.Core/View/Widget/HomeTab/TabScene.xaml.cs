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

namespace cl.uv.leikelen.View.Widget.HomeTab
{
    /// <summary>
    /// Lógica de interacción para TabScene.xaml
    /// </summary>
    public partial class TabScene : TabItem, ITab
    {
        public TabScene()
        {
            InitializeComponent();
        }

        public void Fill()
        {
            ScrollWithContent.Visibility = Visibility.Visible;
            List<IntervalDataGrid> intervalData = new List<IntervalDataGrid>();
            List<EventDataGrid> eventData = new List<EventDataGrid>();
            var persons = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene;
            foreach(var person in persons)
            {
                foreach(var subModal in person.SubModalType_PersonInScenes)
                {
                    var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);
                    var events = DataAccessFacade.Instance.GetEventAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);
                    //var timelesses = DataAccessFacade.Instance.GetTimelessAccess().GetAll(person.PersonId, subModal.ModalTypeId, subModal.SubModalTypeId);

                    if(!ReferenceEquals(null, intervals) && intervals.Count > 0)
                    {
                        var intervalRow = new IntervalDataGrid()
                        {
                            PersonName = person.Person.Name,
                            ModalName = subModal.ModalTypeId,
                            SubModalName = subModal.SubModalTypeId,
                            TotalDuration = "",
                            AverageDuration = "",
                            StdDuration = ""
                        };
                        TimeSpan totalDuration = new TimeSpan(0);
                        double stdDev = 0;
                        foreach(var interval in intervals)
                        {
                            var duration = interval.EndTime.Subtract(interval.StartTime);
                            totalDuration = totalDuration.Add(duration);
                        }
                        intervalRow.TotalDuration = totalDuration.ToString(@"hh\:mm\:ss");
                        double secsAverage = totalDuration.TotalSeconds / intervals.Count;
                        intervalRow.AverageDuration = $"{secsAverage} {Properties.GUI.seconds}";
                        foreach (var interval in intervals)
                        {
                            var duration = interval.EndTime.Subtract(interval.StartTime);
                            stdDev += Math.Pow(duration.TotalSeconds - secsAverage, 2);
                        }
                        intervalRow.StdDuration = $"{Math.Sqrt(stdDev/ (intervals.Count-1))}";
                        intervalData.Add(intervalRow);
                    }

                    if(!ReferenceEquals(null, events) && events.Count > 0)
                    {
                        var eventRow = new EventDataGrid()
                        {
                            PersonName = person.Person.Name,
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
                        if(avgValue != 0)
                        {
                            eventRow.AverageValue = $"{avgValue}";
                            eventRow.StdValue = $"{Math.Sqrt(stdValue / (events.Count - 1))}";
                        }
                        eventRow.AverageTime = $"{secsAverage} {Properties.GUI.seconds}";
                        eventRow.StdTime = $"{Math.Sqrt(stdTime / (events.Count - 1))}";
                        eventData.Add(eventRow);
                    }
                }
            }

            EventDataGrid.ItemsSource = eventData;
            EventDataGrid.Items.Refresh();

            IntervalDataGrid.ItemsSource = intervalData;
            IntervalDataGrid.Items.Refresh();
        }
        public void Reset()
        {
            EventDataGrid.ItemsSource = null;
            EventDataGrid.Items.Refresh();

            IntervalDataGrid.ItemsSource = null;
            IntervalDataGrid.Items.Refresh();

            ScrollWithContent.Visibility = Visibility.Hidden;
        }
    }
    

    public class SceneTabDataGrid
    {
        public string PersonName { get; set; }
        public string ModalName { get; set; }
        public string SubModalName { get; set; }
    }

    public class IntervalDataGrid : SceneTabDataGrid
    {
        public string TotalDuration { get; set; }
        public string AverageDuration { get; set; }
        public string StdDuration { get; set; }
    }

    public class EventDataGrid : SceneTabDataGrid
    {
        public string AverageValue { get; set; }
        public string StdValue { get; set; }
        public string AverageTime { get; set; }
        public string StdTime { get; set; }
    }
}
