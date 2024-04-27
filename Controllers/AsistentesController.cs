using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CSharp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Turnero.Models;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Linq;
using Data;

namespace CSharp.Controllers;

public class AsistentesController : Controller
{

    //Angelica
    public readonly TurneroContext _context;

    public AsistentesController(TurneroContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string correo, string password)
    {
        var asistente = await _context.Asistentes.FirstOrDefaultAsync(u => u.Correo == correo);
        if (asistente != null && asistente.Password == password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, asistente.Correo),
            };

            var asistenteIdentity = new ClaimsIdentity(claims, "login");

            var main = new ClaimsPrincipal(asistenteIdentity);
            HttpContext.Response.Cookies.Append("Asistente_Id", asistente.Id.ToString());
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, main);
            return RedirectToAction("Principal", "Asistentes");
        }
        ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
        return View("Index");
    }
    
    public async Task<IActionResult> Todos(){
        return View(await _context.Turnos.ToListAsync());
    }

    public async Task<IActionResult> Pendientes(){
        return View(await _context.Turnos.ToListAsync());
    }
    


   public async Task<IActionResult> Finalizados(){
            return View(await _context.Turnos.ToListAsync());
    }

    public IActionResult Principal()
    {
        /* var resultTurnos = await _context.Turnos.ToListAsync();
        ViewBag.Total = resultTurnos.Where(t => t.Estado.Equals("En Espera")).Count();  */
        //ViewBag.Pendientes = resultTurnos.Where(t => t.Estado.Equals("Pendientes")).Count();

       /*  ViewBag.Categorias = resultTurnos.Where(t => t.Estado.Equals("En Espera")).Select(c => new {c.Categoria}).GroupBy(e=> e.Categoria).ToList(); */

        return View();
    }
}


