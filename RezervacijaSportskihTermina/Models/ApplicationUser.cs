using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    public ICollection<KorisnikTermin> Termini { get; set; } = new List<KorisnikTermin>();
}

