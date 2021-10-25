namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContentTag")]
    public partial class ContentTag
    {
        public long ID { get; set; }

        public long ContentID { get; set; }

        [Required]
        [StringLength(50)]
        public string TagID { get; set; }

        public virtual Content Content { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
