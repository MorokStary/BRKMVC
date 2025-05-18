using AutoMapper;
using FinTrack.Api.Contracts.User;
using FinTrack.Api.Services.Implementations;
using FinTrack.Api.Services.Interfaces;
using FinTrack.Models;
using Microsoft.AspNetCore.Mvc;



namespace FinTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(string))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> LoginUser([FromQuery] CreateUserDto userCreate)
    {
        if (userCreate == null)
            return BadRequest(ModelState);

        var userMap = _mapper.Map<CreateUserDto>(userCreate);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var login = await _userService.LoginAsync(userMap.Name, userMap.Password);

        if (login.IsFailure)
        {
            ModelState.AddModelError("", login.Error);
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<ReadUserDto>(login.Value);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(user);

    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> RegistrationUser([FromBody] CreateUserDto userCreate)
    {
        if (userCreate == null)
            return BadRequest(ModelState);

        var registration = await _userService.RegisterUserAsync(userCreate.Name, userCreate.Password);
       
        if (registration.IsFailure)
        {
            ModelState.AddModelError("", registration.Error);
            return StatusCode(422, ModelState);
        }
        return Ok();
    }
}
