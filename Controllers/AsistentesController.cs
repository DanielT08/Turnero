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
    
    public IActionResult Todos()
    {
        return View();
    }

    public IActionResult Pendientes()
    {
        return View();
    }
    public IActionResult Finalizados()
    {
        return View();
    }
    public async Task <IActionResult> Principal()
    {

        int cantidad = _context.Turnos.Count();
        ViewBag.Cantidad = cantidad;

        // var result = await _context.Turnos.ToListAsync();
        // return Json(cantidad);
        // ViewBag.Total = resultTurnos.Where(t => t.Estado.Equals("En Espera")).Count();
        // ViewBag.Pendientes = resultTurnos.Where(t => t.Estado.Equals("Pendientes")).Count();

        var result =  await _context.Turnos.ToListAsync();
        ViewBag.Categorias = result.Where(t => t.Estado.Equals("Pendiente"))
                                        .GroupBy(t => t.Categoria)
                                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                                        .ToList();



        return View();
    }
}


