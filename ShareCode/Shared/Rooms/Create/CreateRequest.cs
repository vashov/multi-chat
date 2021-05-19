using System;
using System.ComponentModel.DataAnnotations;

namespace ShareCode.Shared.Rooms.Create
{
    public class CreateRequest
    {
        public TimeSpan ChatLiveTime { get; set; }

        public bool OnlyOwnerCanInvite { get; set; }

        [Required(ErrorMessage = "Set your name / pseudonym")]
        [RegularExpression("^[A-Za-z0-9]{3,16}$", ErrorMessage = "Length must be between 3 to 16. \nContains only numbers and latin letters.")]
        public string UserName { get; set; }
    }
}
