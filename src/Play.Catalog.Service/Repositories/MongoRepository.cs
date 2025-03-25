using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;//variable that holds instance of above collection
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;//filter builders , used for query filtering

        public MongoRepository(IMongoDatabase dataBase, string collectionName)
        {
            dbCollection = dataBase.GetCollection<T>(collectionName);
            //asynchronous model used , best aproach
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {//collection that only is read not modified
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        //methods
        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);//equality based filter Eq
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}