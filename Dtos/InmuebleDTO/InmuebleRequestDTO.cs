namespace NetKubernetes.Dtos.InmuebleDTO;

public class InmuebleRequestDTO
{
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public decimal Precio { get; set; }
    public string? Picture { get; set; }
}