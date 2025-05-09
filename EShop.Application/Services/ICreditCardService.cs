using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Application.Services
{
    public interface ICreditCardService
    {
        public bool ValidateCardNumber(string cardNumber);

        public string GetCardType(string cardNumber);
    }
}
