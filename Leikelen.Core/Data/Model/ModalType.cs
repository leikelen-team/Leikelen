using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    /// <summary>
    /// Entity for the modal type
    /// </summary>
    [Table("modal_type")]
    public class ModalType
    {
        /// <summary>
        /// Gets or sets the name of the modal type (key).
        /// </summary>
        /// <value>
        /// The name of the modal type (key).
        /// </value>
        [Key]
        [Column("modaltype_name")]
        public string ModalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the description (optional).
        /// </summary>
        /// <value>
        /// The description (optional).
        /// </value>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the sub modal types.
        /// </summary>
        /// <value>
        /// The sub modal types.
        /// </value>
        public List<Data.Model.SubModalType> SubModalTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Model.ModalType"/> class.
        /// </summary>
        public ModalType()
        {
            SubModalTypes = new List<Data.Model.SubModalType>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> with the name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the name of this instance.
        /// </returns>
        public override string ToString()
        {
            return ModalTypeId;
        }
    }
}
