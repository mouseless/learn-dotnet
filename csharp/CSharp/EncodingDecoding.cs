using System.Buffers.Text;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CSharp;

public class EncodingDecoding(ILogger<EncodingDecoding> _logger)
{
    public void RunShowCases()
    {
        string originalBase64Text = "This is a sample text.";

        _logger.LogInformation($"Original text: {originalBase64Text}");

        // Base64 Encoding
        string base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(originalBase64Text));
        _logger.LogInformation($"Base64 encoded form: {base64Encoded}");

        // Base64 Decoding
        string base64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64Encoded));
        _logger.LogInformation($"Base64 decoded form: {base64Decoded}");

        // Base64Url Encoding
        string base64UrlEncoded = Base64Url.EncodeToString(Encoding.UTF8.GetBytes(originalBase64Text));
        _logger.LogInformation($"Base64Url encoded form: {base64UrlEncoded}");

        // Base64Url Decoding
        string base64UrlDecoded = Encoding.UTF8.GetString(Base64Url.DecodeFromChars(base64UrlEncoded));
        _logger.LogInformation($"Base64Url decoded form: {base64UrlDecoded}");
    }
}