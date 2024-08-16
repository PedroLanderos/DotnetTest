using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALSETDotnetTest.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        [ForeignKey("Subscriber")]
        public int SubscriberId { get; set; }

        [ForeignKey("SubscribedTo")]
        public int SubscribedToId { get; set; }

        //references the foreing keys to validate the suscriptions 
        public Researcher Subscriber { get; set; } //references the current researcher who wants to suscribe
        public Researcher SubscribedTo { get; set; }//references the researcher to suscribe
    }
}
