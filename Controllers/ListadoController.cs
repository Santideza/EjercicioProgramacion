using Microsoft.AspNetCore.Mvc;
using EjercicioProgramacion.Models;
using System.Collections.Generic;

namespace EjercicioProgramacion.Controllers;

public class ListadoController : Controller
{
    public IActionResult Productos()
    {
        // Datos de ejemplo
        var productos = new List<ProductosCredito>
        {
            new ProductosCredito { Id = 1, Nombre = "Crédito Personal", Tea = 25.5m, ComisionPct = 2.0m, MinMeses = 6, MaxMeses = 60 },
            new ProductosCredito { Id = 2, Nombre = "Crédito Hipotecario", Tea = 8.5m, ComisionPct = 1.5m, MinMeses = 120, MaxMeses = 360 },
            new ProductosCredito { Id = 3, Nombre = "Crédito Vehicular", Tea = 15.0m, ComisionPct = 3.0m, MinMeses = 12, MaxMeses = 84 }
        };

        return View(productos);
    }
}