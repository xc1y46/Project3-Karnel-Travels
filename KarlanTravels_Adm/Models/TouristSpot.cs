namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TouristSpot")]
    public partial class TouristSpot
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TouristSpot()
        {
            TourDetail = new HashSet<TourDetail>();
        }

        [StringLength(40)]
        public string TouristSpotId { get; set; }

        [Required]
        [StringLength(255)]
        public string TouristSpotName { get; set; }

        [Required]
        [StringLength(40)]
        public string CityId { get; set; }

        [Required]
        [StringLength(40)]
        public string CategoryId { get; set; }

        public double Rating { get; set; }

        public bool TouristSpotAvailability { get; set; }

        [StringLength(255)]
        public string TouristSpotNote { get; set; }

        public bool DeleteFlag { get; set; }

        public virtual Category Category { get; set; }

        public virtual City City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetail { get; set; }
    }
}
