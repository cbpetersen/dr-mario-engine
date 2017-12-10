using ApiClient.Interfaces;

namespace ApiClient.Entities
{
    public class Algorithm : IAlgorithm
    {
        public string Name { get; set; }
        public string AlgorithmId { get; set; }
    }
}