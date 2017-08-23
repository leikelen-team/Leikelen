using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("modal_type")]
    public class ModalType
    {
        [Key]
        [Column("name")]
        public string ModalTypeId { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public List<SubModalType> SubModalTypes { get; set; }

        public ModalType() { }
    }
}
