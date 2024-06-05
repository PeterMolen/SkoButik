using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace SkoButik.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly PredictionEngine<FaqData, FaqPrediction> _predictionEngine;
        public ChatBotController()
        {
            // Skapa en MLContext
            var mlContext = new MLContext();
            // Ladda den tränade modellen
            var loadedModel = mlContext.Model.Load("model.zip", out var modelSchema);
            // Skapa en prediction engine
            _predictionEngine = mlContext.Model.CreatePredictionEngine<FaqData, FaqPrediction>(loadedModel);
        }

        [HttpGet]
        public IActionResult Ask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ask(string question)
        {
            var prediction = _predictionEngine.Predict(new FaqData { Question = question });
            ViewBag.Answer = prediction.Answer;
            return View();
        }
    }
}
