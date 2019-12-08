using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmoteLog.Models
{
    public class LogEntry
    {
        [Key]
        public Guid LogId { get; set; }

        public string UserId { get; set; }

        public DateTime PublishDate { get; }

        [Required(ErrorMessage = "Tell me how you're doing!")]
        public int Mood { get; set; }

        public string Entry { get; set; }
    }
}
