namespace ApiClient.Interfaces
{
    public interface IGameResult
    {
        string AlgorithmId { get; set; }
        string WeightsId { get; set; }
        int Fitness { get; set; }
        int Pills { get; set; }
        int PillsSpawned { get; set; }
        int Bacterias { get; set; }
    }
}