using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravianAnalytics.Models {
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(TravianId), IsUnique = true)]
    [Table("Alliances")]
    public class Alliance {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TravianId { get; set; }

        public DateTime LastUpdate { get; set; }

        public virtual List<Player.Player> Players { get; set; } = new List<Player.Player>();
    }
}