﻿using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IHistoryService
    {
        Task<List<CalculatorHistory>> GetHistoryAsync();

        Task AddAsync(CalculatorHistory calculatorHistory);
        Task DeleteHistoryAsync(long id);
        Task UpdateHistoryAsync(CalculatorHistory calculatorHistory);
    }
}