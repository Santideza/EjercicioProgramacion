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

    [Required]
    [Range(1000, 1000000, ErrorMessage = "El monto debe estar entre 1000 y 1000000")]
    public decimal Monto { get; set; }

    [Required]
    [Range(1, 360, ErrorMessage = "Los meses deben estar entre 1 y 360")]
    public int Meses { get; set; }

    // Resultados
    public decimal Tem { get; set; }
    public decimal CuotaMensual { get; set; }
    public decimal Comision { get; set; }
    public decimal CostoTotal { get; set; }
}