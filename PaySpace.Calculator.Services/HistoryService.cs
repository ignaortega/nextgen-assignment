using Microsoft.EntityFrameworkCore;

using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Services
{
    internal sealed class HistoryService(CalculatorContext context) : IHistoryService
    {
        public async Task AddAsync(CalculatorHistory history)
        {
            history.Timestamp = DateTime.Now;

            await context.AddAsync(history);
            await context.SaveChangesAsync();
        }

        public async Task DeleteHistoryAsync(long id)
        {
            var historyToDelete = await context.Set<CalculatorHistory>().FindAsync(id);
            
            if (historyToDelete == null)
            {
                throw new CalculatorResourceNotFoundException($"There is no record for calculation history ({id})");
            }

            context.Set<CalculatorHistory>().Remove(historyToDelete);
            await context.SaveChangesAsync();
        }


        public Task<List<CalculatorHistory>> GetHistoryAsync()
        {
            return context.Set<CalculatorHistory>()
                .OrderByDescending(_ => _.Timestamp)
                .ToListAsync();
        }

        public async Task UpdateHistoryAsync(CalculatorHistory calculatorHistory)
        {
            var historyToUpdate = await context.Set<CalculatorHistory>().FindAsync(calculatorHistory.Id);

            if(historyToUpdate == null)
            {
                throw new CalculatorResourceNotFoundException($"There is no record for calculation history ({calculatorHistory.Id})");
            }

            historyToUpdate.Income = calculatorHistory.Income;
            historyToUpdate.Calculator = calculatorHistory.Calculator;
            historyToUpdate.PostalCode= calculatorHistory.PostalCode;  
            historyToUpdate.Tax= calculatorHistory.Tax;
            await context.SaveChangesAsync();
        }
    }
}