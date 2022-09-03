namespace KarlanTravels_Adm.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionRecord")]
    public partial class TransactionRecord
    {
        public int TransactionRecordId { get; set; }

        [Required]
        [StringLength(40)]
        public string TourId { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [Range(1,1000)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TransactionFee { get; set; }

        public bool Canceled { get; set; }

        public bool Paid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RecordedTime { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [StringLength(255)]
        public string TransactionNote { get; set; }

        public bool Deleted { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
