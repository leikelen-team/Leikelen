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
    /// Entity for the sub modal type
    /// </summary>
    [Table("submodal_type")]
    public class SubModalType
    {
        /// <summary>
        /// Gets or sets the name of the sub modal type (key).
        /// </summary>
        /// <value>
        /// The name of the sub modal type (key).
        /// </value>
        [Column("submodaltype_name")]
        public string SubModalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the modal type associated (foreign key).
        /// </summary>
        /// <value>
        /// The name of the modal type associated (foreign key).
        /// </value>
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
        /// Gets or sets the file path associated inside the modal folder (optional).
        /// </summary>
        /// <value>
        /// The file path associated inside the modal folder (optional).
        /// </value>
        [Column("file")]
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the modal type associated.
        /// </summary>
        /// <value>
        /// The modal type associated.
        /// </value>
        [ForeignKey("ModalTypeId")]
        public Data.Model.ModalType ModalType { get; set; }

        /// <summary>
        /// Gets or sets the sub modal type person in scenes.
        /// </summary>
        /// <value>
        /// The sub modal type person in scenes.
        /// </value>
        public List<Data.Model.SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Model.SubModalType"/> class.
        /// </summary>
        public SubModalType()
        {
            SubModalType_PersonInScenes = new List<SubModalType_PersonInScene>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> with the name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the name of this instance.
        /// </returns>
        public override string ToString()
        {
            return SubModalTypeId;
        }
    }
}
