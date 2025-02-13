using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos
{
    public record ItemDto(Guid itemId, string itemName, string itemDescription, decimal itemPrice, DateTimeOffset itemCreatedDate);

    public record CreatedItemDto([Required] string itemName, string itemDescription, [Range(0, 1000)] decimal itemPrice);

    public record UpdateItemDto([Required] string itemName, string itemDescription, [Range(0, 1000)] decimal itemPrice);
}