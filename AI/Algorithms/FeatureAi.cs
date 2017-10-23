using AI.Algorithms.Weights;
using Engine;

namespace AI.Algorithms
{
    public class FeatureAi : IAlgorithm
    {
        private readonly float _landingHeight;
        private readonly float _rowTransitions;
        private readonly float _columnTransitions;
        private readonly float _numberOfHoles;
        private readonly float _wellSums;
        private readonly float _bacteriasCleared;
        private readonly float _pillsCleared;

        public FeatureAi(AiWeights aiWeights)
        {
            this._landingHeight = aiWeights.LandingHeight;
            this._rowTransitions = aiWeights.RowTransitions;
            this._columnTransitions = aiWeights.ColumnTransitions;
            this._bacteriasCleared = aiWeights.BacteriasCleared;
            this._pillsCleared = aiWeights.PillsCleared;
            this._numberOfHoles = aiWeights.NumberOfHoles;
            this._wellSums = aiWeights.WellSums;
        }

        public float CalculateFitness(byte[][] gameBoard, Pill previousBlock, int bacteriasCleared, int pillsCleared)
        {
            var features = new Features(gameBoard);
            var fitness = 0f;

            fitness += bacteriasCleared * this._bacteriasCleared;
            fitness += pillsCleared * this._pillsCleared;
            fitness += features.LandingHeight(previousBlock) * this._landingHeight;
            fitness += features.ColumnTransitions() * this._columnTransitions;
            fitness += features.NumberOfHoles() * this._numberOfHoles;
            fitness += features.RowTransitions() * this._rowTransitions;
            fitness += features.WellSums() * this._wellSums;

            return fitness;
        }
    }
}
