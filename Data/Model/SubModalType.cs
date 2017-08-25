using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("submodal_type")]
    public class SubModalType
    {
        [Column("submodaltype_name")]
        public string SubModalTypeId { get; set; }

        [Column("modaltype_name")]
        public string ModalTypeId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("file")]
        public string File { get; set; }
        
        [ForeignKey("ModalTypeId")]
        public ModalType ModalType { get; set; }

        public List<SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        public SubModalType()
        {
            SubModalType_PersonInScenes = new List<SubModalType_PersonInScene>();
        }
    }
}
