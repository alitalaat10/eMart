using Microsoft.EntityFrameworkCore.Storage;

namespace eMart.Services
{
    public class MailSettings
    {
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? ApiKey { get; set; }
    }
}
