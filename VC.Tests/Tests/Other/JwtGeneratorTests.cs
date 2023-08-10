using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using VC.Helpers.JWT;
using VC.Tests.Data;
using Xunit;

namespace VC.Tests.Tests.Other
{
    public class JwtGeneratorTests
    {
        private readonly IConfiguration _config;
        private readonly JwtGenerator _jwtGenerator;

        public JwtGeneratorTests()
        {
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"JwtSettings:Key", "testKeytestKeytestKeytestKeytestKey"}
                })
                .Build();
            _jwtGenerator = new JwtGenerator(_config);
        }

        [Fact]
        public void CreateToken_ReturnsValidToken()
        {
            var mongoClaims = new List<MongoClaim> {
                    new MongoClaim(){ Type = "CanDelete", Value = "true" }
                };

            var userName = "testUser";
            var email = "test@test.com";

            // Arrange
            var user = TestDataHelper.CreateApplicationUser(userName, email, mongoClaims);

            // Act
            var token = _jwtGenerator.CreateToken(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            Assert.Equal(mongoClaims.First().Type, jwtSecurityToken.Claims.Skip(2).First().Type);
            Assert.Equal(mongoClaims.First().Value, jwtSecurityToken.Claims.Skip(2).First().Value);

            Assert.Equal(userName, jwtSecurityToken.Claims.First().Value);
            Assert.Equal(email, jwtSecurityToken.Claims.Skip(1).First().Value);
        }
    }
}
