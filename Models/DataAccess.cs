using MongoDB.Driver;

namespace WeatherApp.Models
{
    public class DataAccess<T>
    {
        public IMongoCollection<T> collection;

        public DataAccess(string databaseName, string dataCollectionName)
        {
            // In a real world scenario this would be held in external configuration
            var connectionString = "mongodb://localhost:27017";

            var client = new MongoClient(connectionString);

            collection = client.GetDatabase(databaseName).GetCollection<T>(dataCollectionName);
        }

    }
}