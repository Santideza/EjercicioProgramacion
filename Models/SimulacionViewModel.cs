using System.ComponentModel.DataAnnotations;

namespace EjercicioProgramacion.Models;

public class SimulacionViewModel
{
    public int ProductId { get; set; }
    public string NombreProducto { get; set; }
    public decimal Tea { get; set; }
    public decimal ComisionPct { get; set; }
    public int MinMeses { get; set; }
    public int MaxMeses { get; set; }

    [Required(ErrorMessage = "El monto es requerido")]
    [Range(0.01, 1_000_000, ErrorMessage = "El monto debe estar entre 0.01 y 1,000,000")]
    public decimal Monto { get; set; }

    [Required(ErrorMessage = "Los meses son requeridos")]
    [Range(1, 600, ErrorMessage = "Los meses deben estar entre 1 y 600")]
    public int Meses { get; set; }

    public string ErrorMeses { get; set; }

    // Resultados
    public decimal Tem { get; set; }
    public decimal CuotaMensual { get; set; }
    public decimal Comision { get; set; }
    public decimal CostoTotal { get; set; }
}