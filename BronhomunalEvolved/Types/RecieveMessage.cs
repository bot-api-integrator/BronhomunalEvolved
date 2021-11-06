using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Types
{
	public class RecieveMessage
	{
		public String? Text { get; set; }
		public List<Base64File>? Photos { get; set; }
		public String? AuthorId { get; set; }
		public Int64? SendTime { get; set; }
		public String? IntegratorTopic { get; set; }

		public RecieveMessage() { }
	}
}
