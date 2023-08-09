using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VC.Models.Identity;

namespace VC.Tests.Data
{
    public static class MockingHelpers
    {
        public static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
        {
            return new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),                 //IUserStore<TUser> store
                null,                                                   //IOptions<IdentityOptions> optionsAccessor
                null,                                                   //IPasswordHasher<TUser> passwordHasher
                null,                                                   //IEnumerable<IUserValidator<TUser>> userValidators
                null,                                                   //IEnumerable < IPasswordValidator < TUser >> passwordValidators
                null,                                                   //ILookupNormalizer keyNormalizer
                null,                                                   //IdentityErrorDescriber errors
                null,                                                   //IServiceProvider services
                null);                                                  //ILogger<UserManager<TUser>> logger
        }

        public static Mock<SignInManager<ApplicationUser>> CreateSignInManagerMock(UserManager<ApplicationUser> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                userManager,                                            //UserManager<TUser> userManager
                Mock.Of<IHttpContextAccessor>(),                        //IHttpContextAccessor contextAccessor
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),//IUserClaimsPrincipalFactory<TUser> claimsFactory
                null,                                                   //IOptions<IdentityOptions> optionsAccessor
                null,                                                   //ILogger<SignInManager<TUser>> logger
                null,                                                   //IAuthenticationSchemeProvider schemes
                null);                                                  //IUserConfirmation<TUser> confirmation
        }
    }
}
