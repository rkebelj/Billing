namespace Billing.Models.Invoice
{
    public class InvoiceModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public  static int DDV { get; set; } = 22;
        public int? Discount { get; set; }


    }
}