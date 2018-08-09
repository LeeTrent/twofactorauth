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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CanAccessStudentLinkHandler(SignInManager<ApplicationUser> signInMgr)
        {
            _signInManager = signInMgr;

            Console.WriteLine("[CanAccessStudentLinkHandler][Constructor] - (_signInManager == null): " 
                                + (_signInManager == null));

            Console.WriteLine("[CanAccessStudentLinkHandler][Constructor] - (_signInManager.UserManager == null): " 
                                + (_signInManager.UserManager == null));                                
        }        

        // protected override Task HandleRequirementAsync (AuthorizationHandlerContext authContext,
        //                                                 CanAccessStudentLink requirement)
        // {
        // //     Console.WriteLine("[][] - (): " + );
        //     Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
        //                         + "(_signInManager.IsSignedIn(authContext.User): " 
        //                         + _signInManager.IsSignedIn(authContext.User)
        //                     );
            
        //     string userId = _signInManager.UserManager.GetUserId(authContext.User);
        //     Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
        //                         + "(ApplicationUser.Id): " 
        //                         + userId
        //                     );
        //     ApplicationUser appUser = GetUserAsync(userId).Result;
        //     Console.WriteLine   ("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (ApplicationUser == null): " 
        //                             + (appUser == null)
        //                         );                      
        //     Console.WriteLine   ("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
        //                             + "(ApplicationUser.TwoFactorEnabled): " 
        //                             + appUser.TwoFactorEnabled
        //                         );
        //    Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - "
        //                         + "(authContext.User.IsInRole(\"STUDENT\"): " 
        //                         + authContext.User.IsInRole("STUDENT")
        //                   );

        //     //string          userId              = _signInManager.UserManager.GetUserId(authContext.User);
        //     //ApplicationUser appUser             = GetUserAsync(userId).Result;
        //     bool            isSignedIn          = _signInManager.IsSignedIn(authContext.User);
        //     bool            isTwoFactorEnabled  = appUser.TwoFactorEnabled;
        //     bool            isStudent           = authContext.User.IsInRole("STUDENT");

        //     Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (User is signed-in)....: " + isSignedIn);
        //     Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (Two-Factor is enabled): " + isTwoFactorEnabled);
        //     Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (User is a Student)....: " + isStudent);

        //     if ( isSignedIn 
        //             && isTwoFactorEnabled
        //             && isStudent )
        //     {
        //         authContext.Succeed(requirement);
        //     }

        //     return Task.CompletedTask;
        // }

        protected override Task HandleRequirementAsync (AuthorizationHandlerContext authContext,
                                                        CanAccessStudentLink requirement)
        {
            bool isSignedIn         = false;
            bool isTwoFactorEnabled = false;
            bool isStudent          = false;

            isSignedIn = _signInManager.IsSignedIn(authContext.User);
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (User is signed-in): " + isSignedIn);

            if ( isSignedIn )
            {
                string appUserId = _signInManager.UserManager.GetUserId(authContext.User);
                Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (ApplicationUser.Id): " + appUserId);  

                if ( String.IsNullOrEmpty(appUserId) == false )
                {
                    ApplicationUser appUser = GetUserAsync(appUserId).Result;
                    Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (ApplicationUser == null): "  + (appUser == null) );

                    if ( appUser != null) 
                    {
                        isTwoFactorEnabled = appUser.TwoFactorEnabled;
                    }

                    if ( authContext != null && authContext.User != null)
                    {
                        isStudent = authContext.User.IsInRole("STUDENT");
                    }
                }
            }
       
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (isSignedInn)........: " + isSignedIn);
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (isTwoFactorEnabled).: " + isTwoFactorEnabled);
            Console.WriteLine("[CanAccessStudentLinkHandler][HandleRequirementAsync] - (isStudent)..........: " + isStudent);

            if ( isSignedIn 
                    && isTwoFactorEnabled 
                    && isStudent )
            {
                authContext.Succeed(requirement);
            }            

            return Task.CompletedTask;
        }

        private  Task<ApplicationUser> GetUserAsync(string userId)
        {
            return _signInManager.UserManager.FindByIdAsync(userId);
        }
    }
}