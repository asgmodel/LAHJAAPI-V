//using Api.LAHJAAPI.Utilities;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;

//namespace Api;

//public class MyClaimsTransformation(
//    ClaimsChange claimsChange,
//    UserManager<ApplicationUser> userManager,
//    TrackSubscription trackSubscription,
//    IUserClaims userClaims,
//    IUserClaimRepository userClaimRepository,
//    IPlanRepository planRepository) : IClaimsTransformation
//{
//    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
//    {
//        //ClaimsIdentity claimsIdentity = new ClaimsIdentity();
//        if (principal.Identity != null && principal.Identity.IsAuthenticated)
//        {
//            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (trackSubscription.UserId != userId) trackSubscription.RefreshData();
//            trackSubscription.UserId = userId!;
//            await trackSubscription.GetSubscriptionsAsync();
//            userClaims.CustomerId = trackSubscription.CustomerId;

//            // var user = await userManager.Users.Include(u => u.Subscription)
//            //.FirstOrDefaultAsync(u => u.Id == userId);
//            //userClaims.CustomerId = user!.CustomerId;
//            //claimsChange.IsChange = false;
//        }

//        //if (!principal.HasClaim(c => c.Type == ClaimTypes2.CustomerId))
//        //{
//        //    var claim = await userClaimRepository.GetByAsync(c => c.UserId == userId && c.ClaimType == ClaimTypes2.CustomerId);

//        //    if (claim != null)
//        //    {
//        //        //trackSubscription.CurrentNumberRequests = claim.ClaimValue.ToInt64();
//        //        claimsIdentity.AddClaim(claim.ToClaim());
//        //    }
//        //}


//        //principal.AddIdentity(claimsIdentity);
//        return principal;
//    }
//}
