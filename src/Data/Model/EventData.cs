using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.src.Data.Model
{
    [Table("event_data")]
    public class EventData
    {
        [Key]
        [Column("event_data_id")]
        public int EventDataId { get; set; }
        [Column("event_time")]
        public TimeSpan EventTime { get; set; }

        public EventData() { }
    }
}
