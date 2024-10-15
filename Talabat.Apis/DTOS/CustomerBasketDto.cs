using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.Apis.DTOS
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal DeliveryMethodCost { get; set; }
    }
}
