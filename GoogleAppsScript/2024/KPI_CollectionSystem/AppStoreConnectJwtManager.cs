using Google.Cloud.Functions.Framework;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppStoreConnectJwt
{
    public class AppStoreConnectJwtManager : IHttpFunction
    {
        private readonly ILogger _logger;

        public AppStoreConnectJwtManager(ILogger<AppStoreConnectJwtManager> logger) => _logger = logger;

        public async Task HandleAsync(HttpContext context)
        {
            await context.Response.WriteAsync(GetAppStoreConnectJwt());
        }

        private string GetAppStoreConnectJwt()
        {
            string privateKey = "";

            string issuerId = "";

            string keyId = "";

            ECDsa ecdsa = ECDsa.Create();

            ecdsa.ImportPkcs8PrivateKey(Convert.FromBase64String(EncodeBase64(privateKey)), out _);

            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            Dictionary<string, object> headers = new()
            {
                { "kid", keyId },
                { "typ", "JWT" },
            };

            Dictionary<string, object> payload = new()
            {
                { "aud", "appstoreconnect-v1" },
                { "iat", utcNow.ToUnixTimeSeconds() },
                { "exp", utcNow.AddMinutes(10).ToUnixTimeSeconds() },
                { "iss", issuerId },
            };

            ES256Algorithm algorithm = new(ECDsa.Create(), ecdsa);

            JsonNetSerializer jsonNetSerializer = new();

            JwtBase64UrlEncoder jwtBase64UrlEncoder = new();

            JwtEncoder jwtEncoder = new(algorithm, jsonNetSerializer, jwtBase64UrlEncoder);

            return jwtEncoder.Encode(headers, payload, string.Empty);
        }

        private string EncodeBase64(string data)
        {
            using StringReader reader = new(data);

            string? line;

            List<string> lines = new();

            while ((line = reader.ReadLine()) != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(line);

                line = Encoding.UTF8.GetString(bytes);

                lines.Add(line);
            }

            return string.Join(string.Empty, lines.Skip(1).Take(4));
        }
    }
}