using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestWebApiOnlineMove.Common
{
    public class AuthOptions
    {
        public string Issuer { get; set; } // Кто сгенериривал токен
        public string Audience { get; set; } // Для кого
        public string Secret { get; set; } 
        public int TokenLifetime {get; set;}
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}
