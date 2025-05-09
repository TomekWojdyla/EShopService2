using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.CreditCardExceptions
{
    public class CardNumberInvalidException : Exception
    {
        public CardNumberInvalidException() : base("Card Number is invalid") { }

        public CardNumberInvalidException(Exception innerException) : base("Card Number is invalid", innerException) { }
    }
}
