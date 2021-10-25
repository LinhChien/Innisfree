namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Credential")]
    public partial class Credential
    {
        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        public string UserGroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleID { get; set; }

        public virtual Role Role { get; set; }

        public virtual UserGroup UserGroup { get; set; }
    }
}
