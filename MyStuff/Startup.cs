using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyStuff.Startup))]
namespace MyStuff
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
