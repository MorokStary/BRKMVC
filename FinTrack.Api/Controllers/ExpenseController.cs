using AutoMapper;
using FinTrack.Api.Contracts.Expense;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseCategoryService _categoryService;
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;

        public ExpenseController(
            IExpenseService expenseService, 
            IExpenseCategoryService categoryService, 
            IBudgetService budgetService, 
            IMapper mapper)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
            _budgetService = budgetService;
            _mapper = mapper;
        }

        [HttpGet("{budgetId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReadExpenseDto>))]
        public async Task<IActionResult> GetExpenses(int budgetId)
        {
            var expenses = await _expenseService.GetExpensesAsync(budgetId);

            var expensesMap = _mapper.Map<IEnumerable<ReadExpenseDto>>(expenses.Value.ToList());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(expensesMap);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateExpense([FromQuery] int budgetId, [FromBody] CreateExpenseDto expenseCreate)
        {
            if (expenseCreate == null)
                return BadRequest(ModelState);

            var expense = _mapper.Map<Expense>(expenseCreate);

            if((await _budgetService.GetBudgetAsync(budgetId)).IsFailure)
            {
                return BadRequest(ModelState);
            }

            expense.BudgetId = budgetId;

            //var category = await _categoryService.GetExpenseCategory((int)expense.ExpenseCategoryId);

           // expense.ExpenseCategory = category.Value;
            var result = await _expenseService.CreateExpenseAsync(expense);
            if (result.IsFailure)
            {
                ModelState.AddModelError("", result.Error);
                return StatusCode(422, ModelState);
            }

            return Ok();
        }



    }
}
