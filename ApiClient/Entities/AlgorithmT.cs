using ApiClient.Interfaces;

namespace ApiClient.Entities
{
    public class AlgorithmT<TWeights> : IAlgorithmT<TWeights>
    {
        public string Name { get; set; }
        public string AlgorithmId { get; set; }
        public TWeights Weights { get; set; }
    }
}
