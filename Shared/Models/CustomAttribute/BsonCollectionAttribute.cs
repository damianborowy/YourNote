using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        private readonly string _collectionName;

        public BsonCollectionAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }

        public string CollectionName => _collectionName;

    }
}
