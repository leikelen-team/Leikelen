﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cl.uv.leikelen.Data.Model
{
    [Table("represent_type")]
    public class RepresentType
    {
        [Key]
        [Column("represent_type_id")]
        public int RepresentTypeId { get; set; }
        [Column("smt_pis_id")]
        public int SubModalType_PersonInSceneId { get; set; }
        [Column("value")]
        public double? Value { get; set; }
        [Column("subtitle")]
        public string Subtitle { get; set; }
        [Column("index")]
        public int? Index { get; set; }

        public IntervalData IntervalData { get; set; }
        public EventData EventData { get; set; }

        public RepresentType() { }
    }
}
