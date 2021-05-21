using System;
using System.ComponentModel.DataAnnotations;

namespace ShareCode.Shared.Rooms.Create
{
    public class CreateRequest
    {
        private const string _msg = "Length must be between 3 to 16. \nContains only numbers and latin letters.";
        private const string _pattern = "^[A-Za-z0-9]{3,16}$";

        [StringLength(16, MinimumLength = 3)]
        public string Topic { get; set; }

        public string ChatLifespan { get; set; }
        public bool OnlyOwnerCanInvite { get; set; }

        [RegularExpression(_pattern, ErrorMessage = _msg)]
        public string UserName { get; set; }
    }
}
