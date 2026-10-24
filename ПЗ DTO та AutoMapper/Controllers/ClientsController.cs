using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ClientManagementAPI.DTOs;
using ClientManagementAPI.Models;

namespace ClientManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private static readonly List<Client> Clients = new()
        {
            new Client
            {
                Id = 1,
                FullName = "Коваленко Олена Петрівна",
                Email = "olena.kovalenko@example.com",
                Phone = "+380501112233",
                RegistrationDate = new DateTime(2026, 8, 12)
            },
            new Client
            {
                Id = 2,
                FullName = "Шевчук Андрій Іванович",
                Email = "andrii.shevchuk@example.com",
                Phone = "+380671234567",
                RegistrationDate = new DateTime(2026, 9, 3)
            },
            new Client
            {
                Id = 3,
                FullName = "Мельник Марія Олексіївна",
                Email = "maria.melnyk@example.com",
                Phone = "+380931112244",
                RegistrationDate = new DateTime(2026, 10, 1)
            }
        };

        private readonly IMapper _mapper;

        public ClientsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClientReadDto>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(Clients));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ClientReadDto> GetById(int id)
        {
            var client = Clients.FirstOrDefault(c => c.Id == id);

            if (client is null)
            {
                return NotFound("Client not found");
            }

            return Ok(_mapper.Map<ClientReadDto>(client));
        }

        [HttpPost]
        public ActionResult<ClientReadDto> Create([FromBody] ClientCreateDto dto)
        {
            var client = _mapper.Map<Client>(dto);

            client.Id = Clients.Count == 0 ? 1 : Clients.Max(c => c.Id) + 1;
            client.RegistrationDate = DateTime.Now;

            Clients.Add(client);

            var result = _mapper.Map<ClientReadDto>(client);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}
