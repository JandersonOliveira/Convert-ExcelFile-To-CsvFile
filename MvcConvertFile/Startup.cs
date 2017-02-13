using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcConvertFile.Startup))]
namespace MvcConvertFile
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
