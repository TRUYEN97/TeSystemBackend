using Microsoft.Extensions.Configuration;
using TeSystemBackend.Application.Constants;

namespace TeSystemBackend.Application.Helpers;

public class JwtConfiguration
{
    public string Key { get; set; } = string.Empty;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int RefreshTokenExpiryDays { get; set; } = 7;

    public static JwtConfiguration FromConfiguration(IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(ConfigurationKeys.JwtSection);
        var key = Environment.GetEnvironmentVariable(ConfigurationKeys.JwtKeyEnvironmentVariable)
                  ?? jwtSection[ConfigurationKeys.JwtKey];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(ErrorMessages.JwtConfigurationMissing);
        }

        var refreshTokenDaysConfig = jwtSection[ConfigurationKeys.JwtRefreshTokenExpiryDays];
        var refreshTokenDays = 7;
        if (int.TryParse(refreshTokenDaysConfig, out var configDays) && configDays > 0)
        {
            refreshTokenDays = configDays;
        }

        return new JwtConfiguration
        {
            Key = key,
            Issuer = jwtSection[ConfigurationKeys.JwtIssuer],
            Audience = jwtSection[ConfigurationKeys.JwtAudience],
            RefreshTokenExpiryDays = refreshTokenDays
        };
    }

    public void ValidateKey()
    {
        if (string.IsNullOrWhiteSpace(Key))
        {
            throw new InvalidOperationException(ErrorMessages.JwtKeyNotConfigured);
        }
    }
}


