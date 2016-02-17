using System;

namespace EmailCSharp
{
	[Serializable]
	public class BaseMessage
	{
		public string Time { get; set; }
		public string Sender { get; set; }
		public string Receiver { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
        public bool Read { get; set; }
	}
}