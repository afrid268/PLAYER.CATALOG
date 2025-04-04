using System;

namespace Play.Catalog.Service.Entities
{

    public class Item : IEntity//common shared usage DRY implementation , we generalise item in IItems
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}