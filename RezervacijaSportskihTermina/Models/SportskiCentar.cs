using RezervacijaSportskihTermina.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class SportskiCentar
{
    public int Id { get; set; }

    [Required]
    public string Naziv { get; set; } = string.Empty;

    public string? Lokacija { get; set; }

    public ICollection<Termin> Termini { get; set; } = new List<Termin>();
}

