using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TravianAnalytics.Dtos {
    public class ReportDto {
        [Required]
        public AllianceDto Alliance { get; set; }

        [Required]
        public List<PlayerDto> Players { get; set; }
    }
}