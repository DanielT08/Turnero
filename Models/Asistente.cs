using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turnero.Models
{
    public class Asistente
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string?  Password { get; set; }
        public DateTime? FechaRegistro {get; set;}
    }    
    
}