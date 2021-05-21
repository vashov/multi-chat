using System;
using System.ComponentModel.DataAnnotations;

namespace ShareCode.Shared.Rooms.Enter
{
    public class EnterRequest
    {
        [Required]
        public Guid Invite { get; set; }

        [RegularExpression("^[A-Za-z0-9]{3,16}$", ErrorMessage = "Length must be between 3 to 16. Have to contains only numbers and latin letters.")]
        public string UserName { get; set; }
    }
}
