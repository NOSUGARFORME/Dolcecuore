using System.Collections.Generic;
using System.Linq;

namespace Dolcecuore.Services.Catalog.Api.Models;

public static class ProductModelMappingConfiguration
{
    public static IEnumerable<ProductModel> ToModels(this IEnumerable<Entities.Product> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static ProductModel ToModel(this Entities.Product entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Category = entity.Category,
            Description = entity.Description,
            ImagePath = entity.ImagePath,
            Price = entity.Price
        };
    }

    public static Entities.Product ToEntity(this ProductModel model)
    {
        return new Entities.Product
        {
            Id = model.Id,
            Name = model.Name,
            Category = model.Category,
            Description = model.Description,
            ImagePath = model.ImagePath,
            Price = model.Price
        };
    }
}