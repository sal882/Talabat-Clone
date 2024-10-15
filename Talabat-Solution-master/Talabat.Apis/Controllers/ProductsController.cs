using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.DTOS;
using Talabat.Apis.Errors;
using Talabat.Apis.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.Apis.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<ProductType> _typesRepo;
        ///private readonly IGenericRepository<ProductBrand> _brandsRepo;

        public ProductsController(
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<ProductType> typesRepo,
            ///IGenericRepository<ProductBrand> brandsRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            ///_productRepo = productRepo;
            ///_typesRepo = typesRepo;
            ///_brandsRepo = brandsRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Get All Products
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductsSpecParams specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            
            var countSpec = new ProductWithFilterationForCountSpecifications(specParams);
            var dataCount = await _unitOfWork.Repository<Product>().GetCountWitheSpecAsync(countSpec);
            return Ok(new Pagination<ProductToReturnDTO>(specParams.PageSize, specParams.PageIndex, dataCount,data));
        }

        ///Get specific Product based on id
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            if (product is null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));
        }

        //Get All Brands of Products
        [HttpGet("brands")] // Get: baseURL/api/Products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(brands);
        }

        //Get All Types of Products
        [HttpGet("types")] // Get: baseURL/api/Products/types
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(types);
        }
    }
}
