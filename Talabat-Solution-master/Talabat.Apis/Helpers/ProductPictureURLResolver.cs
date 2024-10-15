using AutoMapper;
using Talabat.Apis.DTOS;
using Talabat.Core.Entities;

namespace Talabat.Apis.Helpers
{
    /// <summary>
    /// this class used to resolve the picture url for product to add the base url before it's 
    /// path, to when i click it open in from server, all picture are stored in server
    /// at wwwroot/images/product/[picture name]
    /// </summary>
    public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// inject a object from IConfiguration service to connect with appsettings file
        /// </summary>
        /// <param name="configuration">this the object</param>
        public ProductPictureURLResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            ///check if the product return with picture path or not
            ///if don't we will return empty string at it
            if (source.PictureUrl is not null)
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";

            return string.Empty ;
        }
    }
}
