using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            //Seeding Data For ProductBrands Table to seed just for first time
            if (!context.ProductBrands.Any())
            {
                //Read All Data From Brand Json Fiel
                var brandText = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                //Convert All Text Data to List<ProductBrand> to insert at it
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandText);
                //check if brans is not null or is not Empty
                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                        //Add All Brand to ProductBrands Table at DB
                        await context.Set<ProductBrand>().AddAsync(brand);
                    //Save all Changes to Added to DB
                    await context.SaveChangesAsync();
                }
            }

            //Seeding Data For ProductTypes Table to seed just for first time
            if (!context.ProductTypes.Any())
            {
                //Read All Data From Brand Json Fiel
                var typeText = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                //Convert All Text Data to List<ProductType> to insert at it
                var types = JsonSerializer.Deserialize<List<ProductType>>(typeText);
                //check if brans is not null or is not Empty
                if (types?.Count() > 0)
                {
                    foreach (var type in types)
                        //Add All Brand to ProductType Table at DB
                        await context.Set<ProductType>().AddAsync(type);
                    //Save all Changes to Added to DB
                    await context.SaveChangesAsync();
                }
            }

            //Seeding Data For Products Table to seed just for first time
            if (!context.Products.Any())
            {
                //Read All Data From Brand Json Fiel
                var productText = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                //Convert All Text Data to List<ProductBrand> to insert at it
                var products = JsonSerializer.Deserialize<List<Product>>(productText);
                //check if brans is not null or is not Empty
                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                        //Add All Brand to Product Table at DB
                        await context.Set<Product>().AddAsync(product);
                    //Save all Changes to Added to DB
                    await context.SaveChangesAsync();
                }
            }

            if(!context.DeliveryMethods.Any())
            {
                var deliveryMethodtext = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodtext);

                if(deliveryMethods?.Count > 0)
                {
                    try
                    {
                        foreach (var deliveyMethod in deliveryMethods)
                            await context.Set<DeliveryMethod>().AddAsync(deliveyMethod);

                        await context.SaveChangesAsync();
                    }
                    catch(Exception exr)
                    {
                        Console.WriteLine($"badr {exr.ToString()}");
                    }
                }
            }
        }
    }
}
