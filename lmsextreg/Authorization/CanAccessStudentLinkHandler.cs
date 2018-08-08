using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using lmsextreg.Data;
using lmsextreg.Constants;

namespace lmsextreg.Authorization
{
    public class CanAccessStudentLinkHandler : AuthorizationHandler<CanAccessStudentLink>
    {
        UserManager<ApplicationUser> _userManager;

        public CanAccessStudentLinkHandler(UserManager<ApplicationUser> userMgr)
        {
            _userManager = userMgr;

            Console.WriteLine("[CanAccessStudentLinkHandler][Constructor] - (_userManager == null): " 
                                + (_userManager == null));
        }        

        protected override Task HandleRequirementAsync
        (
            AuthorizationHandlerContext authContext,
            CanAccessStudentLink requirement
        )
        {
           // Console.WriteLine("[][] - (): " + );

            string userId = _userManager.GetUserId(authContext.User);
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
                                + "(ApplicationUser.Id): " 
                                + userId
                            );

            ApplicationUser appUser = GetUserAsync(userId).Result;
        
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (ApplicationUser == null): " 
                                + (appUser == null));            
            
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
                                + "(ApplicationUser.TwoFactorEnabled): " 
                                + appUser.TwoFactorEnabled
                            );

           Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
                                + "(authContext.User.IsInRole(\"STUDENT\"): " 
                                + authContext.User.IsInRole("STUDENT")
                            );

            if ( appUser.TwoFactorEnabled )
            {
                authContext.Succeed(requirement);
            }
            

            // if (authContext.User.IsInRole("STUDENT"))
            // {
            //     authContext.Succeed(requirement);
            // }

            return Task.FromResult(0);
        }

        private  Task<ApplicationUser> GetUserAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }
    }
}