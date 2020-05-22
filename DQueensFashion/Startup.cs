using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DQueensFashion.Startup))]
namespace DQueensFashion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
