using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("interval_data")]
    public class IntervalData
    {
        [Key]
        [Column("interval_data_id")]
        public int IntervalDataId { get; set; }

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        public RepresentType RepresentType { get; set; }

        public IntervalData() { }
    }
}
