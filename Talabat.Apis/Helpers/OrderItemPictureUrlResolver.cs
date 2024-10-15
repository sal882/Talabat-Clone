using AutoMapper;
using Talabat.Apis.DTOS;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Apis.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
           
            return string.Empty ;
            
        }
    }
}
