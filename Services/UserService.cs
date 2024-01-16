using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDevBP.Entities;
using ApiDevBP.Models;
using SQLite;

public class UserService : IUserService, IDisposable
{
    private readonly SQLiteConnection _db;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;

    public UserService(IOptions<DatabaseOptions> databaseOptions, ILogger<UserService> logger, IMapper mapper)
    {
        _db = new SQLiteConnection(databaseOptions.Value.ConnectionString);
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<bool> SaveUser(UserModel user)
    {
        try
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            var result = _db.Insert(userEntity);

            if (result > 0)
            {
                return true;
            }
            else
            {
                _logger.LogWarning("No se pudo insertar el usuario en la base de datos.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar guardar un usuario.");
            throw;
        }
    }

    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        try
        {
            var users = _db.Query<UserEntity>($"Select * from Users");
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar obtener usuarios.");
            throw;
        }
    }

    public async Task<bool> UpdateUser(int id, UserModel updatedUser)
    {
        try
        {
            var existingUser = _db.Table<UserEntity>().FirstOrDefault(u => u.Id == id);

            if (existingUser != null)
            {
                _mapper.Map(updatedUser, existingUser);

                var result = _db.Update(existingUser);
                return result > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar actualizar un usuario.");
            throw;
        }
    }

    public async Task<bool> DeleteUser(int id)
    {
        try
        {
            var existingUser = _db.Table<UserEntity>().FirstOrDefault(u => u.Id == id);

            if (existingUser != null)
            {
                var result = _db.Delete(existingUser);
                return result > 0;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar eliminar un usuario.");
            throw;
        }
    }

    // Cerramos la conexión a la base de datos al liberar el recurso.
    public void Dispose()
    {
        try
        {
            _db.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar cerrar la conexión a la base de datos.");
        }
    }
}
