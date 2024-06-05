using System.ComponentModel.DataAnnotations;

namespace SkoButik.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

