using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public sealed class CalculatorController(
        ILogger<CalculatorController> logger,
        IHistoryService historyService,
        ICalculatorFactory calculatorFactory,
        IMapper mapper)
        : ControllerBase
    {
        [HttpPost("calculate-tax")]
        public async Task<ActionResult<CalculateResult>> Calculate(CalculateRequest request)
        {
            try
            {
                var calculator = await calculatorFactory.GetCalculatorAsync(request.PostalCode);

                var calculateResult = await calculator.CalculateAsync(request.Income);

                await historyService.AddAsync(new CalculatorHistory
                {
                    Tax = calculateResult.Tax,
                    Calculator = calculateResult.Calculator,
                    PostalCode = request.PostalCode,
                    Income = request.Income
                });

                return this.Ok(mapper.Map<CalculateResultDto>(calculateResult));
            }
            // We could have a single catch for all excptions
            // and handle the CalculatorExceptions to include the message there
            // but I rather to have "our" exceptions handling separated
            catch (CalculatorException e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest();
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<CalculatorHistory>>> History()
        {
            var history = await historyService.GetHistoryAsync();

            return this.Ok(mapper.Map<List<CalculatorHistoryDto>>(history));
        }
    }
}