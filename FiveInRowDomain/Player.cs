namespace FiveInRowDomain
{
    public class Player : IEquatable<Player?>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Playyerr";

        public Player(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
        public Player()
        {
            // nothing - id is random Guid
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
