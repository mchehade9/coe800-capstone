using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestingCoe800.Startup))]
namespace TestingCoe800
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
