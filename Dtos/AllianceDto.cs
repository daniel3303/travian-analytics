using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TravianAnalytics.Dtos {
    public class AllianceDto {
        public int TravianId { get; set; }
        
        public string Name { get; set; }
    }
}