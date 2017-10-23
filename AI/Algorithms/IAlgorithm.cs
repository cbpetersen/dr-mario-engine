using Engine;

namespace AI.Algorithms
{
    public interface IAlgorithm
    {
        float CalculateFitness(byte[][] gameBoard, Pill previousBlock, int bacteriasCleared, int pillsCleared);
    }
}
