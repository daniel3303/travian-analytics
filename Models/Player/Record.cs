using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravianAnalytics.Models.Player {
    [Index(nameof(PlayerId), nameof(Time), IsUnique = true)]
    [Table("Records")]
    public class Record {
        public int Id { get; set; }
        
        [Required]
        public virtual Player Player { get; set; }

        private int PlayerId { get; set; }

        public bool Online { get; set; }
        public int Villages { get; set; }
        public int Population { get; set; }
        
        public DateTime Time { get; set; }
    }
}