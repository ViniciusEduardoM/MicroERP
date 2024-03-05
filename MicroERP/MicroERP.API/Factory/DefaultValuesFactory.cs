using MicroERP.API.Data;
using MicroERP.ModelsDB.Models.MasterData.Users;

namespace MicroERP.API.Factory
{
    public class DefaultValuesFactory
    {
        private readonly DataContext _context;

        public DefaultValuesFactory(DataContext context) 
        {
            _context = context;

            CreateDefaultRole();
        }  

        private void CreateDefaultRole()
        {
            if (!_context.Roles.Any())
                _context.Roles.Add(new Role { Name = "User" });

            _context.SaveChanges();
        }
    }
}
