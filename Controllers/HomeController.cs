using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CSharp.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using Turnero.Models;

namespace CSharp.Controllers;

public class HomeController : Controller
{

//variable de conexion
    private readonly TurneroContext _context;
//inicializar conexion
    public HomeController(TurneroContext context){
        _context = context;
    }


    //Angelica
    public IActionResult Index()
    {
        return View();
    }

    //Angelica
    public async Task<IActionResult> Categorias(string tipo, string documento)
    {
        var Tipo=tipo;
        ViewData["tipo"]=Tipo;
        var Documento=documento;
        ViewData["documento"]=Documento;
        Response.Cookies.Append("tipo",tipo);
        Response.Cookies.Append("documento",documento);
        return View(await _context.Categorias.ToListAsync());
    }


    //David 
    public async Task<IActionResult> CrearTurno(string siglas)
    {
        string seleccion = "";
        string turno = "";
        var result = await _context.Categorias.FirstOrDefaultAsync(c => c.Siglas == siglas);
        int contador = result.Contador +1;

        turno = siglas+"-"+(contador < 10 ? "00"+contador: "0"+contador);

        ViewData["turno"] = turno;
        var tipo = HttpContext.Request.Cookies["tipo"];
        ViewData["tipo"]=tipo;
        var documento = HttpContext.Request.Cookies["documento"];
        ViewData["documento"]=documento;
        var Fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
        ViewData["fecha"]=Fecha;
        var Hora = DateTime.Now.ToString("HH:mm:ss");
        ViewData["hora"]=Hora;
        return View();
    }

    //Daniel
    public IActionResult Turnos()
    {
        return View();
    } 
     public IActionResult Pausar()
    {
        return View();
    }
    //Angelica 
    [HttpPost]
    public async Task<IActionResult> Asignar(Turno turno)
    {
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
