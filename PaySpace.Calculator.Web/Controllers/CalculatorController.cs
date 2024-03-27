using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using PaySpace.Calculator.Web.Models;
using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorHttpService _calculatorHttpService;

        public CalculatorController(ICalculatorHttpService calculatorHttpService)
        {
            _calculatorHttpService = calculatorHttpService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long? id)
        {
            var vm = await this.GetCalculatorViewModelAsync();

            if (id.HasValue)
            {
                var calculatorHistoryList = await _calculatorHttpService.GetHistoryAsync();
                var history = calculatorHistoryList.Where(h => h.Id == id).FirstOrDefault();

                vm.Income = history.Income;
                vm.PostalCode= history.PostalCode;
                vm.Id = history.Id;
            }

            return this.View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var vm = await this.GetCalculatorHistoryViewModelAsync();
            return this.View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await this.DeleteHistory(id);
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);
                return this.BadRequest(e.Message);
            }
            return this.Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> Index(CalculateRequestViewModel request)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    await _calculatorHttpService.CalculateTaxAsync(new CalculateRequest
                    {
                        PostalCode = request.PostalCode,
                        Income = request.Income,
                        Id = request.Id
                    });

                    return this.RedirectToAction(nameof(this.History));
                }
                catch (Exception e)
                {
                    this.ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            var vm = await this.GetCalculatorViewModelAsync(request);

            return this.View(vm);
        }

        private async Task<CalculatorViewModel> GetCalculatorViewModelAsync(CalculateRequestViewModel? request = null)
        {
            var postalCodes = await _calculatorHttpService.GetPostalCodesAsync();

            return new CalculatorViewModel
            {
                PostalCodes = new SelectList(postalCodes.Select(pc => pc.Code)),
                Income = request?.Income ?? 0,
                PostalCode = request?.PostalCode ?? string.Empty,
                Id = request?.Id
            };
        }

        private async Task<CalculatorHistoryViewModel> GetCalculatorHistoryViewModelAsync(CalculateRequestViewModel? request = null)
        {
            var calculatorHistory = await _calculatorHttpService.GetHistoryAsync();

            return new CalculatorHistoryViewModel
            {
                CalculatorHistory = calculatorHistory ?? new List<CalculatorHistory>()
            };
        }

        private async Task DeleteHistory(long id)
        {
            await _calculatorHttpService.DeleteHistory(id);
        }
    }
}