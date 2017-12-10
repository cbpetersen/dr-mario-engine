namespace ApiClient.Interfaces
{
    public interface IAlgorithmT<TWeights> : IAlgorithm
    {
        TWeights Weights { get; set; }
    }
}