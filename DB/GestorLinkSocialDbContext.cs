using DB.Modelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class GestorLinkSocialDbContext :DbContext
    {
        //Contructor sin parametros (defecto)
        public GestorLinkSocialDbContext()
        {
        }
        //Contructor con el contexto
        public GestorLinkSocialDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=BBDD-LinkSocial;User Id=postgres;Password=1234; SearchPath=public");
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Accesos> Accesos { get; set; }
    }
}
