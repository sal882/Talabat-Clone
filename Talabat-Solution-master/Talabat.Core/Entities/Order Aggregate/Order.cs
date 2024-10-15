using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(string buyerEmail, ICollection<OrderItem> items, Address shippingAddress, DeliveryMethod delieveryMethod, decimal subTotal, string paymentIntendId)
        {
            BuyerEmail = buyerEmail;
            Items = items;
            ShippingAddress = shippingAddress;
            DeliveryMethod = delieveryMethod;
            SubTotal = subTotal;
            PaymentIntendId = paymentIntendId;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ICollection<OrderItem> Items { get; set; }
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public decimal SubTotal { get; set; }
        public string PaymentIntendId { get; set; } 

        public decimal Total() 
            => DeliveryMethod.Cost + SubTotal;
    }
}
