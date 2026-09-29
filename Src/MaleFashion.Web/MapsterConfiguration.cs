using MaleFashion.Domain.Entities;
using MaleFashion.Web.Areas.Admin.Models;
using Mapster;
using System.Collections;

namespace MaleFashion.Web
{
    public class MapsterConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config.NewConfig<CategoryAddCommand, Category>();
            //config.NewConfig<CategoryModel, CategoryAddCommand>();
            //config.NewConfig<ProductAddCommand, Product>();
            //config.NewConfig<ProductModel, ProductAddCommand>();
            //config.NewConfig<StockAddCommand, Stock>();
            //config.NewConfig<StockModel, StockAddCommand>();

            // config.NewConfig<StockModel, StockAddCommand>();

        }
    }
}
