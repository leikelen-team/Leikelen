using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.src.Data.Model
{
    [Table("submodal_type")]
    public class SubModalType
    {
        [Key]
        [Column("submodal_type_id")]
        public int SubModalTypeId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("path")]
        public string Path { get; set; }

        public string ModalTypeName { get; set; }
        [ForeignKey("ModalTypeName")]
        public ModalType ModalType { get; set; }

        public List<SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        public SubModalType() { }
    }
}
