using System.Threading.Tasks;
using FiveInRow.Storage;
using FiveInRowDomain;
using Microsoft.AspNetCore.SignalR;

namespace FiveInRow.Hubs
{
    public class GameHub : Hub
    {
        //private Dictionary<string, FiveInRowMultiplayer> games = new();
        private IGStorage gameStorage;

        public GameHub(IGStorage storage) 
        {
            gameStorage = storage;
        }

        public async Task CreateGame(string userId, Mover mover)
        {
            var game = new FiveInRowMultiplayer();
            if (mover == Mover.X)
            {
                game.X = new Player(userId);
            } else
            {
                game.O = new Player(userId);
            }
            gameStorage.StoreGame(game.Id, game);
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);

            await Clients.Group(game.Id).SendAsync("GameCreated", game);
        }

        public async Task AcceptGame(string gameId, string userId)
        {
            var game = gameStorage.LoadGame(gameId);
            if (game == null)
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId);
                return;
            }
            
            if (game.X != null && game.O != null)
            {
                // already accepted
                await Clients.Caller.SendAsync("GameAlreadyAccepted", gameId, "accept");
                return;
            }
            if (game.X == null)
            {
                game.X = new Player(userId);
            }
            else
            {
                game.O = new Player(userId);
            }
            gameStorage.StoreGame(gameId, game);

            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);
            await Clients.Group(game.Id).SendAsync("GameStarted", game);
        }

        public async Task Move(string gameId, string userId, int x, int y)
        {
            var game = gameStorage.LoadGame(gameId);
            if (game == null)
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId, "move");
                return;
            }
            string err = game.AddMove(new Player(userId), x, y);
            if (err == "")
            {
                _ = game.Game.WhoWon();
                gameStorage.StoreGame(gameId, game);
                await Clients.Group(game.Id).SendAsync("GameMove", game);
            } else
            {
                await Clients.Caller.SendAsync("WrongMove", game, err);
            }
        }
        public async Task Reset(string gameId)
        {
            var game = gameStorage.LoadGame(gameId);
            if (game == null)
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId, "reset");
                return;
            }
            if (game.Game.IsActive())
            {
                await Clients.Caller.SendAsync("CannotReset", "game is in progress");
                return;
            }
            game.Game.Reset();
            var pTmp = game.X;
            game.X = game.O;
            game.O = pTmp;
            gameStorage.StoreGame(gameId, game);

            await Clients.Group(gameId).SendAsync("GameStarted", game);
        }
    }
}
