using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LessonControllerServer.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "LessonControllerServer"; // издатель токена
        public const string AUDIENCE = "LessonControllerClient"; // потребитель токена
        const string KEY = "DSAD123128dkl>DKlasu09fd yH{k'2;#$:~!|@#09F(SAFkl";   // ключ для шифрации
        public const int LIFETIME = 99999; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
