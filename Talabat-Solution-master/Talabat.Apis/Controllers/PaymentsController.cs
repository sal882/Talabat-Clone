using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Apis.DTOS;
using Talabat.Apis.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string webHookSecret = "whsec_bf99507323a03d7c1337a71c73038e6a56ce354455ef1fa2cfc150c736b724b5";

        public PaymentsController(
            IPaymentService paymentService,
            ILogger<PaymentsController> logger
            )
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreatOrUpdatePayment(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400));
            
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], webHookSecret);

                var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                Order order;
                switch(stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentService.UpdatePaymentInteneToBeSuccededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation($"Payment Succeded with {paymentIntent.Id}");
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdatePaymentInteneToBeSuccededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation($"Payment Failed with {paymentIntent.Id}");
                        break;
                }
                return Ok();
            
        }
    }
}
