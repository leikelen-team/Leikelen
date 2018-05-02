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
    /// Entity for a scene (or class).
    /// </summary>
    [Table("scene")]
    public class Scene
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Scene"/> is selected in Scene GUI selector.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool Selected { get; set; } = false;

        /// <summary>
        /// Gets or sets the scene identifier.
        /// </summary>
        /// <value>
        /// The scene identifier (auto incremental).
        /// </value>
        [Column("scene_id")]
        public int SceneId { get; set; }

        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        /// <value>
        /// The name of the scene (optional).
        /// </value>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of participants.
        /// </summary>
        /// <value>
        /// The number of participants (optional).
        /// </value>
        [Column("number_of_participants")]
        public int? NumberOfParticipants { get; set; }

        /// <summary>
        /// Gets or sets the type description of the scene.
        /// </summary>
        /// <value>
        /// The type description (text) of the scene.
        /// </value>
        [Column("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the place in which was recorded the scene.
        /// </summary>
        /// <value>
        /// The place in which was recorded the scene.
        /// </value>
        [Column("place")]
        public string Place { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description (multi line text).
        /// </value>
        [DataType(DataType.MultilineText)]
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the record real date time.
        /// </summary>
        /// <value>
        /// The record real date time.
        /// </value>
        [Column("record_real_datetime")]
        public DateTime? RecordRealDateTime { get; set; }

        /// <summary>
        /// Gets or sets the date time in which the recording was started in the software.
        /// </summary>
        /// <value>
        /// The date time in which the recording was started in the software.
        /// </value>
        [Column("record_start_datetime")]
        public DateTime RecordStartedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the scene.
        /// </summary>
        /// <value>
        /// The duration of the scene. Its set automatically by the recorder controller.
        /// </value>
        [DataType(DataType.Duration)]
        [Column("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the persons in scene.
        /// </summary>
        /// <value>
        /// The persons in scene.
        /// </value>
        public List<PersonInScene> PersonsInScene { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        public Scene()
        {
            PersonsInScene = new List<PersonInScene>();
        }

        /// <summary>
        /// Returns the Name of the scene as a <see cref="System.String" />.
        /// </summary>
        /// <returns>
        /// The Name of the scene as a <see cref="System.String" />.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
