using Rps.Domain.AI;
using System;

namespace Rps.Domain.Model
{
    public class Player
        : IEquatable<Player>
    {
        private IPlayerStrategy playerStrategy;

        public const string ComputerPlayerID = "A4524AE5-F254-45D9-97B8-9F0A0FEE06C7";
        public const string ComputerPlayerName = "Computer";

        public static readonly Player Empty = new Player(string.Empty, string.Empty, false);
        public static readonly Player ComputerPlayer = new Player(ComputerPlayerID, ComputerPlayerName, true);

        public string ID { get; private set; }
        public string Name { get; private set; }
        public bool IsComputerControlled { get; private set; }
        

        public Player(string id,
                      string name,
                      bool isComputerControlled)
        {
            this.ID = id;
            this.Name = name;
            this.IsComputerControlled = isComputerControlled;
        }

        public IPlayerStrategy GetPlayerStrategy()
        {
            if (!IsComputerControlled)
            {
                throw new InvalidOperationException("Player strategy is not valid for human controlled player.");
            }

            if (playerStrategy == null)
            {
                playerStrategy = new BasicPlayerStrategyV1();
            }

            return playerStrategy;
        }

        public bool Equals(Player other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID
                && this.Name == other.Name
                && this.IsComputerControlled == other.IsComputerControlled;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Player);
        }

        public override int GetHashCode()
        {
            return (this.ID ?? string.Empty).GetHashCode()
                ^ (this.Name ?? string.Empty).GetHashCode()
                ^ this.IsComputerControlled.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
