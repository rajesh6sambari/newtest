using System.Linq;
using System.Security.Claims;

namespace TestingTutor.UI.Security
{
    public static class AuthorizeHtmlHelper
    {
        public static bool UserIsInstructorOrHigher(ClaimsIdentity user)
        {
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Instructor"))
                || user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin"))
                || user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
        }

        public static bool UserIsAdminOrHigher(ClaimsIdentity user)
        {
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("Admin"))
                   || user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
        }

        public static bool UserIsSuperAdmin(ClaimsIdentity user)
        {
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value.Equals("SuperAdmin"));
        }
    }
}
