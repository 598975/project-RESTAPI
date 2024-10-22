using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DAT250_REST.Models
{

    public class User : IdentityUser
    {

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int? Id { get; set; }
        //public String? Username { get; set; }
        //public String? Email { get; set; }
        //public String? Password_hash { get; set; }
        public List<Poll>? PollsCreated { get; set; }
        public List<Vote>? Votes {  get; set; }
    }
}
