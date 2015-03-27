using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProyectoInventarioOET.Startup))]
namespace ProyectoInventarioOET
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
