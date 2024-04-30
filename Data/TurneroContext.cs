using Microsoft.EntityFrameworkCore;
using Turnero.Models;


namespace Data 

{
    public class TurneroContext: DbContext
    { 
        public TurneroContext(DbContextOptions<TurneroContext> options)
            : base(options){}

    
        public DbSet<Turno> Turnos {get;set;}
        public DbSet<Categoria> Categorias {get;set;}
        public DbSet<Asistente> Asistentes {get;set;}

        public DbSet<Modulo> Modulos {get;set;}

    }
}