using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("person_in_scene")]
    public class PersonInScene
    {
        [Column("scene_id")]
        public int SceneId { get; set; }
        public Scene Scene { get; set; }

        [Column("person_id")]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public List<SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        public PersonInScene()
        {
            SubModalType_PersonInScenes = new List<SubModalType_PersonInScene>();
        }
    }
}
