using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Linq;

namespace YourNote.Client.Utils
{
    public class ClaimsParser
    {
        public static string Select(AuthenticationState authState, string type)
        {
            var claims = authState.User.Claims.ToList();

            return claims?.FirstOrDefault(x => x.Type.Equals(type, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}