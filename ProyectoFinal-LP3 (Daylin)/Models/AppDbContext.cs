﻿using Microsoft.EntityFrameworkCore;
using ProyectoFinal_LP3__Daylin_.Controllers;

namespace ProyectoFinal_LP3__Daylin_.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<PacienteViewModel> Pacientes { get; set; }
        public DbSet<MotivoViewModel> Motivos { get; set; }
        public DbSet<DentistaViewModel> Dentistas { get; set; }
        public DbSet<CitaViewModel> Citas { get; set; }
    }
}
