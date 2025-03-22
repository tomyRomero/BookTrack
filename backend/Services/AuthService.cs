using backend.Models.DTOs;
using backend.Services.Interfaces;
using backend.Repositories.Interfaces;
using backend.Utilities.Interfaces;

namespace backend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public AuthService(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }


    public async Task<UserResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        // Validate that the Username and Password are not null
        if (string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
        {
            throw new ArgumentException("Username and Password are required.");
        }

        // Retrieve the user from the database
        var user = await _userRepository.GetUserByUsernameAsync(loginDTO.Username) ?? throw new UnauthorizedAccessException("Invalid username or password.");

        // Retrieve the stored password hash and salt
        var storedHash = user.PasswordHash;
        var storedSalt = user.Salt;

        // Verify the provided password
        if (!_hasher.VerifyPassword(loginDTO.Password, storedHash, storedSalt))
        {
            // Password does not match
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        //Generate and return JWT token if password is correct
        var JwtToken = _hasher.GenerateJwtToken(user);

        return new UserResponseDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Token = JwtToken
        };
    }


    public Task LogoutAsync()
    {
        //Invalidate the current user's JWT on the client side
        return Task.CompletedTask;
    }

}

