using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BronhomunalEvolved
{
	public abstract class Client
	{
		public void Start()
		{
			Thread thread = new Thread(Run);
			thread.Start();
		}

		public virtual void Run()
		{

		}
	}
}
