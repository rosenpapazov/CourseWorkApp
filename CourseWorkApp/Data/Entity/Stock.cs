using System.ComponentModel.DataAnnotations;

namespace CourseWorkApp.Data.Entity
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
