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
    /// Entity for the union of one person in a scene, with a submodal type
    /// </summary>
    [Table("smt_pis")]
    public class SubModalType_PersonInScene
    {
        /// <summary>
        /// Gets or sets the sub modal type person in scene identifier (auto incremental).
        /// </summary>
        /// <value>
        /// The sub modal type person in scene identifier (auto incremental).
        /// </value>
        [Column("smt_pis_id")]
        public int SubModalType_PersonInSceneId { get; set; }

        /// <summary>
        /// Gets or sets the scene identifier.
        /// </summary>
        /// <value>
        /// The scene identifier.
        /// </value>
        [Column("scene_id")]
        public int SceneId { get; set; }

        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        [Column("person_id")]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the sub modal type identifier.
        /// </summary>
        /// <value>
        /// The sub modal type identifier.
        /// </value>
        [Column("submodaltype_name")]
        public string SubModalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the modal type identifier.
        /// </summary>
        /// <value>
        /// The modal type identifier.
        /// </value>
        [Column("modaltype_name")]
        public string ModalTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sub modal type associated.
        /// </summary>
        /// <value>
        /// The sub modal type associated.
        /// </value>
        public SubModalType SubModalType { get; set; }

        /// <summary>
        /// Gets or sets the person in scene.
        /// </summary>
        /// <value>
        /// The person in scene.
        /// </value>
        public PersonInScene PersonInScene { get; set; }

        /// <summary>
        /// Gets or sets the represent types.
        /// </summary>
        /// <value>
        /// The represent types.
        /// </value>
        public List<RepresentType> RepresentTypes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubModalType_PersonInScene"/> class.
        /// </summary>
        public SubModalType_PersonInScene()
        {
            RepresentTypes = new List<RepresentType>();
        }
    }
}
