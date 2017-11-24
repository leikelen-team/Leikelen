using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("smt_pis")]
    public class SubModalType_PersonInScene
    {
        [Column("smt_pis_id")]
        public int SubModalType_PersonInSceneId { get; set; }

        [Column("scene_id")]
        public int SceneId { get; set; }

        [Column("person_id")]
        public int PersonId { get; set; }

        [Column("submodaltype_name")]
        public string SubModalTypeId { get; set; }

        [Column("modaltype_name")]
        public string ModalTypeId { get; set; }

        public SubModalType SubModalType { get; set; }
        public PersonInScene PersonInScene { get; set; }
        public List<RepresentType> RepresentTypes { get; set; }

        public SubModalType_PersonInScene()
        {
            RepresentTypes = new List<RepresentType>();
        }
    }
}
