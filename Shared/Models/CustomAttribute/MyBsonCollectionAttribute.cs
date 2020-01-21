using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MyBsonCollectionAttribute : Attribute
    {
        private readonly string _collectionName;

        public MyBsonCollectionAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }

        public string CollectionName => _collectionName;

    }
}
