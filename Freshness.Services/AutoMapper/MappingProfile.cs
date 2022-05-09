using AutoMapper;
using Freshness.Domain.Entities;
using Freshness.Models.RequestModels;
using Freshness.Models.ResponseModels;

namespace Freshness.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccessoryCreateRequestModel, Accessory>();
            CreateMap<AccessoryUpdateRequestModel, Accessory>();
            CreateMap<Accessory, AccessoryResponseModel>();

            CreateMap<Address, AddressResponseModel>();
            CreateMap<AddressCreateRequestModel, Address>();
            CreateMap<AddressUpdateRequestModel, Address>();
            CreateMap<AddressUpdateRequestModel, AddressCreateRequestModel>();

            CreateMap<Worker, WorkerResponseModel>();
            CreateMap<WorkerCreateRequestModel, Worker>();
            CreateMap<WorkerUpdateRequestModel, Worker>();

            CreateMap<Call, CallResponseModel>()
                .ForMember(x => x.Worker, opt => opt.MapFrom(y => y.Worker));

            CreateMap<CustomerUpdateRequestModel, CustomerCreateRequestModel>();
            CreateMap<Customer, CustomerResponseModel>()
                .ForMember(x => x.Notes, opt => opt.MapFrom(y => y.Notes))
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y.Address));

            CreateMap<ServiceCreateRequestModel, Service>();
            CreateMap<ServiceUpdateRequestModel, Service>();
            CreateMap<Service, ServiceResponseModel>();

            CreateMap<Note, NoteResponseModel>();

            CreateMap<Order, OrderResponseModel>()
                .ForMember(x => x.Worker, opt => opt.MapFrom(y => y.Worker))
                .ForMember(x => x.Customer, opt => opt.MapFrom(y => y.Customer));

            CreateMap<Worker, WorkerResponseModel>();
            CreateMap<WorkerCreateRequestModel, Worker>();
            CreateMap<WorkerUpdateRequestModel, Worker>();
        }
    }
}
