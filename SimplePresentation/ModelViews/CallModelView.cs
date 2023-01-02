using System;
using System.ComponentModel.DataAnnotations;

namespace SimplePresentation.ModelViews
{
    public class CallModelView
    {
        public Guid CallerId { get; set; }
        [Required]
        [RegularExpression(@"^+\+\(?([2-9]{1}[-])?([0-9]{3})[-]\)?([0-9]{3}[-])?([0-9]{2}[-])?([0-9]{2})$",
            ErrorMessage = "Not a valid phone number try format +X-XXX-XXX-XX-XX")]
        public string PhoneNumber { get; set; }
    }
}
