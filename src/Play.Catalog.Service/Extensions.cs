using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service
{// mapping of dto and repository entities
    public static class Extensions
    {
        public static ItemDto AsDto(this Item value)
        {
            //item entitiy to itemdto
            return new ItemDto(value.Id, value.Name, value.Description, value.Price, value.CreatedDate);
        }
    }
}