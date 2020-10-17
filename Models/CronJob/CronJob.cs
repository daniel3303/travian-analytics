using System;

namespace TravianAnalytics.Models.CronJob {
    public class CronJob {
        public int Id { get; set; }
        public string Service { get; set; }
        public DateTime ExecutionTime { get; set; } = DateTime.Now;
        public bool Success { get; set; }
        public string Output { get; set; }
    }
}