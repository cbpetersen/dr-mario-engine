using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Engine
{
    using System.Linq;

    public class BoardManager
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly IList<Bacteria> _bacterias = new List<Bacteria>();

        public virtual Pill ActivePill { get; private set; }
        public Pill PreviousPill { get; private set; }
        public virtual byte[][] GameBoard { get; private set; }
        public virtual GameStats GameStats { get; }

        public virtual int NumberOfColumns => this._columns;

        public BoardManager(byte[][] gameBoard) : this(gameBoard, null)
        {
        }

        public BoardManager(byte[][] gameBoard, Pill activePill)
        {
            this.GameBoard = gameBoard;
            this._rows = gameBoard.GetLength(0);
            this.ActivePill = activePill;
            this._columns = gameBoard[0].Length;
            this.GameStats = new GameStats();
        }

        internal BoardManager(byte[][] gameBoard, Pill activePill, GameStats gameStats)
        {
            this.GameBoard = gameBoard;
            this._rows = gameBoard.GetLength(0);
            this.ActivePill = activePill;
            this._columns = gameBoard[0].Length;
            this.GameStats = gameStats;
        }

        public bool CanSpawn()
        {
            if (this.ActivePill != null && this.ActivePill.Placed)
            {
                return false;
            }
            var leftSpawnArea = (this._columns - 4) / 2;

            for (var row = this._rows - 1; row >= this._rows - 1; row--)
            {
                if (this.GameBoard[row].Skip(leftSpawnArea).Take(2).Any(x => x != 0))
                {
                    return false;
                }
            }

            return true;
        }

        public Pill SpawnPill(Color type1, Color type2)
        {
            if (!this.CanSpawn())
            {
                throw new InvalidOperationException("can't spawn a new pill");
            }

            this.ActivePill = new Pill(type1, type2,
                new Position {Column = (this._columns - 2) / 2, Row = this._rows - 1});
            this.GameStats.NewPillSpawned();

            return this.ActivePill;
        }

        public ICollection<Position> PositionsBelowWithSameType(Position pos, int type)
        {
            var positions = new List<Position>();
            if (this.GameBoard[pos.Row][pos.Column] != type)
            {
                return positions;
            }

            positions.Add(pos);
            for (var row = pos.Row - 1; row >= 0; row--)
            {
                if (this.GameBoard[row][pos.Column] != type)
                {
                    break;
                }

                positions.Add(new Position(pos.Column, row));
            }

            for (var row = pos.Row + 1; row < this._rows; row++)
            {
                if (this.GameBoard[row][pos.Column] != type)
                {
                    return positions;
                }

                positions.Add(new Position(pos.Column, row));
            }

            return positions;
        }

        public ICollection<Position> PositionsOnSameRowWithSameType(Position pos, int type)
        {
            var positions = new List<Position>();
            if (this.GameBoard[pos.Row][pos.Column] != type)
            {
                return positions;
            }

            positions.Add(pos);
            for (var column = pos.Column - 1; column >= 0; column--)
            {
                if (this.GameBoard[pos.Row][column] != type)
                {
                    break;
                }

                positions.Add(new Position(column, pos.Row));
            }

            for (var column = pos.Column + 1; column < this.NumberOfColumns; column++)
            {
                if (this.GameBoard[pos.Row][column] != type)
                {
                    return positions;
                }

                positions.Add(new Position(column, pos.Row));
            }

            return positions;
        }


        public IEnumerable<Position> CheckBoard()
        {
            return CheckBoard(this.ActivePill ?? this.PreviousPill);
        }

        public IEnumerable<Position> CheckBoard(Pill pill)
        {
            const int minClearing = 3;
            var remove = new List<Position>();
            var below1 = new List<Position>();
            var below2 = new List<Position>();
            var next1 = new List<Position>();
            var next2 = new List<Position>();

            // Row BELOW

            if (!pill.Rotated)
            {
                below1.AddRange(PositionsBelowWithSameType(pill.Position, pill.Matrix.X1));
                below2.AddRange(PositionsBelowWithSameType(new Position(pill.Position.Column + 1, pill.Position.Row),
                    pill.Matrix.X2));
            }
            else if (pill.UniColored)
            {
                below1.AddRange(PositionsBelowWithSameType(pill.Position, pill.Matrix.X1));
            }
            else
            {
                below1.AddRange(PositionsBelowWithSameType(pill.Position, pill.Matrix.X1));
                below2.AddRange(PositionsBelowWithSameType(pill.Position, pill.Matrix.Y1));
            }


            // Column

            if (!pill.Rotated && pill.UniColored)
            {
                next1.AddRange(PositionsOnSameRowWithSameType(pill.Position, pill.Matrix.X1));
            }
            else if (!pill.Rotated)
            {
                next1.AddRange(PositionsOnSameRowWithSameType(pill.Position, pill.Matrix.X1));
                next2.AddRange(PositionsOnSameRowWithSameType(new Position(pill.Position.Column + 1, pill.Position.Row),
                    pill.Matrix.X2));
            }
            else
            {
                next1.AddRange(PositionsOnSameRowWithSameType(pill.Position, pill.Matrix.X1));
                next2.AddRange(PositionsOnSameRowWithSameType(new Position(pill.Position.Column, pill.Position.Row - 1),
                    pill.Matrix.Y1));
            }

            // Console.WriteLine($"below1: {below1.Count}");
            // Console.WriteLine($"below2: {below2.Count}");
            // Console.WriteLine($"Next1: {next1.Count}");
            // Console.WriteLine($"Next2: {next2.Count}");

            if (below1.Count >= minClearing)
            {
                remove.AddRange(below1);
            }

            if (below2.Count >= minClearing)
            {
                remove.AddRange(below2);
            }

            if (next1.Count >= minClearing)
            {
                remove.AddRange(next1);
            }

            if (next2.Count >= minClearing)
            {
                remove.AddRange(next2);
            }

            return remove.GroupBy(p => new {p.Column, p.Row})
                .Select(g => g.First())
                .ToList();
        }

        public byte[][] GetBoard()
        {
            return this.GameBoard;
        }

        public bool Move(Move move)
        {
            if (move == Engine.Move.None)
            {
                return true;
            }

            if (this.ActivePill == null)
            {
                return false;
            }

            var tempMove = this.ActivePill.Clone();
            tempMove.Move(move);

            var validMove = this.CheckPill(tempMove);
             Console.WriteLine($"valid moved? {validMove} ~ {move:G}");
            if (!validMove && (move == Engine.Move.Down || move == Engine.Move.Fall))
            {
                LockAndCheck();
                return true;
            }
            else if (!validMove)
            {
                return false;
            }

            this.ActivePill = tempMove;

            return true;
        }

        public bool IsValidMove(Move move)
        {
            var tempMove = this.ActivePill.Clone();
            tempMove.Move(move);

            return this.CheckPill(tempMove);
        }

        internal bool CheckPill(Pill pill)
        {
            var column1 = pill.Position.Column;
            var row1 = pill.Position.Row;
            var column2 = pill.Rotated ? column1 : column1 + 1;
            var row2 = pill.Rotated ? row1 - 1 : row1;

            if (row1 < 0 || row2 < 0)
            {
                return false;
            }
            if (row2 > _rows - 1 || row2 > _rows - 1)
            {
                return false;
            }
            
            if (column1 < 0 || column2 < 0)
            {
                return false;
            }
            if (column1 > NumberOfColumns - 1 || column2 > NumberOfColumns - 1)
            {
                return false;
            }
            
            // X1
            if (this.GameBoard[row1][column1] != 0)
            {
                return false;
            }
            
            // X2 / Y1
            if (this.GameBoard[row2][column2] != 0)
            {
                return false;
            }
            
            return true;
        }

        internal void LockAndCheck()
        {
            this.Lockblock();
            var positions = this.CheckBoard();
            RemoveAtPositions(positions.ToArray());
        }

        internal void Lockblock()
        {
            this.PreviousPill = this.ActivePill;
            this.ActivePill = null;
            // Console.WriteLine("Lock started");
            for (var row = this.PreviousPill.Position.Row;
                row < this.PreviousPill.Position.Row + 2 && row < this._rows;
                row++)
            {
                for (var column = this.PreviousPill.Position.Column;
                    column < this.PreviousPill.Position.Column + 2 && column < this._columns;
                    column++)
                {
                    if (row < 0)
                    {
                        continue;
                    }

                    if (row >= this._rows)
                    {
                        continue;
                    }

                    if (column < 0)
                    {
                        continue;
                    }

                    if (column >= this._columns)
                    {
                        continue;
                    }

                    if (this.GameBoard[row][column] != 0)
                    {
                        continue;
                    }

                    // lock
                    // x1
                    if (row == this.PreviousPill.Position.Row && column == this.PreviousPill.Position.Column &&
                        this.PreviousPill.Matrix.X1 != 0)
                    {
                        this.GameBoard[row][column] = this.PreviousPill.Matrix.X1;
                        // Console.WriteLine($"Lock yx1 at {row}, {column}");
                        if (this.PreviousPill.Matrix.X2 != 0)
                        {
                            this.GameBoard[row][column + 1] = this.PreviousPill.Matrix.X2;
                            // Console.WriteLine($"Lock x2 at {row}, {column + 1}");
                        }
                        if (this.PreviousPill.Matrix.Y1 != 0)
                        {
                            // Console.WriteLine($"Lock y1 at {row + 1}, {column}");
                            this.GameBoard[row - 1][column] = this.PreviousPill.Matrix.Y1;
                        }
                    }
                }
            }

            // Console.WriteLine("Lock done");
        }

        internal BoardManager RemoveAtPositions(Position[] positions)
        {
            Console.WriteLine($"Remove: {positions.Length}");
            var pills = 0;
            var bacterias = 0;
            foreach (var position in positions)
            {
                this.GameBoard[position.Row][position.Column] = 0;
                var backteria = _bacterias.FirstOrDefault(x => x.Position == position);
//                var backteria = _bacterias.FirstOrDefault(x => x.Position.Column == position.Column && x.Position.Row == position.Row);
                if (backteria != null)
                {
                    _bacterias.Remove(backteria);
                    bacterias++;
                }
                else
                {
                    pills++;
                }
            }

            this.GameStats.NewBacteriaClearings(bacterias);
            this.GameStats.NewPillClearings(pills);
            // TODO: Should stuff above collapse?

            return this;
        }

        public Bacteria AddBacteria(Bacteria bacteria)
        {
            this.GameBoard[bacteria.Position.Row][bacteria.Position.Column] = (byte) bacteria.Color;
            _bacterias.Add(bacteria);

            return bacteria;
        }
    }
}
