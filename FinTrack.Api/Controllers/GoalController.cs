using FinTrack.Api.Contracts.Goals;
using FinTrack.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalAnalysisService _goalAnalysisService;

        public GoalController(IGoalAnalysisService goalAnalysisService)
        {
            _goalAnalysisService = goalAnalysisService;
        }

        [HttpPost("analyze")]
        [ProducesResponseType(200, Type = typeof(GoalRecommendationResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AnalyzeGoal([FromBody] FinancialGoalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _goalAnalysisService.AnalyzeGoalAsync(request);
            return Ok(result);
        }
    }
}
