namespace FiveInRowDomain
{
    public class FiveInRowGame
    {
        public IList<Move> Moves { get;  set; } = new List<Move>();
        public Mover NextMover { get;  set; } = Mover.X;
        //private Dictionary<string, Mover> moveDict = new();

        public Mover? Winner { get; set; } = null;

        public bool IsActive() => Winner == null;

        public FiveInRowGame()
        {
            //init();
        }

        public void Reset()
        {
            Winner = null;
            Moves.Clear();
            NextMover = Mover.X;
            AddMove(0, 0);
        }

        public bool AddMove(int x, int y)
        {
            //var moveDict = InitMovesDict();
//            var occupied = moveDict.ContainsKey($"x={x}y={y}");//Moves.Where(m => (m.X == x) && (m.Y == y)).Any();
            var occupied = Moves.Where(m => (m.X == x) && (m.Y == y)).Any();
            if (occupied) return false;
            var mover = NextMover;
            var move = new Move() { X = x, Y = y, Mover = mover };
            Moves.Add(move);
            NextMover = NextMover == Mover.X ? Mover.O : Mover.X;
            return true;
        }

        public Mover? WhoWon()
        {
            if (Winner == null)
            {
                Winner = CalculateWinner();
            }
            return Winner;
        }

        public Mover? CalculateWinner()
        {
            var moveDicst = InitMovesDict();
            foreach (var move in Moves)
            {
                if (CheckDirection(move, 1, 0, moveDicst) || // горизонталь
                    CheckDirection(move, 0, 1, moveDicst) || // вертикаль
                    CheckDirection(move, 1, 1, moveDicst) || // диагональ вниз
                    CheckDirection(move, 1, -1, moveDicst))  // диагональ вверх
                {
                    return move.Mover;
                }
            }
            return null;
        }

        private Dictionary<string, Mover> InitMovesDict()
        {
            Dictionary<string, Mover> moveDict = new();
            foreach (var m in Moves)
            {
                moveDict[$"x={m.X}y={m.Y}"] = m.Mover;
            }
            return moveDict;
        }

        private bool CheckDirection(Move startMove, int deltaX, int deltaY, Dictionary<string, Mover> moveDict)
        {
            int count = 1;
            int x = startMove.X;
            int y = startMove.Y;

            // Проверяем в одну сторону
            count += CountInDirection(x, y, deltaX, deltaY, startMove.Mover, moveDict);
            // Проверяем в другую сторону
            count += CountInDirection(x, y, -deltaX, -deltaY, startMove.Mover, moveDict);

            return count >= 5;
        }

        private int CountInDirection(int startX, int startY, int deltaX, int deltaY, Mover mover, Dictionary<string, Mover> moveDict)
        {
            int count = 0;
            int x = startX + deltaX;
            int y = startY + deltaY;

//            while (Moves.Any(m => m.X == x && m.Y == y && m.Mover == mover))
            while (moveDict.ContainsKey($"x={x}y={y}") && moveDict[$"x={x}y={y}"] == mover)
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
