namespace CrecheConnect.PayFastServer.Models;

public class Transaction
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }
    public decimal Amount { get; set; }
    public string PaymentStatus { get; set; } = "PENDING";
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}
