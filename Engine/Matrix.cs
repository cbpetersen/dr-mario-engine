namespace Engine
{
    /// <summary>
    ///
    /// ##########
    /// # x1, x2 #
    /// # y1, y2 #
    /// ##########
    /// </summary>
    public class Matrix
    {
        public byte X1 { get; private set; }
        public byte X2 { get; private set; }
        public byte Y1 { get; private set; }

        public byte Y2
        {
            get { return 0; }
        }

        public Matrix(byte x1, byte x2)
        {
            this.X1 = x1;
            this.X2 = x2;
            this.Y1 = 0;
        }

        public Matrix(Matrix matrix)
        {
            X1 = matrix.X1;
            X2 = matrix.X2;
            Y1 = matrix.Y1;
        }

        public override string ToString()
        {
            return $"{X1},{X2},{Y1},{Y2}";
            //return string.Format("[Matrix: x1={0}, x2={1}, y1={2}, y2={3}]", x1, x2, y1, y2);
        }

        public void RotateLeft()
        {
            if (Y1 == 0)
            {
                Y1 = X1;
                X1 = X2;
                X2 = 0;
            }
            else
            {
                X2 = Y1;
                Y1 = 0;
            }
        }
    }
}