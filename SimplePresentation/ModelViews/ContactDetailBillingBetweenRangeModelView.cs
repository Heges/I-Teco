using Application;
using Domain;
using System;
using System.Collections.Generic;

namespace SimplePresentation.ModelViews
{
    public class ContactDetailBillingBetweenRangeModelView : IMapWith<Contact>
    {
        public Guid ContactId { get; set; }
        public DateTime StartRangeDate { get; set; }
        public DateTime EndRangeDate { get; set; }
        public List<CallDetailModelView> CallingHistory { get; set; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Contact, ContactDetailBillingBetweenRangeModelView>()
                .ForMember(vm => vm.ContactId, opt => opt.MapFrom(model => model.Id))
                .ForMember(vm => vm.CallingHistory, opt => opt.MapFrom(model => model.CallingHistories));
        }
    }
}
