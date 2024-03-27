using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

using PaySpace.Calculator.Common.Models;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public sealed class BackEndCalculatorController(
        ILogger<BackEndCalculatorController> logger,
        IHistoryService historyService,
        ICalculatorFactory calculatorFactory,
        IPostalCodeService postalCodeService,
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

                if (request.Id.HasValue)
                {
                    await historyService.UpdateHistoryAsync(new CalculatorHistory
                    {
                        Tax = calculateResult.Tax,
                        Calculator = calculateResult.Calculator,
                        PostalCode = request.PostalCode,
                        Income = request.Income,
                        Id = request.Id.Value
                    });
                }
                else
                {
                    await historyService.AddAsync(new CalculatorHistory
                    {
                        Tax = calculateResult.Tax,
                        Calculator = calculateResult.Calculator,
                        PostalCode = request.PostalCode,
                        Income = request.Income
                    });
                }

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

        [HttpPost("delete-history")]
        public async Task<ActionResult> DeleteHistoryAsync(long id)
        {
            try
            {
                await historyService.DeleteHistoryAsync(id);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest();
            }

            return this.Ok();
        }

        [HttpGet("postalcode")]
        public async Task<ActionResult<List<PostalCode>>> PostalCodes()
        {
            var postalCodes = await postalCodeService.GetPostalCodesAsync();

            return this.Ok(mapper.Map<List<PostalCodeDto>>(postalCodes));
        }
    }
}