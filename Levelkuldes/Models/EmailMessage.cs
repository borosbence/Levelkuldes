using System.Collections.Generic;

namespace Levelkuldes.Models
{
    class EmailMessage
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public List<string> ToAddresses = new List<string>();
        public string HTMLBody { get; set; }
    }
}
