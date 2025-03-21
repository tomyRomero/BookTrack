using backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using backend.Utilities.Interfaces;


namespace backend.Utilities;

//Hasher class focuses on security-related operations like hashing passwords and creating tokens. 

//encapsulate all JWT-related logic, keeping it separate from the AuthService.
public class Hasher : IHasher
{
    private readonly JwtSettings _jwtSettings;

    // Constructor takes IOptions<JwtSettings> to initialize JwtSettings
    public Hasher(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    // Generates a salt using a cryptographically secure random number generator
    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
            // Populate the salt array with random bytes
        }

        // Return the salt as a Base64-encoded string
        return Convert.ToBase64String(saltBytes);

    }

    // Hashes the password using the provided salt and HMACSHA512 algorithm
    public string HashPassword(string password, string salt)
    {
        if (string.IsNullOrEmpty(salt))
        {
            throw new FormatException("Salt cannot be null or empty.");
        }

        // Convert the Base64-encoded salt string to a byte array
        byte[] saltBytes = Convert.FromBase64String(salt);

         // Initialize HMACSHA512 with the salt bytes as the key
        using var hmac = new HMACSHA512(saltBytes);

        // Compute the hash of the password using the HMACSHA512 algorithm
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        // Return the hashed password as a Base64-encoded string
        return Convert.ToBase64String(hashBytes);
    }

    // Verifies if the provided password matches the stored hash, using stored salt
    public bool VerifyPassword(string password, string storedHash, string storedSalt)
    {

        // Handle null or empty salt
        if (string.IsNullOrEmpty(storedSalt))
        {
            throw new ArgumentNullException(nameof(storedSalt), "Salt cannot be null or empty.");
        }

        // Handle null hash, return false since password does not match
        if (storedHash == null)
        {
            return false;
        }

        // Convert the stored salt to a byte array
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        // Use HMACSHA512 to compute the hash of the provided password with the stored salt
        using var hmac = new HMACSHA512(saltBytes);
        // Convert the computed hash to a Base64 string
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        string computedHashString = Convert.ToBase64String(computedHash);

        // Return whether the computed hash matches the stored hash
        return computedHashString == storedHash;
    }


    // Generates a JWT (JSON Web Token) for the user, with expiration time
    public string GenerateJwtToken(User user)
    {
        Console.WriteLine("GenerateJwtToken() is running");

        //Part of the System.IdentityModel.Tokens.Jwt, used to create and validate JSON Web Tokens
        var tokenHandler = new JwtSecurityTokenHandler();

        //ensure _jwtSettings is not null at runtime, key is needed to sign the token
        if (string.IsNullOrEmpty(_jwtSettings.Secret))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        //GetBytes(): This has to return at least 32 bytes. 
        //And bytes do not necessarily equal number of characters 
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
    
        //I left this here for debugging purposes
        Console.WriteLine("key (Base64): " + Convert.ToBase64String(key));

        // Define the token descriptor which contains claims, expiration, and signing credentials
        //Using Microsoft.IdentityModel.Tokens
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Set the claims for the token, including the username as a claim
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
            }),

            // Set the expiration time for the token
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),

            // Use HMACSHA256 with the secret key to sign the token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // Create the token using the token descriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine("token from GenerateJwtToken(): " + token);

        // Return the token as a string
        return tokenHandler.WriteToken(token);
    }


}