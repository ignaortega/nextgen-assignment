using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface ICalculatorFactory
    {
        Task<ICalculator> GetCalculatorAsync(string postalCode);
    }
}
