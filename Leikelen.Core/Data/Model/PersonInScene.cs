using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    /// <summary>
    /// Entity for a person in a scene
    /// </summary>
    [Table("person_in_scene")]
    public class PersonInScene
    {
        /// <summary>
        /// Gets or sets the identifier of the associated scene.
        /// </summary>
        /// <value>
        /// The scene identifier.
        /// </value>
        [Column("scene_id")]
        public int SceneId { get; set; }

        /// <summary>
        /// Gets or sets the scene associated.
        /// </summary>
        /// <value>
        /// The scene associated.
        /// </value>
        public Data.Model.Scene Scene { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated person.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        [Column("person_id")]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the person associated.
        /// </summary>
        /// <value>
        /// The person.
        /// </value>
        public Data.Model.Person Person { get; set; }

        /// <summary>
        /// Gets or sets the sub modal type person in scenes.
        /// </summary>
        /// <value>
        /// The sub modal type person in scenes.
        /// </value>
        public List<Data.Model.SubModalType_PersonInScene> SubModalType_PersonInScenes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Model.PersonInScene"/> class.
        /// </summary>
        public PersonInScene()
        {
            SubModalType_PersonInScenes = new List<SubModalType_PersonInScene>();
        }
    }
}
