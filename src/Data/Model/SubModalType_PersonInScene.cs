using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.src.Data.Model
{
    [Table("submodal_type_person_in_scene")]
    public class SubModalType_PersonInScene
    {
        [Column("submodal_type_id")]
        public int SubModalTypeId { get; set; }
        [Column("scene_id")]
        public int SceneId { get; set; }
        [Column("person_id")]
        public int PersonId { get; set; }

        public SubModalType SubModalType { get; set; }
        public PersonInScene PersonInScenes { get; set; }
        public List<RepresentType> RepresentType { get; set; }

        public SubModalType_PersonInScene() { }
    }
}
