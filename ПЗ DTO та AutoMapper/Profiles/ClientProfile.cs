using AutoMapper;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;

namespace ClientManagementAPI.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientReadDto>().ReverseMap();

            CreateMap<Client, ClientCreateDto>().ReverseMap()
                .ForMember(client => client.Id, options => options.Ignore())
                .ForMember(client => client.RegistrationDate, options => options.Ignore());
        }
    }
}
