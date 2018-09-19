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
    /// Entity for the represent type of a data in a smt_pis
    /// </summary>
    [Table("represent_type")]
    public class RepresentType
    {
        /// <summary>
        /// Gets or sets the represent type identifier (auto incremental).
        /// </summary>
        /// <value>
        /// The represent type identifier (auto incremental).
        /// </value>
        [Key]
        [Column("represent_type_id")]
        public int RepresentTypeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the submodal type person in scene associated with this instance.
        /// </summary>
        /// <value>
        /// The identifier for the submodal type person in scene associated with this instance.
        /// </value>
        [Column("smt_pis_id")]
        public int SubModalType_PersonInSceneId { get; set; }

        /// <summary>
        /// Gets or sets the numeric value (optional).
        /// </summary>
        /// <value>
        /// The numeric value (optional).
        /// </value>
        [Column("value")]
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets the subtitle (optional).
        /// </summary>
        /// <value>
        /// The subtitle (optional).
        /// </value>
        [Column("subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the index associated for timeless data (optional).
        /// </summary>
        /// <value>
        /// The index associated for timeless data (optional).
        /// </value>
        [Column("index_data")]
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the event data identifier (optional).
        /// </summary>
        /// <value>
        /// The event data identifier (optional).
        /// </value>
        [Column("event_data_id")]
        public int? EventDataId { get; set; }

        /// <summary>
        /// Gets or sets the interval data identifier (optional).
        /// </summary>
        /// <value>
        /// The interval data identifier (optional).
        /// </value>
        [Column("interval_data_id")]
        public int? IntervalDataId { get; set; }

        /// <summary>
        /// Gets or sets the event data.
        /// </summary>
        /// <value>
        /// The event data.
        /// </value>
        public Data.Model.EventData EventData { get; set; }

        /// <summary>
        /// Gets or sets the interval data.
        /// </summary>
        /// <value>
        /// The interval data.
        /// </value>
        public Data.Model.IntervalData IntervalData { get; set; }

        /// <summary>
        /// Gets or sets the sub modal type person in scene.
        /// </summary>
        /// <value>
        /// The sub modal type person in scene.
        /// </value>
        public Data.Model.SubModalType_PersonInScene SubModalType_PersonInScene { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Model.RepresentType"/> class.
        /// </summary>
        public RepresentType()
        {

        }
    }
}
