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

    }


    public class MongoDbService<T> : IMongoDbRepository<T>, IDatabaseService<T> where T : class
    {

        public IMongoDatabase Database { get; }

        public MongoDbService(IMongoClient client)
        {
            Database = client.GetDatabase("YourNote");
        }

        

        #region IMongoDbRepository Implementation
        public T InsertOne(T model)
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);

            collection.InsertOne(model);
            return model;
        }

        public List<T> Find()
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);

            return collection.Find(_ => true).ToList();
            
        }

        public T FindOne(string id)
        {
            var collectionName = GetCollectionName();
            var collection = Database.GetCollection<T>(collectionName);
            var stringFilter = "{ _id: ObjectId('" + id + "') }";
            return collection.Find(stringFilter).First();

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

        public T Update(T obj)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region private Methods
        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault()
                as BsonCollectionAttribute).CollectionName;
        }
        #endregion
    }
}
