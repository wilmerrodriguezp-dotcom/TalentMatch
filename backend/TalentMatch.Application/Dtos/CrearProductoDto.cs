using System.ComponentModel.DataAnnotations;

namespace TalentMatch.Application.Dtos;

public class CrearProductoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
}
