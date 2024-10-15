using Talabat.Core.Entities;

namespace Talabat.Apis.DTOS
{
    /// <summary>
    /// this is the DTO Class Which represent the return Product Data to the End user [frontend]
    /// and i customize more properties and i will mapping it by autoMapper Package
    /// </summary>
    public class ProductToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; } 
        public string ProductBrand { get; set; }
        public int ProductTypeId { get; set; } 

        public string ProductType { get; set; }
    }
}
