using Microsoft.AspNetCore.Mvc;
using EShop.Application.Services;
using EShop.Domain.CreditCardExceptions;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {

        protected ICreditCardService _creditCardService;

        public CreditCardController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        // GET: api/<CreditCardController>
        [HttpGet]
        public IActionResult Get(string cardNumber)
        {
            try
            {
                _creditCardService.ValidateCardNumber(cardNumber);
                return Ok(new { cardProvider = _creditCardService.GetCardType(cardNumber) });
            }
            catch (CardNumberTooLongException ex)
            {
                return StatusCode((int)HttpStatusCode.RequestUriTooLong, new { error = "The card number is too long", code = (int)HttpStatusCode.RequestUriTooLong });
            }
            catch (CardNumberTooShortException)
            {
                return BadRequest(new { error = "The card number is too short", code = (int)HttpStatusCode.BadRequest });
            }
            catch (CardNumberInvalidException)
            {
                return BadRequest(new { error = "Invalid Card Number", code = (int)HttpStatusCode.NotAcceptable }); //406
            }
        }
    }
}
