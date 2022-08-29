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
        public string TransactionTypeId { get; set; }

        [Required]
        [StringLength(40)]
        public string TourId { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal TransactionFee { get; set; }

        public bool Canceled { get; set; }

        public bool Paid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RecordedTime { get; set; }

        public DateTime DueDate { get; set; }

        [NotMapped]
        [DataType(DataType.DateTime)]
        [Display(Name ="DueDate")]
        [DisplayFormat(DataFormatString = "{0:HH:mm MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime _dueDate
        {
            get
            {
                return DueDate;
            }
            set
            {
                DueDate = value.AddDays(7);
            }
        }

        [StringLength(255)]
        public string TransactionNote { get; set; }

        public int? AdminId { get; set; }

        public bool Deleted { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Tour Tour { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
