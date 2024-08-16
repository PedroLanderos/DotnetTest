using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ALSETDotnetTest.Models
{
    public class Journal
    {
        [Key]
        public int JournalId { get; set; }

        [ForeignKey("Researcher")]
        public int ResearcherId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string FileName { get; set; }

        //references the pad direction from the server
        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string FilePath { get; set; } //the files will be into the server and will have unique path to identify them

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        //references the researcher who has uploaded the file (a different table)
        public Researcher? Researcher { get; set; }
    }
}
