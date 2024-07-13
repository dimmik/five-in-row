using FiveInRowDomain;

namespace FiveInRow.Storage
{
    public interface IGStorage
    {
        public FiveInRowMultiplayer? LoadGame(string gameId);
        public bool StoreGame(string gameId, FiveInRowMultiplayer game);
    }
}
