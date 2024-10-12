using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Enumerations
{
    /// <summary>
    /// Represents the different types of user roles in the system.
    /// </summary>
    public enum RoleType
    {


        /// <summary>
        /// Role with the highest level of permissions, responsible for overall system management.
        /// </summary>
        Admin,

        /// <summary>
        /// Role responsible for managing products, orders, and customer interactions.
        /// </summary>
        Manager,

        /// <summary>
        /// Role responsible for moderating customer reviews and content.
        /// </summary>
        Moderator
    }       
}
