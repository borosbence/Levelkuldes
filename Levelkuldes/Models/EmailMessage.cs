using System.Collections.Generic;

namespace Levelkuldes.Models
{
    class EmailMessage
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public List<string> ToAddresses { get; set; }
        public string HTMLBody { get; set; }
    }
}
