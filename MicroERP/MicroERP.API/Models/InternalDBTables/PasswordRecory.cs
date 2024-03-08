namespace MicroERP.API.Models.InternalDBTables
{
    public class PasswordRecory
    {
        public int Id { get;set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CodeRecovery { get; set; }

        public DateTime Expiration { get; set; }
    }
}
