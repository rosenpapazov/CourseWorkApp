namespace CourseWorkApp.Data.DTOs
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public int ClientId { get; set; }
        public int StockId { get; set; }
        public long Count { get; set; }
    }
}
