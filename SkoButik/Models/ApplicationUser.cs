using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using System.Net.Sockets;


namespace SkoButik.Models
{
    public class ApplicationUser : IdentityUser
    {

        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Phone]
        [StringLength(13, MinimumLength = 10)]
        public string Phone { get; set; }

        [StringLength (40, MinimumLength = 3)]
        public string Address { get; set; }

        [StringLength(6, MinimumLength = 5)]
        public string ZipCode { get; set; }

        [StringLength(25, MinimumLength = 5)]
        public string City { get; set; }
      

        public virtual ICollection<Order>? Orders { get; set; }


    }
}
