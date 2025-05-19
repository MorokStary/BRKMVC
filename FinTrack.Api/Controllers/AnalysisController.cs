using FinTrack.Api.Contracts.Analysis;
using FinTrack.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet("overview")]
        [ProducesResponseType(200, Type = typeof(AnalysisOverviewResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOverview([FromQuery] int userId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _analysisService.GetOverviewAsync(userId, from, to);
            return Ok(result);
        }

        [HttpGet("recommendations")]
        [ProducesResponseType(200, Type = typeof(List<RecommendationResponse>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRecommendations([FromQuery] int userId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _analysisService.GetRecommendationsAsync(userId, from, to);
            return Ok(result);
        }

        [HttpGet("anomalies")]
        [ProducesResponseType(200, Type = typeof(List<AnomalyResponse>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAnomalies([FromQuery] int userId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _analysisService.GetAnomaliesAsync(userId, from, to);
            return Ok(result);
        }
    }
}
