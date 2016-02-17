using System;

namespace EmailCSharp
{
	[Serializable]
	public class Contact
	{
		public string Name { get; set; }
        public bool Server { get; set; }// = false;
	}
}