using System.Threading.Tasks;
using FiveInRowDomain;
using Microsoft.AspNetCore.SignalR;

namespace FiveInRow.Hubs
{
    public class GameHub : Hub
    {
        private Dictionary<string, FiveInRowMultiplayer> games = new();

        public GameHub(GStorage storage) 
        {
            games = storage.games;
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
            games[game.Id] = game;
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);

            await Clients.Group(game.Id).SendAsync("GameCreated", game);
        }

        public async Task AcceptGame(string gameId, string userId)
        {
            if (!games.ContainsKey(gameId))
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId);
                return;
            }
            var game = games[gameId];
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
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Id);
            await Clients.Group(game.Id).SendAsync("GameStarted", game);
        }

        public async Task Move(string gameId, string userId, int x, int y)
        {
            if (!games.ContainsKey(gameId))
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId, "move");
                return;
            }
            var game = games[gameId];
            string err = game.AddMove(new Player(userId), x, y);
            if (err == "")
            {
                _ = game.Game.WhoWon();
                await Clients.Group(game.Id).SendAsync("GameMove", game);
            } else
            {
                await Clients.Caller.SendAsync("WrongMove", game, err);
            }
        }
        public async Task Reset(string gameId)
        {
            if (!games.ContainsKey(gameId))
            {
                await Clients.Caller.SendAsync("WrongGameId", gameId, "reset");
                return;
            }
            var game = games[gameId];
            if (game.Game.IsActive())
            {
                await Clients.Caller.SendAsync("CannotReset", "game is in progress");
                return;
            }
            game.Game.Reset();
            var pTmp = game.X;
            game.X = game.O;
            game.O = pTmp;

            await Clients.Group(gameId).SendAsync("GameStarted", game);
        }
    }
    public class GStorage
    {
        public Dictionary<string, FiveInRowMultiplayer> games = new();
    }
}
