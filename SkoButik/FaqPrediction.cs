using Microsoft.ML.Data;

namespace SkoButik
{
    public class FaqPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Answer { get; set; }
    }
}
