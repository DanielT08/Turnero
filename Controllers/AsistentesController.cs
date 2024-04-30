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
using System.Runtime.CompilerServices;

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

    public async Task<IActionResult> Pendientes(string modulo, string categoria){

        ViewBag.Modulo = modulo;

        ViewBag.Pendientes = await _context.Turnos.Where(w => w.Categoria == categoria).ToListAsync();
        return View();
    }
    


   public async Task<IActionResult> Finalizados(){
            return View(await _context.Turnos.ToListAsync());
    }

    public IActionResult Principal()
    {

        int cantidad = _context.Turnos.Count();
        ViewBag.Cantidad = cantidad;

        // var result = await _context.Turnos.ToListAsync();
        // return Json(cantidad);
        // ViewBag.Total = resultTurnos.Where(t => t.Estado.Equals("En Espera")).Count();
        // ViewBag.Pendientes = resultTurnos.Where(t => t.Estado.Equals("Pendientes")).Count();

        var result =   _context.Turnos.ToList();
        ViewBag.Categorias = result.Where(t => t.Estado.Equals("Pendiente"))
                                        .GroupBy(t => t.Categoria)
                                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                                        .ToList();



        return View();
    }

    public async Task<IActionResult> Modulos(){

        ViewBag.Modulos = await _context.Modulos.Where(x => x.Estado.Equals("Habilitado")).ToListAsync();
        ViewBag.Categoria = await _context.Categorias.ToListAsync();
        /* return Json(ViewBag.Modulos); */

        return View();
    }

    public async Task<IActionResult> CambiarEstado(string turno, string modulo){
       var result  = await _context.Turnos.FirstOrDefaultAsync(t => t.Ficho == turno);

        result.Modulo = modulo;
        result.FechaAtendido = DateTime.Now;
        result.Estado ="Atendiendo";
        _context.Turnos.Update(result);
        _context.SaveChanges();

        HttpContext.Response.Cookies.Append("IdTurno", result.Ficho);
        /* return Json(ViewBag.Modulos); */

        return RedirectToAction("Atencion");
    }

    public async Task<IActionResult> Atencion(){
        
        string turno = HttpContext.Request.Cookies["IdTurno"];
        ViewBag.Turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Ficho == turno);
        /* return Json(ViewBag.Modulos); */

        return View();
    }

     public async Task<IActionResult> Finalizar(int id){
       var result  = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id);

        result.Estado = "Finalizado";
        result.FechaSalida = DateTime.Now;
        _context.Turnos.Update(result);
        _context.SaveChanges();

        return RedirectToAction("Pendientes");
    }

    
}


