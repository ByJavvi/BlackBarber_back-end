using System;
using System.Collections.Generic;

namespace BlackBarberAPI.Models;

public partial class Barbero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdUsuario { get; set; }

    public int? Estatus { get; set; }

    public virtual ICollection<BarberoServicio> BarberoServicios { get; set; } = new List<BarberoServicio>();
    public virtual ICollection<ServicioCitum> ServiciosCitas { get; set; } = new List<ServicioCitum>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
