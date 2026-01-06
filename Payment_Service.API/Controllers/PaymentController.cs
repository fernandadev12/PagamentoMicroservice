using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Command;
using PaymentService.Application.DTO;
using PaymentService.Application.Querie;
using PaymentService.Domain.Enums;

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
        /// Lista os dados de um pagamento.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var list = await _mediator.Send(new GetListPaymentsQuerie());
            return Ok(list);
        }

        /// <summary>
        /// Atualiza os dados de um pagamento.
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePayment([FromBody] PaymentRequestDTO request)
        {
            var list = await _mediator.Send(new UpdatePaymentCommand());
            return Ok(list);
        }
        /// <summary>
        /// Cancela um pagamento.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true para cancelados</returns>
        [HttpPost]
        public async Task<IActionResult> CancelPayment(Guid id)
        {
            var paymentCancel = await _mediator.Send(new CancelPaymentCommand());
            return Ok(paymentCancel);
        }
        
    }
}