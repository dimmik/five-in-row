using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveInRowDomain
{
    public class FiveInRowMultiplayer
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public FiveInRowGame Game { get; private set; } = new();
        public Player? X { get; set; } = null;
        public Player? O { get; set; } = null;
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
    public class Player : IEquatable<Player?>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Player(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Player);
        }

        public bool Equals(Player? other)
        {
            return other is not null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(Player? left, Player? right)
        {
            return EqualityComparer<Player>.Default.Equals(left, right);
        }

        public static bool operator !=(Player? left, Player? right)
        {
            return !(left == right);
        }
    }
}
