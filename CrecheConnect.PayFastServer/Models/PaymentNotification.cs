using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CrecheConnect.PayFastServer.Models;

public class PaymentNotification
{
    [FromForm(Name = "payment_status")]
    [Required] public string PaymentStatus { get; set; }

    [FromForm(Name = "pf_payment_id")]
    public string PfPaymentId { get; set; }

    [FromForm(Name = "item_name")]
    public string ItemName { get; set; }

    [FromForm(Name = "amount_gross")]
    public decimal AmountGross { get; set; }

    [FromForm(Name = "email_address")]
    public string EmailAddress { get; set; }

    [FromForm(Name = "merchant_id")]
    public string MerchantId { get; set; }

    [FromForm(Name = "signature")]
    public string Signature { get; set; }
}
