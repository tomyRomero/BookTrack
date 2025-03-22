using backend.Models;
using backend.Models.DTOs;
using backend.Services.Interfaces;
using backend.Repositories.Interfaces;
using backend.Utilities.Interfaces;


namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public UserService(IUserRepository userRepository, IHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }

    public async Task<UserRegistrationResponseDTO> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO)
    {
        //handle null values if any
        ArgumentNullException.ThrowIfNull(userRegistrationDTO);
        ArgumentNullException.ThrowIfNull(userRegistrationDTO.Username);
        ArgumentNullException.ThrowIfNull(userRegistrationDTO.Password);

        var existingUser = await _userRepository.GetUserByUsernameAsync(userRegistrationDTO.Username);

        if (existingUser != null)
        {
            throw new Exception("Username is already taken.");
        }

        // Generate salt and hash the password using the Hasher class
        var salt = _hasher.GenerateSalt();
        var hashedPassword = _hasher.HashPassword(userRegistrationDTO.Password, salt);
        var user = new User
        {
            Username = userRegistrationDTO.Username,
            PasswordHash = hashedPassword,
            Salt = salt,
        };

        //add user
        await _userRepository.AddUserAsync(user); 
        var addedUser = await _userRepository.GetUserByUsernameAsync(user.Username) ?? throw new Exception("User was not added correctly.");

         //return added user details
        return new UserRegistrationResponseDTO
        {
            UserId = addedUser.UserId,
            Username = addedUser.Username,
        };

    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        return user;
    }

    


}


