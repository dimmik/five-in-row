using FiveInRowDomain;

namespace FiveInRow.Storage
{
    public class InMemoryGStorage : IGStorage
    {

        private Dictionary<string, FiveInRowMultiplayer> games = new();

        public FiveInRowMultiplayer? LoadGame(string gameId)
        {
            if (games.ContainsKey(gameId))
            {
                var gm = games[gameId];
                return gm;
            }
            else
            {
                return null;
            }
        }

        public bool StoreGame(string gameId, FiveInRowMultiplayer game)
        {
            games[gameId] = game;
            return true;
        }
    }
}
