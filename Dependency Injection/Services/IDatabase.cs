using System.Collections.Generic;

namespace DependencyInjection.Services
{
    // Defines basic operations for a string-based database
    public interface IDatabase
    {
        // Number of unique keys stored in the database
        public int Size { get; }

        // Adds a string entry under the specified key
        public void AddString(string key, string newData);

        // Retrieves all strings associated with the given key
        public IEnumerable<string> GetData(string key);

        // Deletes all stored data
        public void DeleteAll();
    }
}
