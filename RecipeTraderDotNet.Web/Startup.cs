using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipeTraderDotNet.Web.Startup))]
namespace RecipeTraderDotNet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
