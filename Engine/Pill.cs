namespace Engine
{
    using System;
    using System.Linq;

    public class Pill
    {
        public Matrix Matrix { get; private set; }
        public bool Placed { get; set; }
        public Position Position { get; private set; }
        public bool UniColored { get; private set; }
        public bool Rotated { get; private set; }

        public int UniqueRotations => this.UniColored ? 2 : 4;

        public Pill(Position position)
        {
            Matrix = GetRandomPill();
            Placed = false;
            Position = position;
        }

        public Pill(Color type1, Color type2, Position position)
        {
            UniColored = type1 == type2;
            Matrix = new Matrix((byte) type1, (byte) type2);
            Placed = false;
            Position = position;
        }

        public Pill Clone()
        {
            return new Pill(new Position(this.Position))
            {
                Placed = this.Placed,
                Rotated = this.Rotated,
                Matrix = new Matrix(this.Matrix)
            };
        }

        public void Move(Move move)
        {
            switch (move)
            {
                case Engine.Move.Down:
                    this.Position.Row--;
                    break;

                case Engine.Move.Left:
                    this.Position.Column--;
                    break;

                case Engine.Move.Right:
                    this.Position.Column++;
                    break;

                case Engine.Move.RotateLeft:
                    this.Matrix.RotateLeft();
                    this.Rotated = !this.Rotated;
                    break;

                default:
                {
                    throw new InvalidOperationException("Move not valid");
                }
            }
        }

        internal static Matrix GetRandomPill()
        {
            var blockTypes = Enum.GetValues(typeof(Color)).Cast<Color>().ToArray();

            return new Matrix((byte) Random.Instance().NextEnum<Color>(),
                (byte) Random.Instance().NextEnum<Color>());
        }

        internal void Merge(Pill tempPill)
        {
            this.Matrix = new Matrix(tempPill.Matrix);
            this.Placed = tempPill.Placed;
            this.Position = new Position(tempPill.Position);
            this.Rotated = tempPill.Rotated;
            this.UniColored = tempPill.UniColored;
        }
    }
}