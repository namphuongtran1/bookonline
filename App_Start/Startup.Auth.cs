using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace BookManage
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/SSO/Index"),
                SlidingExpiration = true
            });
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()//hàm cung cấp id xác thực ứng dụng trên google
            {
                ClientId = "487350180858-4fb27ir2vm89mupi3n62jnpb9ftetuee.apps.googleusercontent.com",
                ClientSecret = "U2Da6DfzzaMO7-AGqRtBV8Pu",
                CallbackPath = new PathString("/GoogleLoginCallback")
            });
        }
    }
}