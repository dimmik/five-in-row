using MongoDB.Driver;
using MongoDB.Bson;
using System;
using FiveInRow.Storage;
using FiveInRowDomain;
using MongoDB.Bson.Serialization;

public class MongoGStorage : IGStorage
{
    private readonly string Collection = "FiveInRow";
    private readonly IMongoCollection<BsonDocument> _collection;

    public MongoGStorage(string srv, string login, string pwd)
    {
        var pwdSafe = pwd.Replace("@", "%40");
        var connectionString = $"mongodb+srv://{login}:{pwdSafe}@{srv}";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("GameDatabase");
        _collection = database.GetCollection<BsonDocument>(Collection);
    }

    public FiveInRowMultiplayer? LoadGame(string gameId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", gameId);
        var document = _collection.Find(filter).FirstOrDefault();

        if (document == null)
            return null;

        return BsonSerializer.Deserialize<FiveInRowMultiplayer>(document);
    }

    public bool StoreGame(string gameId, FiveInRowMultiplayer game)
    {
        try
        {
            var document = game.ToBsonDocument();
            document["_id"] = gameId;

            var filter = Builders<BsonDocument>.Filter.Eq("_id", gameId);
            var options = new ReplaceOptions { IsUpsert = true };

            var result = _collection.ReplaceOne(filter, document, options);

            return result.IsAcknowledged && (result.ModifiedCount > 0 || result.UpsertedId != null);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string WhoAmI()
    {
        return $"Mongo DB Storage";
    }
}