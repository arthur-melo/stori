using FluentAssertions;
using Server.API.Models;
using Server.API.Models.Dtos.Responses;
using Server.API.Models.Entities;
using Server.API.Repositories;
using Server.UnitTests.Helpers;

namespace Server.UnitTests.Repositories;

public class UserRepositoryTests : BaseTests
{
  [Fact]
  public async Task GetUserByIdAsync_ValidUser_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserByIdAsync(id);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().BeEquivalentTo(user);
  }

  [Fact]
  public async Task GetUserByIdAsync_InvalidUser_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserByIdAsync(1);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetUserByEmailAsync_ValidUser_ReturnsValidResponse()
  {
    // Arrange
    var email = "user@example.com";

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = email,
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserByEmailAsync(email);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(user);
  }

  [Fact]
  public async Task GetUserByEmailAsync_InvalidUser_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserByEmailAsync("");

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetUserResponseByIdAsync_ReturnsValidResponse()
  {
    // Arrange
    var id = 1;
    var username = "username";

    var user = new User()
    {
      Id = id,
      Username = username,
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    var userResponse = autoMapper
      .mapper.ProjectTo<UserAuthorizedResponse>(QueryableUtils.MapToIQueryable(user))
      .First();

    // Act
    var actual = await userRepository.GetUserResponseByIdAsync(id);

    // Assert
    actual.Should().NotBeNull();
    actual!.username.Should().Be(username);
  }

  [Fact]
  public async Task GetUserResponseByIdAsync_InvalidId_ReturnsEmptyResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserResponseByIdAsync(0);

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetUserByUsernameAsync_InvalidUsername_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.GetUserByUsernameAsync("");

    // Assert
    actual.Should().BeNull();
  }

  [Fact]
  public async Task PatchUserAsync_InvalidUser_ReturnsNullResponse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.PatchUserAsync(1, null, null, null, null, null);

    // Assert
    actual.Should().BeNull();
  }

  [Theory]
  [InlineData("username", null, null, null, null)]
  [InlineData(null, "user@example.com", null, null, null)]
  [InlineData(null, null, "password", null, null)]
  [InlineData(null, null, null, "name", null)]
  [InlineData(null, null, null, null, "image.png")]
  public async Task PatchUserAsync_ValidUser_ReturnsValidResponse(
    string? username,
    string? email,
    string? password,
    string? name,
    string? profileImg
  )
  {
    // Arrange
    var createdAt = DateTime.UtcNow;

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
      CreatedAt = createdAt,
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    var patchedUser = new User()
    {
      Username = username ?? "",
      Email = email ?? "",
      Password = password ?? "",
      Name = name ?? "",
      ProfileImg = profileImg,
      CreatedAt = createdAt,
    };

    var patchedUserResponse = new Envelope<UserAuthorizedResponse>(
      [
        autoMapper
          .mapper.ProjectTo<UserAuthorizedResponse>(QueryableUtils.MapToIQueryable(patchedUser))
          .First(),
      ]
    );

    // Act
    var actual = await userRepository.PatchUserAsync(
      1,
      email,
      password,
      username,
      name,
      profileImg
    );

    // Assert
    actual.Should().NotBeNull();
    actual.Should().BeEquivalentTo(patchedUserResponse);
    _context.Users.Should().HaveCount(1);
  }

  [Fact]
  public async Task IsEmailInUseAsync_ValidEmail_ReturnsTrue()
  {
    // Arrange
    var email = "user@example.com";

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = email,
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsEmailInUseAsync(email);

    // Assert
    actual.Should().BeTrue();
  }

  [Fact]
  public async Task IsEmailInUseAsync_InvalidEmail_ReturnsFalse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsEmailInUseAsync("email");

    // Assert
    actual.Should().BeFalse();
  }

  [Fact]
  public async Task IsUsernameInUseAsync_ValidUsername_ReturnsTrue()
  {
    // Arrange
    var username = "username";

    var user = new User()
    {
      Id = 1,
      Username = username,
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsUsernameInUseAsync(username);

    // Assert
    actual.Should().BeTrue();
  }

  [Fact]
  public async Task IsUsernameInUseAsync_InvalidUsername_ReturnsFalse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsUsernameInUseAsync("");

    // Assert
    actual.Should().BeFalse();
  }

  [Fact]
  public async Task IsUserInDatabaseAsync_ValidId_ReturnsTrue()
  {
    // Arrange
    var id = 1;

    var user = new User()
    {
      Id = id,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsUserInDatabaseAsync(id);

    // Assert
    actual.Should().BeTrue();
  }

  [Fact]
  public async Task IsUserInDatabaseAsync_InvalidId_ReturnsFalse()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var actual = await userRepository.IsUserInDatabaseAsync(1);

    // Assert
    actual.Should().BeFalse();
  }

  [Fact]
  public async Task SaveUserAsync_ValidUser_ReturnsValidResponse()
  {
    // Arrange
    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
    };

    // Act
    var actual = await userRepository.SaveUserAsync(user);

    // Assert
    actual.Should().NotBeNull();
    actual.Should().Be(user);
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_RemovePhoto_ReturnsValidResponse()
  {
    // Arrange
    var user = new User()
    {
      Id = 1,
      Username = "",
      Email = "",
      Password = "",
      Name = "",
      ProfileImg = "some-img.png",
    };

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var userEntity = await userRepository.RemoveUserPhotoAsync(1);

    // Assert
    userEntity.Should().NotBeNull();
    userEntity!.ProfileImg.Should().BeNull();
  }

  [Fact]
  public async Task RemoveUserPhotoAsync_InvalidUser_ReturnsNull()
  {
    // Arrange
    var autoMapper = new AutoMapperFactory();
    var userRepository = new UserRepository(_context, autoMapper.mapper);

    // Act
    var userEntity = await userRepository.RemoveUserPhotoAsync(1);

    // Assert
    userEntity.Should().BeNull();
  }
}
