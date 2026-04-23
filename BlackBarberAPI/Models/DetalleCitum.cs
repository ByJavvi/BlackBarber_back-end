using System;
using System.Collections.Generic;

namespace BlackBarberAPI.Models;

public partial class DetalleCitum
{
    public int Id { get; set; }

    public int? IdServicioCita { get; set; }

    public decimal Precio { get; set; }
    public int IdAnadidoServicion { get; set; }

    public virtual ServicioCitum? IdServicioCitaNavigation { get; set; }
    public virtual AnadidoServicio? IdAnadidoServicionNavigation { get; set; }
}
