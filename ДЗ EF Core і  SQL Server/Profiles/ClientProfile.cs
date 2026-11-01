using AutoMapper;
using ClientManagement.API.DTOs;
using ClientManagement.API.Models;

namespace ClientManagement.API.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientReadDto>();
        }
    }
}
