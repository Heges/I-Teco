using Application;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace SimplePresentation.ModelViews
{
    public class ContactModelView : IMapWith<Contact>
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is Requierd")] //
        [MinLength(3, ErrorMessage = "Too low (type more that 4)")] 
        [MaxLength(20, ErrorMessage = "Too long (type less that 20)")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Not a valid Display Name")]
        public string DisplayName { get; set; } 
        [RegularExpression(@"^+\+\(?([2-9]{1}[-])?([0-9]{3})[-]\)?([0-9]{3}[-])?([0-9]{2}[-])?([0-9]{2})$",
            ErrorMessage = "Not a valid phone number try format +X-XXX-XXX-XX-XX")] 
        public string PhoneNumber { get; set; }
        public List<ContactConferenceModelView> Conferences { get; set; }
        public List<CallDetailModelView> CallingHistory { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Contact,ContactModelView> ()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.DisplayName, opt => opt.MapFrom(model => model.Profile.DisplayName))
                .ForMember(vm => vm.PhoneNumber, opt => opt.MapFrom(model => model.Phone.PhoneNumber))
                .ForMember(vm => vm.CallingHistory, opt => opt.MapFrom(model => model.CallingHistories))
                .ForMember(vm => vm.Conferences, opt => opt.MapFrom(model => model.Conferences));
        }
    }
}
