namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("City")]
    public partial class City
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            Customer = new HashSet<Customer>();
            Facility = new HashSet<Facility>();
            TouristSpot = new HashSet<TouristSpot>();
        }

        [StringLength(40)]
        public string CityId { get; set; }

        [Required]
        [StringLength(255)]
        public string CityName { get; set; }

        [Required]
        [StringLength(40)]
        public string CountryId { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(255)]
        public string CityNote { get; set; }

        public virtual Country Country { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facility> Facility { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TouristSpot> TouristSpot { get; set; }
    }
}
