namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        public int AdminId { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminName { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        [Required]
        [StringLength(40)]
        public string RoleId { get; set; }

        [StringLength(255)]
        public string AdminNote { get; set; }

        //public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual AdminRole AdminRole { get; set; }

    }
}
