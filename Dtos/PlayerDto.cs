using Newtonsoft.Json;

namespace TravianAnalytics.Dtos {
    public class PlayerDto {
        public int TravianId { get; set; }
        
        public string Name { get; set; }
        
        public bool Online { get; set; }
        
        public int Population { get; set; }
        
        public int Villages { get; set; }
    }
}