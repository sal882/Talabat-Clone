using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Apis.DTOS
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public ICollection<OrderItemToReturnDto> Items { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public int DeliveryMethodCost { get; set; }
        public string DeliverMethodTime { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntendId { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
