using RezervacijaSportskihTermina.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Termin
{
    public int Id { get; set; }

    [Required]
    public DateTime DatumVreme { get; set; }

    [Range(0, 10000)]
    public decimal Cena { get; set; }

    public int SportId { get; set; }
    public Sport? Sport { get; set; }

    public int SportskiCentarId { get; set; }
    public SportskiCentar? SportskiCentar { get; set; }

    public ICollection<KorisnikTermin> Korisnici { get; set; } = new List<KorisnikTermin>();
}

