using System;
using System.ComponentModel.DataAnnotations;

namespace CourseWorkApp.Data.Entity
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public Client Client { get; set; }
        public Stock Stock { get; set; }
        public long Count { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
