using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Root.Models
{
	public class Card
	{
		public string Id { get; set; }
		public string GroupId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string Type { get; set; }
		public string Difficulty { get; set; }
		public string Question1 { get; set; }
		public string Question2 { get; set; }
		public string Question3 { get; set; }
		public string Answer1 { get; set; }
		public string Answer2 { get; set; }
		public string Answer3 { get; set; }
		public string Hint1 { get; set; }
		public string Hint2 { get; set; }
		public string Hint3 { get; set; }
		public string AnsweredCorrectCount1 { get; set; }
		public string AnsweredCorrectCount2 { get; set; }
		public string AnsweredCorrectCount3 { get; set; }
	}
}
