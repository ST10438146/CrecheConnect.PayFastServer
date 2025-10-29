using Microsoft.AspNetCore.Mvc;
using CrecheConnect.PayFastServer.Services;
using CrecheConnect.PayFastServer.Models;

namespace CrecheConnect.PayFastServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly PayFastService _payFastService;

        public HomeController(PayFastService payFastService)
        {
            _payFastService = payFastService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pay(string name, string email, decimal amount, string description)
        {
            if (string.IsNullOrWhiteSpace(email) || amount <= 0)
            {
                ViewBag.Error = "Please enter valid payment details.";
                return View("Index");
            }

            var (url, payload, signature) = _payFastService.GeneratePaymentData(
                amount,
                "Creche Fee",
                description ?? "Monthly Creche Payment",
                email
            );

            // Build an auto-posting HTML form
            var htmlForm = new System.Text.StringBuilder();
            htmlForm.Append("<html><head><meta charset='UTF-8'><title>Redirecting to PayFast</title>");
            htmlForm.Append("<script>window.onload=function(){document.forms[0].submit();}</script></head><body>");
            htmlForm.Append("<h3>Redirecting you to PayFast...</h3>");
            htmlForm.Append($"<form action='{url}' method='POST'>");

            foreach (var kv in payload)
            {
                htmlForm.Append($"<input type='hidden' name='{kv.Key}' value='{kv.Value}' />");
            }

            htmlForm.Append($"<input type='hidden' name='signature' value='{signature}' />");
            htmlForm.Append("</form></body></html>");

            return Content(htmlForm.ToString(), "text/html");
        }

        [HttpGet("payment/success")]
        public IActionResult PaymentSuccess()
        {
            return View("PaymentResult", "Payment successful! Thank you.");
        }

        [HttpGet("payment/cancel")]
        public IActionResult PaymentCancel()
        {
            return View("PaymentResult", "Payment canceled. Please try again.");
        }
    }
}
