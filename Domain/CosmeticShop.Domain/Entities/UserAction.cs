using CosmeticShop.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShop.Domain.Entities
{
    /// <summary>
    /// Represents an action taken by a user.
    /// </summary>
    public class UserAction : IEntity
    {
        /// <summary>
        /// Gets the unique identifier of the user action.
        /// </summary>
        [Key]
        public Guid Id { get; init; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who performed the action.
        /// </summary>
        [Required]
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets or sets the type of action performed by the user.
        /// </summary>
        [Required]
        public ActionType Type { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the action was performed.
        /// </summary>
        [Required]
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Gets or sets the user who performed the action.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAction"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the user action.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="type">The type of action performed by the user.</param>
        /// <param name="actionDate">The date and time of the action.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> or <paramref name="userId"/> is an empty GUID.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is not provided.</exception>
        public UserAction(Guid userId, ActionType type)
        {
            if (!Enum.IsDefined(typeof(ActionType), type))
                throw new ArgumentOutOfRangeException(nameof(type), "Invalid action type.");

            Id = Guid.NewGuid();
            UserId = userId;
            Type = type;
            ActionDate = DateTime.UtcNow;
        }
    }
}
