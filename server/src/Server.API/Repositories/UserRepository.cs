using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Server.API.Models;
using Server.API.Models.Context;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories.Interfaces;

namespace Server.API.Repositories;

public class UserRepository(StoriContext context, IMapper mapper) : IUserRepository
{
  private readonly StoriContext _context = context;
  private readonly IMapper _mapper = mapper;

  public async Task<User?> GetUserByIdAsync(int id)
  {
    var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);

    return user;
  }

  public async Task<User?> GetUserByEmailAsync(string email)
  {
    var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

    return user;
  }

  public Task<UserAuthorizedResponse?> GetUserResponseByIdAsync(int id)
  {
    return _context
      .Users.Where(u => u.Id == id)
      .ProjectTo<UserAuthorizedResponse>(_mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();
  }

  public Task<UserUnauthorizedResponse?> GetUserByUsernameAsync(string username)
  {
    return _context
      .Users.AsNoTracking()
      .Where(b => b.Username == username)
      .ProjectTo<UserUnauthorizedResponse>(_mapper.ConfigurationProvider)
      .SingleOrDefaultAsync();
  }

  public async Task<Envelope<UserAuthorizedResponse>?> PatchUserAsync(
    int Id,
    string? Email,
    string? Password,
    string? Username,
    string? Name,
    string? ProfileImg
  )
  {
    var user = await _context.Users.SingleOrDefaultAsync(user => user.Id == Id);

    if (user is null)
    {
      return null;
    }

    if (!string.IsNullOrEmpty(Email))
    {
      user.Email = Email;
    }

    if (!string.IsNullOrEmpty(Password))
    {
      user.Password = Password;
    }

    if (!string.IsNullOrEmpty(Username))
    {
      user.Username = Username;
    }

    if (!string.IsNullOrEmpty(Name))
    {
      user.Name = Name;
    }

    if (!string.IsNullOrEmpty(ProfileImg))
    {
      user.ProfileImg = ProfileImg;
    }

    _context.SaveChanges();

    var mappedUser = _mapper.Map<UserAuthorizedResponse>(user);

    return new Envelope<UserAuthorizedResponse>([mappedUser]);
  }

  public async Task<bool> IsEmailInUseAsync(string email)
  {
    var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

    return user is not null;
  }

  public async Task<bool> IsUsernameInUseAsync(string username)
  {
    var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

    return user is not null;
  }

  public async Task<bool> IsUserInDatabaseAsync(int userId)
  {
    var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

    return user is not null;
  }

  public async Task<User> SaveUserAsync(User user)
  {
    var result = await _context.Users.AddAsync(user);
    await _context.SaveChangesAsync();

    return result.Entity;
  }

  public async Task<User?> RemoveUserPhotoAsync(int userId)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

    if (user is null)
    {
      return null;
    }

    user.ProfileImg = null;

    await _context.SaveChangesAsync();

    return user;
  }
}
