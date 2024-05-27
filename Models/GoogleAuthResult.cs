using BuddyBlazor.Enum;

namespace BuddyBlazor.Models
{
    public class GoogleAuthResult
    {
        public RegisterOutcome RegisterOutcome { get; set; }
        public string Message { get; set; }
        public string JWTToken { get; set; }
    }
}
