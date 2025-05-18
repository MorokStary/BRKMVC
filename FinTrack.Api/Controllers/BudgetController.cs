using AutoMapper;
using FinTrack.Api.Contracts.Budget;
using FinTrack.Api.Services.Implementations;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public BudgetController(
            IBudgetService budgetService,
            IUserService userService, 
            ICurrencyService currencyService,
            IMapper mapper)
        {
            _budgetService = budgetService;
            _userService = userService;
            _currencyService = currencyService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReadBudgetDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBudgets(int userId)
        {
            var budgets = _mapper.Map<IEnumerable<ReadBudgetDto>>(
                            (await _budgetService.GetBudgetsAsync(userId)).Value);
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(budgets);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateBudget([FromQuery] int userId, [FromQuery] int currencyId, [FromBody] CreateBudgetDto budgetCreate)
        {
            if (budgetCreate == null)
                return BadRequest(ModelState);

            var budget = _mapper.Map<Budget>(budgetCreate);

            if((await _userService.GetUserById(userId)).IsFailure)
            {
                ModelState.AddModelError("", "Non-existent user identifier");
                return BadRequest(ModelState);
            }

            if ((await _currencyService.IsCurrencyExists(currencyId)).IsFailure)
            {
                ModelState.AddModelError("", "Non-existent currency identifier");
                return BadRequest(ModelState);
            }

            budget.UserId = userId;
            budget.CurrencyId = currencyId;

            var result = await _budgetService.CreateBudgetAsync(budget);

            if (result.IsFailure)
            {
                ModelState.AddModelError("", result.Error);
                return StatusCode(422, ModelState);
            }

            return Ok();
        }
    }
}
