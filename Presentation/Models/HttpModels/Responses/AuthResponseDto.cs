using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Responses
{
    public abstract class AuthResponseDto
    {
        public string Token { get; set; }
    }
}
