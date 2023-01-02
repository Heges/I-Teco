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
    public class ConferenceEditModelView : IMapWith<Conference>
    {
        public Guid ConferenceId { get; set; }
        [Required]
        public DateTime DateConference { get; set; }
        [Required]
        public List<ContactConferenceModelView> Contacts { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Conference, ConferenceEditModelView>()
                .ForMember(vm => vm.ConferenceId, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.DateConference, opt => opt.MapFrom(model => model.DateConference))
                .ForMember(vm => vm.Contacts, opt => opt.MapFrom(model => model.Contacts));
        }
    }
}
