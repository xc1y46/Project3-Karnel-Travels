namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            TransactionRecords = new HashSet<TransactionRecord>();
        }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int? BankAccountId { get; set; }

        [Required]
        [StringLength(40)]
        public string CityId { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Column(TypeName = "money")]
        public decimal AmountToPay { get; set; }

        [Column(TypeName = "money")]
        public decimal AmountToRefund { get; set; }

        [NotMapped]
        public int maxViolations = 5;

        [Range(0, 5)]
        public int Violations { get; set; }

        [StringLength(255)]
        public string CustomerNote { get; set; }

        public bool Deleted { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public virtual City City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionRecord> TransactionRecords { get; set; }
    }
}