using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpModels.Responses
{
    public record UserLoginResponse(Guid id, string firstName, string lastName, string email, string role, string token);
}
