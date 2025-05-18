using AutoMapper;
using FinTrack.Api.Contracts.IncomeCategory;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeCategoryController : ControllerBase
    {
        private readonly IIncomeCategoryService _incomeCategoryService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IncomeCategoryController(IIncomeCategoryService incomeCategoryService, IUserService userService, IMapper mapper)
        {
            _incomeCategoryService = incomeCategoryService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReadIncomeCategoryDto>))]
        public async Task<IActionResult> GeIncomeCategories(int userId)
        {
            var categories = await _incomeCategoryService.GetIncomeCategoriesAsync(userId);
            if (categories.IsFailure)
            {
                return NotFound();
            }

            var categoriesMap = _mapper.Map<IEnumerable<ReadIncomeCategoryDto>>(categories.Value.ToList());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categoriesMap);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateIncomeCategory([FromQuery] int userId, [FromBody] CreateIncomeCategoryDto incomeCategoryCreate)
        {
            if (incomeCategoryCreate == null)
                return BadRequest(ModelState);

            var category = _mapper.Map<IncomeCategory>(incomeCategoryCreate);

            var user = await _userService.GetUserById(userId);

            category.UserId = userId;

            var result = await _incomeCategoryService.CreateIncomeCategoryAsync(category);
            if (result.IsFailure)
            {
                ModelState.AddModelError("", result.Error);
                return StatusCode(422, ModelState);
            }

            return Ok();
        }
    }
}
