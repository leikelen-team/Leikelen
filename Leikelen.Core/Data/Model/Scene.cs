using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("scene")]
    public class Scene
    {
        [Column("scene_id")]
        public int SceneId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("number_of_participants")]
        public int? NumberOfParticipants { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("place")]
        public string Place { get; set; }

        [DataType(DataType.MultilineText)]
        [Column("description")]
        public string Description { get; set; }

        [Column("record_real_datetime")]
        public DateTime? RecordRealDateTime { get; set; }

        [Column("record_start_datetime")]
        public DateTime RecordStartedDateTime { get; set; }

        [DataType(DataType.Duration)]
        [Column("duration")]
        public TimeSpan Duration { get; set; }

        public List<PersonInScene> PersonsInScene { get; set; }

        public Scene()
        {
            PersonsInScene = new List<PersonInScene>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
