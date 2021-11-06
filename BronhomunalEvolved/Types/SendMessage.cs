using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved.Types
{
	public  class SendMessage
	{
		public String? Text { get; set; }
		public List<Base64File>? Photos { get; set; }
	}
}
