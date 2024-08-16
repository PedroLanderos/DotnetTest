using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALSETDotnetTest.Models
{
    public class Researcher
    {
        [Key]
        public int ResearcherId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Name { get; set; }

        public DateTime JoinedDate { get; set; }

        public int TotalJournalsUploaded { get; set; }

        //Foreing Keys and navegation
        public ICollection<Journal> Journals { get; set; } = new List<Journal>();//references the published journals of the autor/researcher
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>(); //references the suscriptions of the autoer/researcher to other or other researchers
    }                                                                //it could be 1:N, N:1, N:M, M;N
}
