using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class WeatherForecast
    {
        #region properties

        // In a real world scenario I would use IOC for better instance 
        // management and testability I thought it would be a bit overkill 
        // for these purposes
        private static readonly IMongoCollection<WeatherForecast> _collection = new DataAccess<WeatherForecast>("weather", "WeatherForecasts").collection;

        [JsonIgnore]
        public ObjectId Id { get; set; }
        // There is some discussion around public passing of ids. In this 
        // trivial case where all of the data is publicly accessible and
        // modifiable anyway, it seemed like the best solution.
        public string _id => Id.ToString();
        public DateTime Date { get; set; }     
        public string Region { get; set; }
        public string Season { get; set; }
        public string Summary { get; set; }

        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        // helper to allow user to pass datetime value as string into
        // the 'put' endpoint
        [JsonIgnore]
        public string DateTimeString {
            get { return Date.ToString(); }
            set { Date = Convert.ToDateTime(value); }
        }

        #endregion 


        #region helpers

        public static IEnumerable<WeatherForecast> GetAll()
        {
            return _collection.AsQueryable();
        }
 
 
        public static WeatherForecast GetById(string id)
        {
            return _collection.Find<WeatherForecast>(f => f.Id == ObjectId.Parse(id)).Single();
        }

        // Simple aggregation pipeline example. I opted for aggregation 
        // from the choices provided as it seemed like the most
        // 'real-worldly' choice given the application
        public static IEnumerable<SeasonalTemps> GetAverageSeasonalTemps(string region)
        {
            var pipeline = new BsonDocument[] {
                new BsonDocument{ { "$match", new BsonDocument("Region", region) }},
                new BsonDocument{{"$group", new BsonDocument{
                    {"_id", "$Season"},
                    {"Average", new BsonDocument{{"$avg","$TemperatureC"}}}}}}};

            return  _collection.Aggregate<SeasonalTemps> (pipeline).ToList();
        }

        #endregion


        #region CUD
 
         public void Save()
         {
             _collection.InsertOne(this);
         }
 
        // Trivial example of how to update.
        // In a real world scenario would at least include some validation
        public void Update(string fieldname, string fieldvalue)
        {
            var update = Builders<WeatherForecast>.Update.Set(fieldname, fieldvalue);
            _collection.UpdateOne(f => f.Id == Id, update);
        }

        public void Delete()
        {
            var operation = _collection.DeleteOne(f => f.Id == Id);
        }

        #endregion
    }
}
