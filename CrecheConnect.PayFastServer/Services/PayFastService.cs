using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CrecheConnect.PayFastServer.Services;

public class PayFastService
{
    private readonly string _merchantId;
    private readonly string _merchantKey;
    private readonly string _passphrase;
    private readonly string _sandboxUrl;

    public PayFastService(IConfiguration config)
    {
        _merchantId = config["PayFast_MerchantId"];
        _merchantKey = config["PayFast_MerchantKey"];
        _passphrase = config["PayFast_Passphrase"];
        _sandboxUrl = config["PayFast_SandboxUrl"];
    }

    public (string Url, Dictionary<string, string> Payload, string Signature) GeneratePaymentData(
        decimal amount, string itemName, string itemDescription, string emailAddress)
    {
        var data = new Dictionary<string, string>
        {
            { "merchant_id", _merchantId },
            { "merchant_key", _merchantKey },
            { "return_url", "https://crecheconnect-server.onrender.com/api/payment/success" },
            { "cancel_url", "https://crecheconnect-server.onrender.com/api/payment/cancel" },
            { "notify_url", "https://crecheconnect-server.onrender.com/api/payment/notify" },
            { "email_address", emailAddress },
            { "amount", amount.ToString("F2") },
            { "item_name", itemName },
            { "item_description", itemDescription }
        };

        var signature = CreateSignature(data);
        return (_sandboxUrl, data, signature);
    }

    private string CreateSignature(Dictionary<string, string> data)
    {
        var ordered = data.OrderBy(kv => kv.Key);
        var query = string.Join("&", ordered.Select(kv => $"{kv.Key}={UrlEncode(kv.Value)}"));

        if (!string.IsNullOrEmpty(_passphrase))
            query += $"&passphrase={UrlEncode(_passphrase)}";

        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(query));
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    protected string UrlEncode(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty; // ✅ Prevent null crash

        Dictionary<string, string> convertPairs = new()
    {
        { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "+" },
        { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" },
        { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },
        { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" },
        { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" }
    };

        var replaceRegex = new System.Text.RegularExpressions.Regex(@"[%!# $&'()*+,/:;=?@\[\]]");
        return replaceRegex.Replace(url, match => convertPairs[match.Value]);
    }

}
