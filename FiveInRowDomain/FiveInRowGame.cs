using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRowDomain
{
    public class FiveInRowGame
    {
        public IList<Move> Moves { get; private set; } = new List<Move>();
        private Mover NextMover { get; set; } = Mover.X;
        public bool AddMove(int x, int y)
        {
            var occupied = Moves.Where(m => (m.X == x) && (m.Y == y)).Any();
            if (occupied) return false;
            var move = new Move() { X = x, Y = y, Mover = NextMover };
            Moves.Add(move);
            NextMover = NextMover == Mover.X ? Mover.O : Mover.X;
            return true;
        }
    }

    public class Move
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Mover Mover { get; set; }
    }

    public enum Mover
    {
        X, O
    }
}
