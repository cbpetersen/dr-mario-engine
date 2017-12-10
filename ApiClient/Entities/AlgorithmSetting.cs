namespace ApiClient.Entities
{
    using Interfaces;

    public class AlgorithmSetting<TWeights> : IAlgorithmT<TWeights>
    {
        public string Name { get; set; }
        public TWeights Weights { get; set; }
        public int EvolutionNumber { get; set; }
        public int OverallAvgFitness { get; set; }
        public int BestFitness { get; set; }
        public int EvolutionFitness { get; set; }
        public string AlgorithmId { get; set; }
        public string WeightsId { get; set; }
        public bool Active { get; set; }
    }
}
