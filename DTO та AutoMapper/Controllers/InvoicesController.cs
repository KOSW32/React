using AutoMapper;
using InvoiceManagement.API.DTOs;
using InvoiceManagement.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private static readonly List<Invoice> Invoices = new()
        {
            new Invoice { Id = 1, Number = "INV-001", ClientName = "ТОВ «Ромашка»", Total = 12500.00m, CreatedAt = new DateTime(2026, 9, 5, 10, 0, 0) },
            new Invoice { Id = 2, Number = "INV-002", ClientName = "ФОП Коваленко О. П.", Total = 3400.50m, CreatedAt = new DateTime(2026, 9, 18, 14, 30, 0) },
            new Invoice { Id = 3, Number = "INV-003", ClientName = "ПП «Вектор»", Total = 87250.75m, CreatedAt = new DateTime(2026, 10, 2, 9, 15, 0) }
        };

        private readonly IMapper _mapper;

        public InvoicesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InvoiceReadDto>> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<InvoiceReadDto>>(Invoices));
        }

        [HttpGet("{id:int}")]
        public ActionResult<InvoiceReadDto> GetById(int id)
        {
            var invoice = Invoices.FirstOrDefault(i => i.Id == id);

            if (invoice is null)
            {
                return NotFound("Invoice not found");
            }

            return Ok(_mapper.Map<InvoiceReadDto>(invoice));
        }

        [HttpPost]
        public ActionResult<InvoiceReadDto> Create([FromBody] InvoiceCreateDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            invoice.Id = Invoices.Count == 0 ? 1 : Invoices.Max(i => i.Id) + 1;
            invoice.Number = $"INV-{invoice.Id:D3}";
            invoice.CreatedAt = DateTime.Now;

            Invoices.Add(invoice);

            var result = _mapper.Map<InvoiceReadDto>(invoice);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<InvoiceReadDto> Update(int id, [FromBody] InvoiceUpdateDto dto)
        {
            var invoice = Invoices.FirstOrDefault(i => i.Id == id);

            if (invoice is null)
            {
                return NotFound("Invoice not found");
            }

            _mapper.Map(dto, invoice);

            return Ok(_mapper.Map<InvoiceReadDto>(invoice));
        }
    }
}
