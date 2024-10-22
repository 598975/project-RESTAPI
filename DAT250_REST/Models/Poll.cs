namespace DAT250_REST.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String? Id { get; set; }
        public String? Question { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime ValidUntil { get; set; }
        public List<VoteOption>? Options { get; set; } 
    }
}
