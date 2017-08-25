using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("person")]
    public class Person
    {
        [Column("person_id")]
        public int PersonId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("photo")]
        public string Photo { get; set; }
        [Column("birthday")]
        public DateTime? Birthday { get; set; }
        [Column("sex")]
        public char? Sex { get; set; }

        public List<PersonInScene> PersonInScenes { get; set; }

        public Person()
        {
            PersonInScenes = new List<PersonInScene>();
        }
    }
}
