namespace Engine
{
    using System;

    public class GameStats
    {
        public int PillsSpawned { get; private set; }
        public int Fitness => this.TotalBacteriaClearings + this.TotalPillClearings;
        public int TotalPillClearings { get; private set; }
        public int TotalBacteriaClearings { get; private set; }

        public GameStats()
        {
            this.PillsSpawned = 0;
            this.TotalBacteriaClearings = 0;
            this.TotalPillClearings = 0;
        }

        public void NewPillSpawned()
        {
            this.PillsSpawned++;
        }

        public void NewPillClearings(int clearedRows)
        {
            this.TotalPillClearings += clearedRows;
        }
          
        public void NewBacteriaClearings(int numbers)
        {
            this.TotalBacteriaClearings += numbers;
        }

        public GameStats Clone()
        {
            return new GameStats
            {
                TotalBacteriaClearings = this.TotalBacteriaClearings,
                TotalPillClearings = this.TotalPillClearings,
                PillsSpawned = this.PillsSpawned
            };
        }
    }
}