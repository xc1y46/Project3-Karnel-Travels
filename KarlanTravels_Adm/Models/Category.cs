namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            TouristSpot = new HashSet<TouristSpot>();
        }

        [Required]
        [StringLength(40)]
        public string CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string CategoryNote { get; set; }

        public bool DeleteFlag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TouristSpot> TouristSpot { get; set; }
    }
}
