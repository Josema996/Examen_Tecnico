using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ApiDevBP.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, ILogger<UsersController> logger, IMapper mapper)
    {
        _userService = userService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Guarda un nuevo usuario.
    /// </summary>
    /// <param name="user">Datos del nuevo usuario.</param>
    /// <returns>Indica si la operación fue exitosa.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveUser(UserModel user)
    {
        try
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var result = await _userService.SaveUser(userEntity);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar guardar un usuario.");
            return StatusCode(500, "Error interno del servidor al guardar el usuario.");
        }
    }

    /// <summary>
    /// Obtiene la lista de usuarios.
    /// </summary>
    /// <returns>Lista de usuarios.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userService.GetUsers();
            var userModels = _mapper.Map<IEnumerable<UserModel>>(users);

            return Ok(userModels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener usuarios.");
            return StatusCode(500, "Error interno del servidor al obtener usuarios.");
        }
    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    /// <param name="id">Identificador del usuario a actualizar.</param>
    /// <param name="updatedUser">Datos actualizados del usuario.</param>
    /// <returns>Indica si la operación fue exitosa.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int id, UserModel updatedUser)
    {
        try
        {
            var existingUser = await _userService.UpdateUser(id, _mapper.Map<UserEntity>(updatedUser));

            if (existingUser)
            {
                return Ok(existingUser);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar actualizar un usuario.");
            return StatusCode(500, "Error interno del servidor al actualizar el usuario.");
        }
    }

    /// <summary>
    /// Elimina un usuario existente.
    /// </summary>
    /// <param name="id">Identificador del usuario a eliminar.</param>
    /// <returns>Indica si la operación fue exitosa.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.DeleteUser(id);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar eliminar un usuario.");
            return StatusCode(500, "Error interno del servidor al eliminar el usuario.");
        }
    }
}