using System;
using System.Collections.Generic;

namespace BlackBarberAPI.Models;

public partial class ServicioCitum
{
    public int Id { get; set; }

    public int? IdCita { get; set; }

    public int? IdServicio { get; set; }

    public decimal Precio { get; set; }

    public int? IdBarbero { get; set; }

    public virtual ICollection<DetalleCitum> DetalleCita { get; set; } = new List<DetalleCitum>();

    public virtual Citum? IdCitaNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
    public virtual Barbero? IdBarberoNavigation { get; set; }
}
