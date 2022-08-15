namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tour")]
    public partial class Tour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tour()
        {
            TourDetail = new HashSet<TourDetail>();
            TransactionRecord = new HashSet<TransactionRecord>();
        }

        [StringLength(40)]
        public string TourId { get; set; }

        [Required]
        [StringLength(255)]
        public string TourName { get; set; }

        public bool TourAvailability { get; set; }

        public DateTime TourStart { get; set; }

        public DateTime TourEnd { get; set; }

        [Column(TypeName = "money")]
        public decimal TourPrice { get; set; }

        [StringLength(255)]
        public string TourNote { get; set; }

        public bool DeleteFlag { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourDetail> TourDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionRecord> TransactionRecord { get; set; }
    }
}
