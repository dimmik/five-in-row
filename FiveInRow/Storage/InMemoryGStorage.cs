using FiveInRowDomain;
using Newtonsoft.Json;

namespace FiveInRow.Storage
{
    public class InMemoryGStorage : IGStorage
    {

        private Dictionary<string, string> games = new();

        public FiveInRowMultiplayer? LoadGame(string gameId)
        {
            if (games.ContainsKey(gameId))
            {
                var gm = games[gameId];
                FiveInRowMultiplayer? g = JsonConvert.DeserializeObject<FiveInRowMultiplayer>(gm);
                return g;
            }
            else
            {
                return null;
            }
        }

        public bool StoreGame(string gameId, FiveInRowMultiplayer game)
        {
            var j = JsonConvert.SerializeObject(game);
            games[gameId] = j;
            return true;
        }

        public string WhoAmI()
        {
            return "In Memory Storage";
        }
    }
}
