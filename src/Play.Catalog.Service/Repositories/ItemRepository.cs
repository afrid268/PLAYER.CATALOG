using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class ItemRepository : IItemRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;//variable that holds instance of above collection
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;//filter builders , used for query filtering

        public ItemRepository(IMongoDatabase dataBase)
        {
            dbCollection = dataBase.GetCollection<Item>(collectionName);
            //asynchronous model used , best aproach
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {//collection that only is read not modified
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        //methods
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);//equality based filter Eq
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}