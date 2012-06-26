using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Commands
{
    public class CommentDeletedEvent
    {
        public string Id { get; set; }
        public string Message
        {
            get { return String.Format("Deleted comment:{0}", this.Id); }
        }
    }
}
