using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocuPath.Startup))]
namespace DocuPath
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
