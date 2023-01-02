using Application;
using Domain;
using System;

namespace SimplePresentation.ModelViews
{
    public class CallDetailModelView : IMapWith<CallingHistory>
    {
        public Guid CallerId { get; set; }
        public string CallerDisplayName { get; set; }
        public string CallerPhoneNumber { get; set; }
        public string CalledDisplayName { get; set; }
        public string CalledPhoneNumber { get; set; }
        public DateTime DateCall { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<CallingHistory, CallDetailModelView>()
                .ForMember(vm => vm.CallerId, opt => opt.MapFrom(note => note.Id))
                .ForMember(vm => vm.CallerDisplayName, opt => opt.MapFrom(note => note.CallerName))
                .ForMember(vm => vm.CalledDisplayName, opt => opt.MapFrom(note => note.CalledName))
                .ForMember(vm => vm.CallerPhoneNumber, opt => opt.MapFrom(note => note.CallerPhone))
                .ForMember(vm => vm.CalledPhoneNumber, opt => opt.MapFrom(note => note.CalledPhone))
                .ForMember(vm => vm.DateCall, opt => opt.MapFrom(note => note.DateCall));
        }
    }
}
