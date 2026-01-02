using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Commands;
using PaymentService.Application.DTO;
using PaymentService.Application.Payments;
using PaymentService.Domain.ValueObjects;

namespace PaymentService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cria um novo pagamento.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentRequestDTO request)
        {
            try
            {
                // Converte o int recebido para enum PaymentType
                if (!Enum.IsDefined(typeof(PaymentType), request.Type))
                    return BadRequest(new { message = "Tipo de pagamento inválido." });

                var result = await _mediator.Send(new CreatePaymentCommand(request));
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Busca um pagamento pelo ID.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery(id));
            if (result is null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Lista todos os pagamentos.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _mediator.Send(new GetPaymentsQuery());
            return Ok(list);
        }
    }
}