using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRowDomain
{
    public class FiveInRowMultiplayer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now;

        public FiveInRowGame Game { get; set; } = new();
        public Player? X { get; set; } = null;
        public Player? O { get; set; } = null;
        public int GetDimension()
        {
            int maxX = Game.Moves.Select(x => Math.Abs(x.X)).Max() * 2;
            int maxY = Game.Moves.Select(x => Math.Abs(x.Y)).Max() * 2;
            return maxX > maxY ? maxX : maxY;
        }
        public Mover? GetMover(Player p)
        {
            if (X == p) return Mover.X;
            if (O == p) return Mover.O;
            return null;
        }
        public string AddMove(Player p, int x, int y)
        {
            Mover m;
            if (p == X)
            {
                m = Mover.X;
            } else  if (p == O)
            {
                m = Mover.O;
            } else
            {
                return "Wrong Player"; // wrong player
            }
            if (Game.NextMover != m)
            {
                return "Wrong Move Sequence"; // not his turn
            }
            Game.AddMove(x, y);
            return "";

        }
    }
}
