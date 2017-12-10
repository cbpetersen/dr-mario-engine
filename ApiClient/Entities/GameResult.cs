using ApiClient.Interfaces;

namespace ApiClient.Entities
{
    public class GameResult<TWeights> : IGameResult
    {
        public GameResult(AlgorithmSetting<TWeights> setting)
        {
            this.Name = setting.Name;
            this.AlgorithmId = setting.AlgorithmId;
            this.WeightsId = setting.WeightsId;
        }

        public string Name { get; set; }
        public string AlgorithmId { get; set; }
        public string WeightsId { get; set; }
        public int Fitness { get; set; }
        public int PillsSpawned { get; set; }
        public int Bacterias { get; set; }
        public int Pills { get; set; }
    }
}
