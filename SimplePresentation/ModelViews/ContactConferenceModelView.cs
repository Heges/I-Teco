using Application;
using Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimplePresentation.ModelViews
{
    public class ContactConferenceModelView : IMapWith<ContactConference>
    {
        public Guid ContactId { get; set; }
        [Required(ErrorMessage = "Name is Requierd")] //
        [MinLength(3, ErrorMessage = "Too low (type more that 4)")]
        [MaxLength(20, ErrorMessage = "Too long (type less that 20)")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Not a valid Display Name")]
        public string DisplayName { get; set; }
        [RegularExpression(@"^+\+\(?([2-9]{1}[-])?([0-9]{3})[-]\)?([0-9]{3}[-])?([0-9]{2}[-])?([0-9]{2})$",
            ErrorMessage = "Not a valid phone number try format +X-XXX-XXX-XX-XX")]
        public string PhoneNumber { get; set; }
        public Guid ConferenceId { get; set; }
        public DateTime DateConference { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<ContactConference, ContactConferenceModelView>()
                .ForMember(vm => vm.ContactId, opt => opt.MapFrom(model => model.ContactId))
                .ForMember(vm => vm.DisplayName, opt => opt.MapFrom(model => model.Contact.Profile.DisplayName))
                .ForMember(vm => vm.PhoneNumber, opt => opt.MapFrom(model => model.Contact.Phone.PhoneNumber))
                .ForMember(vm => vm.DateConference, opt => opt.MapFrom(model => model.Conference.DateConference));
        }
    }
}
