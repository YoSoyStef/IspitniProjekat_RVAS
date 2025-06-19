public class KorisnikTermin
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }

    public int TerminId { get; set; }
    public Termin? Termin { get; set; }
}
