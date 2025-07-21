using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<PacienteViewModel> Pacientes { get; set; }
        public DbSet<MotivoViewModel> Motivos { get; set; }
    }
}
