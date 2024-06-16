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
        private Dictionary<(int x, int y), Mover> moveDict = new(); 
        public bool AddMove(int x, int y)
        {
            var occupied = moveDict.ContainsKey((x, y));//Moves.Where(m => (m.X == x) && (m.Y == y)).Any();
            if (occupied) return false;
            var mover = NextMover;
            var move = new Move() { X = x, Y = y, Mover = mover };
            Moves.Add(move);
            moveDict[(x, y)] = mover;
            NextMover = NextMover == Mover.X ? Mover.O : Mover.X;
            return true;
        }

        public Mover? WhoWon()
        {
            foreach (var move in Moves)
            {
                if (CheckDirection(move, 1, 0) || // горизонталь
                    CheckDirection(move, 0, 1) || // вертикаль
                    CheckDirection(move, 1, 1) || // диагональ вниз
                    CheckDirection(move, 1, -1))  // диагональ вверх
                {
                    return move.Mover;
                }
            }
            return null;
        }

        private bool CheckDirection(Move startMove, int deltaX, int deltaY)
        {
            int count = 1;
            int x = startMove.X;
            int y = startMove.Y;

            // Проверяем в одну сторону
            count += CountInDirection(x, y, deltaX, deltaY, startMove.Mover);
            // Проверяем в другую сторону
            count += CountInDirection(x, y, -deltaX, -deltaY, startMove.Mover);

            return count >= 5;
        }

        private int CountInDirection(int startX, int startY, int deltaX, int deltaY, Mover mover)
        {
            int count = 0;
            int x = startX + deltaX;
            int y = startY + deltaY;

//            while (Moves.Any(m => m.X == x && m.Y == y && m.Mover == mover))
            while (moveDict.ContainsKey((x, y)) && moveDict[(x, y)] == mover)
            {
                count++;
                x += deltaX;
                y += deltaY;
            }

            return count;
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
