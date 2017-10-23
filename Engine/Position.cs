using System;

namespace Engine
{
    public class Position
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Position()
        {
        }

        public Position(int column, int row)
        {
            Row = row;
            Column = column;
        }
        
        public Position(Position position)
        {
            Row = position.Row;
            Column = position.Column;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int) (Column + Row * 1E6);
        }

        public static bool operator ==(Position obj1, Position obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Column == obj2.Column && obj1.Row == obj2.Row;
        }

        public static bool operator !=(Position obj1, Position obj2)
        {
            return !(obj1 == obj2);
        }
    }
}