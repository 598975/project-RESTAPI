using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAT250_REST.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public DateTime PublishedAt {  get; set; }
        public Poll? Poll { get; set; }
        public User? User { get; set; }


    }
}
