using Microsoft.AspNetCore.Mvc;
using EjercicioProgramacion.Models;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace EjercicioProgramacion.Controllers;

public class SimulacionController : Controller
{
    private readonly List<ProductosCredito> _productos = new List<ProductosCredito>
    {
        new ProductosCredito { Id = 1, Nombre = "Crédito Personal", Tea = 25.5m, ComisionPct = 2.0m, MinMeses = 6, MaxMeses = 60 },
        new ProductosCredito { Id = 2, Nombre = "Crédito Hipotecario", Tea = 8.5m, ComisionPct = 1.5m, MinMeses = 120, MaxMeses = 360 },
        new ProductosCredito { Id = 3, Nombre = "Crédito Vehicular", Tea = 15.0m, ComisionPct = 3.0m, MinMeses = 12, MaxMeses = 84 }
    };

    public SimulacionController()
    {
        // Configurar cultura por defecto a es-PE
        var culturePE = new CultureInfo("es-PE");
        CultureInfo.DefaultThreadCurrentCulture = culturePE;
        CultureInfo.DefaultThreadCurrentUICulture = culturePE;
    }

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
    [ValidateAntiForgeryToken]
    public IActionResult Simulador(SimulacionViewModel model)
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

        // Validación adicional: verificar que los meses están dentro del rango del producto
        if (model.Meses < model.MinMeses || model.Meses > model.MaxMeses)
        {
            model.ErrorMeses = $"Los meses deben estar entre {model.MinMeses} y {model.MaxMeses}";
            ModelState.AddModelError(nameof(model.Meses), model.ErrorMeses);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Calcular TEM: TEM = (1 + TEA)^(1/12) - 1
        // TEA está en porcentaje (ej: 25.5), convertir a decimal
        decimal teaDecimal = model.Tea / 100;
        decimal temDecimal = (decimal)Math.Pow((double)(1 + teaDecimal), 1.0 / 12) - 1;
        // Redondear TEM a 6 decimales para cálculos
        temDecimal = Math.Round(temDecimal, 6, MidpointRounding.AwayFromZero);
        // Convertir a porcentaje para mostrar
        model.Tem = temDecimal * 100;

        // Calcular cuota mensual usando fórmula de amortización (sistema francés)
        // Cuota = P * [ i * (1+i)^n ] / [ (1+i)^n - 1 ]
        decimal factor = (decimal)Math.Pow((double)(1 + temDecimal), model.Meses);
        decimal cuotaCalculada = model.Monto * (temDecimal * factor) / (factor - 1);
        model.CuotaMensual = Math.Round(cuotaCalculada, 2, MidpointRounding.AwayFromZero);

        // Comisión: %Comisión * monto
        decimal comisionCalculada = model.Monto * model.ComisionPct / 100;
        model.Comision = Math.Round(comisionCalculada, 2, MidpointRounding.AwayFromZero);

        // Costo total: Cuota * n + Comisión
        decimal costoTotalCalculado = model.CuotaMensual * model.Meses + model.Comision;
        model.CostoTotal = Math.Round(costoTotalCalculado, 2, MidpointRounding.AwayFromZero);

        return View(model);
    }
}