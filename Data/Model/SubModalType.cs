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
        [Column("name")]
        public string SubModalTypeId { get; set; }

        [Column("modal_type_name")]
        public string ModalTypeId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("file")]
        public string File { get; set; }
        
        [ForeignKey("ModalTypeName")]
        public ModalType ModalType { get; set; }

        public List<SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        public SubModalType() { }
    }
}
