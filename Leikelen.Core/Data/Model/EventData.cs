using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// The data model of the application.
/// </summary>
namespace cl.uv.leikelen.Data.Model
{
    /// <summary>
    /// Entity for data of one event.
    /// </summary>
    [Table("event_data")]
    public class EventData
    {
        /// <summary>
        /// Gets or sets the event data identifier.
        /// </summary>
        /// <value>
        /// The event data identifier (auto incremental).
        /// </value>
        [Key]
        [Column("event_data_id")]
        public int EventDataId { get; set; }

        /// <summary>
        /// Gets or sets the event time.
        /// </summary>
        /// <value>
        /// The time at event occurred.
        /// </value>
        [Column("event_time")]
        public TimeSpan EventTime { get; set; }

        /// <summary>
        /// Gets or sets the group for making the interval groups.
        /// </summary>
        /// <value>
        /// the group for making the interval groups.
        /// </value>
        [Column("to_interval")]
        public int ToInterval { get; set; }

        /// <summary>
        /// Gets or sets the represent type associated entity.
        /// </summary>
        /// <value>
        /// The represent type associated entity.
        /// </value>
        public RepresentType RepresentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventData"/> class.
        /// </summary>
        public EventData()
        {

        }
    }
}
