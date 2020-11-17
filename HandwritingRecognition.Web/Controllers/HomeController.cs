using AutoMapper;
using HandwritingRecognition.BL.Interfaces;
using HandwritingRecognition.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HandwritingRecognition.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPredictionService _predictionService;
        private readonly IMapper _mapper;

        public HomeController(IPredictionService predictionService, IMapper mapper)
        {
            _predictionService = predictionService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PredictOnImage()
        {
            return View();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadAsync(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return BadRequest(new PredictionResultViewModel
                {
                    Prediction = '-'
                });
            }

            var resultDto = await _predictionService.PredictCharacterFromImageAsync(base64Image);

            var result = _mapper.Map<PredictionResultViewModel>(resultDto);

            return Ok(result);
        }
    }
}
