namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Country")]
    public partial class Country
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            City = new HashSet<City>();
        }

        [StringLength(40)]
        public string CountryId { get; set; }

        [Required]
        [StringLength(255)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(40)]
        public string Continent { get; set; }

        [Required]
        [StringLength(20)]
        public string CountryCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City> City { get; set; }
    }
}
