using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentType { get; set; }
		public int ObjectId { get; set; }
        public int ByAccountId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
    }
}
