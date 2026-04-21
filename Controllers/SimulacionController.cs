using Microsoft.AspNetCore.Mvc;
using EjercicioProgramacion.Models;
using System.Collections.Generic;
using System.Linq;

namespace EjercicioProgramacion.Controllers;

public class SimulacionController : Controller
{
    private readonly List<ProductosCredito> _productos = new List<ProductosCredito>
    {
        new ProductosCredito { Id = 1, Nombre = "Crédito Personal", Tea = 25.5m, ComisionPct = 2.0m, MinMeses = 6, MaxMeses = 60 },
        new ProductosCredito { Id = 2, Nombre = "Crédito Hipotecario", Tea = 8.5m, ComisionPct = 1.5m, MinMeses = 120, MaxMeses = 360 },
        new ProductosCredito { Id = 3, Nombre = "Crédito Vehicular", Tea = 15.0m, ComisionPct = 3.0m, MinMeses = 12, MaxMeses = 84 }
    };

    [HttpGet]
    public IActionResult Simulador(int productId)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == productId);
        if (producto == null)
        {
            return NotFound();
        }

        var model = new SimulacionViewModel
        {
            ProductId = producto.Id,
            NombreProducto = producto.Nombre,
            Tea = producto.Tea,
            ComisionPct = producto.ComisionPct,
            MinMeses = producto.MinMeses,
            MaxMeses = producto.MaxMeses
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Simulador(SimulacionViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Recargar datos del producto
            var producto = _productos.FirstOrDefault(p => p.Id == model.ProductId);
            if (producto != null)
            {
                model.NombreProducto = producto.Nombre;
                model.Tea = producto.Tea;
                model.ComisionPct = producto.ComisionPct;
                model.MinMeses = producto.MinMeses;
                model.MaxMeses = producto.MaxMeses;
            }
            return View(model);
        }

        // Calcular TEM
        model.Tem = (decimal)(Math.Pow((double)(1 + model.Tea / 100), 1.0 / 12) - 1) * 100;

        // Calcular cuota mensual usando fórmula de amortización
        decimal temDecimal = model.Tem / 100;
        decimal factor = (decimal)Math.Pow((double)(1 + temDecimal), model.Meses);
        model.CuotaMensual = model.Monto * (temDecimal * factor) / (factor - 1);

        // Comisión
        model.Comision = model.Monto * model.ComisionPct / 100;

        // Costo total
        model.CostoTotal = model.CuotaMensual * model.Meses + model.Comision;

        return View(model);
    }
}