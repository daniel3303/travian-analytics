using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravianAnalytics.Models.Player {
    [Index(nameof(TravianId), IsUnique = true)]
    [Table("Players")]
    public class Player {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TravianId { get; set; }

        public virtual Alliance Alliance { get; set; }

        public virtual List<Record> Records { get; set; } = new List<Record>();
    }
}