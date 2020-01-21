using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Server.Services.DatabaseService
{
    interface IMongoDbRepository<T> where T : class
    {
        T InsertOne(T model);
        List<T> Find();
        T FindOne(string id);
        T UpdateOne(string id, T model);
        bool DeleteOne(string id);

    }


    public class MongoDbService<T> : IMongoDbRepository<T>, IDatabaseService<T> where T : class
    {

        public IMongoDatabase Database { get; }

        public MongoDbService(MongoClient client)
        {
            Database = client.GetDatabase("YourNote");
        }

        

        #region IMongoDbRepository Implementation
        public T InsertOne(T model)
        {
            string collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);

            collection.InsertOne(model);
            return model;
        }

        public List<T> Find()
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Empty;

            return collection.Find(filter).ToList();

        }

        public T FindOne(string id)
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("id", id);
            return collection.Find(filter).First();

        }

        public T UpdateOne(string id, T model)
        {

            var filter = Builders<T>.Filter.Eq("id", id);

            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);
            collection.ReplaceOne(filter, model);
            return model;
        }

        public bool DeleteOne(string id)
        {
            var filter = Builders<T>.Filter.Eq("id", id);

            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);
            var deleteResult = collection.DeleteOne(filter);
            return deleteResult.IsAcknowledged;
        }
        #endregion


        #region CRUD methods

        public T Create(T obj)
        {
            return InsertOne(obj);
            
        }

        public T Read(string id)
        {
            return FindOne(id);
        }

        public IList<T> Read()
        {
            return Find();
        }

        public T Update(string id, T obj)
        {
            return UpdateOne(id, obj);
        }

        public bool Delete(string id)
        {
            return DeleteOne(id);
        }
        #endregion


        #region private Methods
        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(MyBsonCollectionAttribute), true)
                .FirstOrDefault()
                as MyBsonCollectionAttribute).CollectionName;
        }
        #endregion
    }
}
