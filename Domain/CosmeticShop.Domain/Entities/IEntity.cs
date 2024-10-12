using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Interface for all entities with a unique identifier.
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; init; }
    }
}
