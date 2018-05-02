using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    /// <summary>
    /// Entity for data of an interval
    /// </summary>
    [Table("interval_data")]
    public class IntervalData
    {
        /// <summary>
        /// Gets or sets the interval data identifier (auto incremental).
        /// </summary>
        /// <value>
        /// The interval data identifier (auto incremental).
        /// </value>
        [Key]
        [Column("interval_data_id")]
        public int IntervalDataId { get; set; }

        /// <summary>
        /// Gets or sets the start time of the interval.
        /// </summary>
        /// <value>
        /// The start time of the interval.
        /// </value>
        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the interval.
        /// </summary>
        /// <value>
        /// The end time of the interval.
        /// </value>
        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the represent type associated entity.
        /// </summary>
        /// <value>
        /// The represent type associated entity.
        /// </value>
        public RepresentType RepresentType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalData"/> class.
        /// </summary>
        public IntervalData()
        {

        }
    }
}
