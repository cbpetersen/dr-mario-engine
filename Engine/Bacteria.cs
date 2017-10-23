namespace Engine
{
    public class Bacteria
    {
        public Position Position { get; }
        public Color Color { get; }

        public Bacteria(Position position, Color color)
        {
            Position = position;
            Color = color;
        }
    }
}