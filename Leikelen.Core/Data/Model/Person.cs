using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace cl.uv.leikelen.Data.Model
{
    /// <summary>
    /// Entity class of a person
    /// </summary>
    [Table("person")]
    public class Person
    {
        /// <summary>
        /// Person auto incremental identifier
        /// </summary>
        [Column("person_id")]
        public int PersonId { get; set; }

        /// <summary>
        /// Name of the person
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// The path to the photo file inside the person's directory (optional)
        /// </summary>
        [Column("photo")]
        public string Photo { get; set; }

        /// <summary>
        /// date of birth (optional)
        /// </summary>
        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Sex of the person. if 0 then male, if 1 then female, otherwise is unknown
        /// </summary>
        [Column("sex")]
        public int? Sex { get; set; }

        /// <summary>
        /// Tracking id number for person's detected by kinect sensor
        /// </summary>
        [Column("tracking_id")]
        public long? TrackingId { get; set; }

        /// <summary>
        /// Not mapped attribute of sex string.
        /// If sex property is 0, then male string (localized)
        /// If sex property is 1, then female string (localized)
        /// Other number of sex property, is unknown string (localized)
        /// </summary>
        [NotMapped]
        public string SexString
        {
            get
            {
                if (Sex.Equals(0))
                    return Properties.GUI.Male;
                if (Sex.Equals(1))
                    return Properties.GUI.Female;
                else
                    return Properties.GUI.Unknown;
            }
        }

        /// <summary>
        /// List of person in scene attributes to know the scenes in what the person is
        /// </summary>
        public List<Data.Model.PersonInScene> PersonInScenes { get; set; }

        /// <summary>
        /// Gets or sets the main color.
        /// </summary>
        /// <value>
        /// The main color.
        /// </value>
        [NotMapped]
        public Color MainColor { get; set; }

        /// <summary>
        /// Gets or sets the secondary color .
        /// </summary>
        /// <value>
        /// The secondary color.
        /// </value>
        [NotMapped]
        public Color SecondaryColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Model.Person"/> class.
        /// </summary>
        public Person()
        {
            PersonInScenes = new List<PersonInScene>();

            var colors = Util.PersonColor.GetNewColors();
            MainColor = colors.Item1;
            SecondaryColor = colors.Item2;
        }

        /// <summary>
        /// Returns the name of the person as the string identifier of the object
        /// </summary>
        /// <returns>The name of the person</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
