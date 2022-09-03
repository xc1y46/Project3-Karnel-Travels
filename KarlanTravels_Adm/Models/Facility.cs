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
            TourDetails = new HashSet<TourDetail>();
        }

        [Required]
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

        [StringLength(255)]
        [DataType(DataType.Url)]
        public string FacilitySocials { get; set; }

        [StringLength(255)]
        [DataType(DataType.PhoneNumber)]
        public string FacilityPhone { get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string FacilityEmail { get; set; }

        [Required]
        [StringLength(40)]
        public string CityId { get; set; }

        [Required]
        [Display(Name = "Service's quantity")]
        [Range(0,200)]
        public int Quantity { get; set; }

        [StringLength(255)]
        public string FacilityImage { get; set; }

        [Required]
        [StringLength(255)]
        public string ServiceNote { get; set; }

        public bool FacilityAvailability { get; set; }

        public bool Deleted { get; set; }

        public virtual City City { get; set; }

        public virtual FacilityType FacilityType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
