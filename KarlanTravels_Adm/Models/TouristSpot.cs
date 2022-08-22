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
            TourDetails = new HashSet<TourDetail>();
        }

        [Required]
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
        public string SubCategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string TouristSpotLocation { get; set; }

        [Required]
        [Range(1,5)]
        public double TouristSpotRating { get; set; }

        public Int64 OpenHour { get; set; }

        [NotMapped]
        [Display(Name = "OpenHour")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan OpenHourVald
        {
            get { return TimeSpan.FromTicks(OpenHour); }
            set { OpenHour = value.Ticks; }
        }

        public Int64 ClosingHour { get; set; }

        [NotMapped]
        [Display(Name = "ClosingHour")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ClosingHourVald
        {
            get { return TimeSpan.FromTicks(ClosingHour); }
            set { ClosingHour = value.Ticks; }
        }

        public bool TouristSpotAvailability { get; set; }

        [StringLength(255)]
        public string TouristSpotImage { get; set; }

        [StringLength(255)]
        public string TouristSpotNote { get; set; }

        public bool Deleted { get; set; }

        public virtual City City { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
