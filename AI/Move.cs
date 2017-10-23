using System;
using System.Collections.Generic;

namespace AI
{
    public class Move
    {
        private Engine.Move[] _moves;

        internal int Rotation { get; set; }

        internal int GameboardWidth { get; set; }

        internal int ColumnOffSet { get; set; }

        public float Fitness { get; set; }

        public bool IsValid { get; set; }

        public Engine.Move[] Moves
        {
            get
            {
                if (this._moves == null)
                {
                    var list = new List<Engine.Move>();
                    list.Add(Engine.Move.None);

                    for (var i = 0; i < this.Rotation; i++)
                    {
                        list.Add(Engine.Move.RotateLeft);
                    }

                    var dir = this.ColumnOffSet < 0 ? Engine.Move.Left : Engine.Move.Right;
                    for (int i = 0; i < Math.Abs(this.ColumnOffSet); i++)
                    {
                        list.Add(dir);
                    }

                    this._moves = list.ToArray();
                }

                return this._moves;
            }
        }
    }
}
