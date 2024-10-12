using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtTokenGenerator
{
    public class JwtConfig
    {
        public string SigningKey { get; set; } = "";
        public TimeSpan LifeTime { get; set; }
        public string Audience { get; set; } = "";
        public string Issuer { get; set; } = "";

        public byte[] SigningKeyBytes => Encoding.UTF8.GetBytes(SigningKey);
    }
}
