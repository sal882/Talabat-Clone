using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Orders;

namespace Talabat.Service
{
    public class OrderService : IOrderSeervice
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            //IGenericRepository<Order> orderRepo
            
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, Address shippingAddress, int deliveryMethodId)
        {
            //1. Get Basket by Id
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //2.Create OrderItems
            List<OrderItem> orderItemsList = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                Product product = await _unitOfWork.Repository<Product>()
                    .GetByIdAsync(item.Id);

                var productItemOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl); ;
                var orederItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                orderItemsList.Add(orederItem);
            }
            //3. Calculate Sub Total
            var subTotal = orderItemsList.Sum(oi => oi.Quantity * oi.Price);
            //4. Get Delivery Method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var spec = new OrderWithPaymentSpecifications(basket.PaymentIntentId); 
            var existingOrder = await _unitOfWork.Repository<Order>()
                                                    .GetEntityWithSpecAsync(spec);

            if(existingOrder is not null)
            {
                 _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            //5. Create Order
            var order = new Order(buyerEmail, orderItemsList, shippingAddress, deliveryMethod, subTotal, basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().Add(order);

            //6. save cahnges in database

            var result = await _unitOfWork.Complete();
            if(result > 0)
                return order;
            return null;


        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public Task<Order>? GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = _unitOfWork.Repository<Order>()?.GetEntityWithSpecAsync(spec);

            if (order is null) return null;
            
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }
    }
}
