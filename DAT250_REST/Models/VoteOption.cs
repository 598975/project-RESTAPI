using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAT250_REST.Models
{
    public class VoteOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String? Caption {  get; set; }
        public int PresentationOrder { get; set; }
        
    }
}