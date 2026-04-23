using System;
using System.Collections.Generic;

namespace BlackBarberAPI.Models;

public partial class AnadidoServicio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int? IdPerteneciente { get; set; }

    public decimal Precio { get; set; }

    public virtual Servicio? IdPertenecienteNavigation { get; set; }
    public virtual ICollection<DetalleCitum> DetalleCitas { get; set; } = new List<DetalleCitum>();
}
