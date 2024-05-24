using Microsoft.EntityFrameworkCore;

namespace BD_MySQL
{
    public class MySqlDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {

            optionBuilder.UseMySQL("server=10.100.1.25;database=race_manager;uid=root;pwd=");
        }
    }
}
