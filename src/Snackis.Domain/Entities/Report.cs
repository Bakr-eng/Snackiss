using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string ReporterId { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsHandled { get; set; }
    }
}
