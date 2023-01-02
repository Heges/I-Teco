using Application;
using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimplePresentation.ModelViews
{
    /// <summary>
    /// 
    /// </summary>
    public class ConferenceModelView : IMapWith<ContactConference>
    {
        public Guid ConferenceId { get; set; }
        public Guid ContactId { get; set; }
        [Required]
        public DateTime DateConference { get; set; }
        [Required]
        public List<ContactConferenceModelView> Contacts { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<ContactConference, ConferenceModelView>()
                .ForMember(vm => vm.ContactId, opt => opt.MapFrom(model => model.ContactId))
                .ForMember(vm => vm.ConferenceId, opt => opt.MapFrom(model => model.ConferenceId))
                .ForMember(vm => vm.DateConference, opt => opt.MapFrom(model => model.Conference.DateConference))
                .ForMember(vm => vm.Contacts, opt => opt.MapFrom(model => model));
        }
    }
}
