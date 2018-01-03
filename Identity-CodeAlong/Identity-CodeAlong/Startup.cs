using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Identity_CodeAlong.Startup))]
namespace Identity_CodeAlong
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
