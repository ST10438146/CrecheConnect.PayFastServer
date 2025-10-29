using Microsoft.AspNetCore.Mvc;
using CrecheConnect.PayFastServer.Services;
using CrecheConnect.PayFastServer.Models;

namespace CrecheConnect.PayFastServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PayFastService _payFastService;

    public PaymentController(PayFastService payFastService)
    {
        _payFastService = payFastService;
    }

    [HttpPost("create-payfast-payment")]
    public IActionResult CreatePayFastPayment([FromBody] Transaction request)
    {
        var (url, payload, signature) = _payFastService.GeneratePaymentData(
            request.Amount, request.ItemName, request.ItemDescription, request.Email);

        var response = new
        {
            payfastUrl = url,
            payload,
            signature
        };
        return Ok(response);
    }

    [HttpPost("notify")]
    public IActionResult Notify([FromForm] PaymentNotification notification)
    {
        Console.WriteLine($"ITN: {notification.PaymentStatus}, {notification.AmountGross}");
        return Ok();
    }

    [HttpGet("success")]
    public IActionResult Success() => Ok("Payment successful. Thank you!");

    [HttpGet("cancel")]
    public IActionResult Cancel() => Ok("Payment cancelled.");
}
