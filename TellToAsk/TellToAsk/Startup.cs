using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TellToAsk.Startup))]
namespace TellToAsk
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
