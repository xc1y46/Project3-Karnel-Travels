namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Facility")]
    public partial class Facility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Facility()
        {
            TourDetail = new HashSet<TourDetail>();
        }

        [StringLength(40)]
        public string FacilityId { get; set; }

        [Required]
        [StringLength(255)]
        public string FacilityName { get; set; }

        [Required]
        [StringLength(40)]
        public string FacilityTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string FacilityLocation { get; set; }

        [Required]
        [StringLength(40)]
        public string CityId { get; set; }

        [Column(TypeName = "money")]
        public decimal FacilityPrice { get; set; }

        public double FacilityQuality { get; set; }

        public int Quantity { get; set; }

        [StringLength(255)]
        public string FacilityNote { get; set; }

        public bool FacilityAvailability { get; set; }

        public bool DeleteFlag { get; set; }

        public virtual City City { get; set; }

        public virtual FacilityType FacilityType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetail { get; set; }
    }
}
