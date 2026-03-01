using BimManagerPortal.Domain.Entities.AppTypes;

namespace BimManagerPortal.Domain.Entities.Plugins
{
    public class PluginEntity
    {
        public PluginEntity(string name, AppType app, string company) 
        {
            Name = name;
            App = app;
            Company = company;
        }
        public string Name { get; set; }
        public AppType App { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
    }
}
