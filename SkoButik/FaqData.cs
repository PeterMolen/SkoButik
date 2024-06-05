using Microsoft.ML.Data;

namespace SkoButik
{
    public class FaqData
    {
        [LoadColumn(0)] // Antager att 'Question' är i den första kolumnen (index 0)
        public string Question { get; set; }

        [LoadColumn(1)] // Antager att 'Answer' är i den andra kolumnen (index 1)
        public string Answer { get; set; }
    }
}
