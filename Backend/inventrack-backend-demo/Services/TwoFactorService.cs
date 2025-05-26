using inventrack_backend_demo.Data;
using inventrack_backend_demo.DTOs;
using inventrack_backend_demo.Model;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Security.Cryptography;
using System.Text;

namespace inventrack_backend_demo.Services
{
    public class TwoFactorService : ITwoFactorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TwoFactorService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TwoFactorSetupDto> GenerateSetupAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var secretKey = GenerateSecretKey();
            
            // Store the secret key (but don't activate it yet)
            var existingSecret = await _context.TwoFactorSecrets
                .FirstOrDefaultAsync(tfs => tfs.UserId == userId);

            if (existingSecret != null)
            {
                existingSecret.SecretKey = secretKey;
                existingSecret.IsActive = false;
            }
            else
            {
                var twoFactorSecret = new TwoFactorSecret
                {
                    UserId = userId,
                    SecretKey = secretKey,
                    IsActive = false
                };
                _context.TwoFactorSecrets.Add(twoFactorSecret);
            }

            await _context.SaveChangesAsync();

            var qrCodeUrl = GenerateQrCodeUrl(user.Email, secretKey);

            return new TwoFactorSetupDto
            {
                SecretKey = secretKey,
                QrCodeUrl = qrCodeUrl,
                ManualEntryKey = FormatSecretKeyForManualEntry(secretKey)
            };
        }

        public async Task<bool> EnableTwoFactorAsync(int userId, string code)
        {
            var twoFactorSecret = await _context.TwoFactorSecrets
                .FirstOrDefaultAsync(tfs => tfs.UserId == userId);

            if (twoFactorSecret == null)
                return false;

            if (!VerifyTotpCode(twoFactorSecret.SecretKey, code))
                return false;

            // Activate the secret and enable 2FA for the user
            twoFactorSecret.IsActive = true;
            
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.TwoFactorEnabled = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableTwoFactorAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.TwoFactorEnabled = false;

            var twoFactorSecret = await _context.TwoFactorSecrets
                .FirstOrDefaultAsync(tfs => tfs.UserId == userId);

            if (twoFactorSecret != null)
            {
                twoFactorSecret.IsActive = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyCodeAsync(int userId, string code)
        {
            var twoFactorSecret = await _context.TwoFactorSecrets
                .FirstOrDefaultAsync(tfs => tfs.UserId == userId && tfs.IsActive);

            if (twoFactorSecret == null)
                return false;

            return VerifyTotpCode(twoFactorSecret.SecretKey, code);
        }

        public string GenerateQrCodeUrl(string email, string secretKey)
        {
            var appName = _configuration["AppSettings:AppName"] ?? "InvenTrackPro";
            var qrCodeText = $"otpauth://totp/{appName}:{email}?secret={secretKey}&issuer={appName}";
            
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeText, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new Base64QRCode(qrCodeData);
            
            return $"data:image/png;base64,{qrCode.GetGraphic(20)}";
        }

        private string GenerateSecretKey()
        {
            var key = new byte[20];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(key);
            return Convert.ToBase64String(key).Replace("=", "").Replace("+", "").Replace("/", "");
        }

        private string FormatSecretKeyForManualEntry(string secretKey)
        {
            // Format the secret key in groups of 4 characters for easier manual entry
            var formatted = new StringBuilder();
            for (int i = 0; i < secretKey.Length; i += 4)
            {
                if (i > 0) formatted.Append(" ");
                formatted.Append(secretKey.Substring(i, Math.Min(4, secretKey.Length - i)));
            }
            return formatted.ToString();
        }

        private bool VerifyTotpCode(string secretKey, string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 6)
                return false;

            var secretKeyBytes = Convert.FromBase64String(secretKey + new string('=', (4 - secretKey.Length % 4) % 4));
            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var timestep = unixTimestamp / 30;

            // Check current timestep and adjacent ones to account for clock drift
            for (int i = -1; i <= 1; i++)
            {
                var computedCode = GenerateTotpCode(secretKeyBytes, timestep + i);
                if (computedCode == code)
                    return true;
            }

            return false;
        }

        private string GenerateTotpCode(byte[] secretKey, long timestep)
        {
            var timestepBytes = BitConverter.GetBytes(timestep);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestepBytes);

            using var hmac = new HMACSHA1(secretKey);
            var hash = hmac.ComputeHash(timestepBytes);

            var offset = hash[hash.Length - 1] & 0x0F;
            var binaryCode = (hash[offset] & 0x7F) << 24
                           | (hash[offset + 1] & 0xFF) << 16
                           | (hash[offset + 2] & 0xFF) << 8
                           | (hash[offset + 3] & 0xFF);

            var code = binaryCode % 1000000;
            return code.ToString("D6");
        }
    }
}
