using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>(); // create a map between LeaveType data model and IndexVM view model
            CreateMap<LeaveTypeCreateVM, LeaveType>(); // expect LeaveTypeCreateVM from the form and convert it to LeaveType data model to save to the database
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap(); // ReverseMap() allows mapping in both directions: LeaveTypeEditVM to LeaveType and LeaveType to LeaveTypeEditVM
        }
    }
}
