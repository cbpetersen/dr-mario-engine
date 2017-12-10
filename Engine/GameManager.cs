using System;
using System.Collections.Generic;
using System.Linq;
using Engine.GameStates;
using Engine.GameStates.Interfaces;

using System.Runtime.CompilerServices;
[assembly:InternalsVisibleTo("Test")]
[assembly:InternalsVisibleTo("AI")]
[assembly:InternalsVisibleTo("AIConsoleRunner")]

namespace Engine
{
    public class GameManager
    {
        private readonly BoardManager _boardManager;
        private readonly Random _random;
        private readonly int _maxDefaultBacteriaHeight;

        public virtual IGameState GameState { get; private set; }

        public GameManager(int height, int width)
        {
            _random = Random.Instance();
            var gameBoard = new byte[height][];
            for (var i = 0; i < height; i++)
            {
                gameBoard[i] = new byte[width];
            }

            _maxDefaultBacteriaHeight = (int)Math.Floor(height * 0.6);

            this._boardManager = new BoardManager(gameBoard);
            this.GameState = new Playing();
        }

        public BoardManager BoardManager => this._boardManager;
        public GameStats GameStats => this._boardManager.GameStats;
        public Pill ActiveBlock => this._boardManager.ActivePill;
        public Pill PreviousBlock => this._boardManager.PreviousPill;
        public IList<Bacteria> Bacterias => _boardManager.Backterias;

        public void OnGameLoopStep()
        {
            if (this.GameState.IsPaused())
            {
                return;
            }

            if (this.GameState.IsGameOver())
            {
                return;
            }

            if (this._boardManager.ActivePill == null)
            {
                try
                {
                    var values = this._boardManager.GameBoard.SelectMany(x => x.Select(y => y)).Distinct().Where(x => x != 0).ToArray();
                    this._boardManager.SpawnPill((Color)_random.Next(values), (Color)_random.Next(values));
                }
                catch (Exception)
                {
                    this.GameState = new GameOver();
                }
                return;
            }

            this.MoveBlock(Move.Down);
        }

        public bool MoveBlock(Move move)
        {
            return this._boardManager.Move(move);
        }

        public Bacteria AddBacteria()
        {
            return AddBacteria(_random.NextEnum<Color>());
        }

        public Bacteria AddBacteria(Color color)
        {
            return AddBacteria(_random.Next(this._boardManager.GameBoard, _maxDefaultBacteriaHeight), color);
        }

        private Bacteria AddBacteria(Position position, Color color)
        {
            return _boardManager.AddBacteria(new Bacteria(position, color));
        }

        public IEnumerable<Bacteria> AddBacterias(int count, int colors = 3)
        {
            return AddBacterias(count, _maxDefaultBacteriaHeight, colors);
        }

        public IEnumerable<Bacteria> AddBacterias(int count, int maxHeight,int colors)
        {
            var bacterias = new List<Bacteria>();
            var colorList = new List<Color>();
            for (var i = 0; i < colors; i++)
            {
                colorList.Add(i == 0 ? _random.NextEnum<Color>() : _random.NextEnum(colorList));
            }

            // Todo: Bacteria vs color rest creations
            for (var i = 0; i < colors; i++)
            {
                for (var j = 0; j < count / colors; j++)
                {
                    bacterias.Add(AddBacteria(_random.Next(this.BoardManager.GameBoard, maxHeight), colorList[i]));
                }
            }

            return bacterias;
        }
    }
}
