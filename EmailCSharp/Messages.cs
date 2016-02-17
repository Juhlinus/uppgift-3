using System;
using System.Collections;
using System.Collections.Generic;

namespace EmailCSharp
{
	[Serializable]
	public class Messages : IEnumerable<BaseMessage>
	{
		private List<BaseMessage> _messages;

		public void Add(BaseMessage message)
		{
            if (_messages == null)
                _messages = new List<BaseMessage>();
            
		    _messages.Add(message);
		}

        public BaseMessage Get(int i)
        {
            return _messages[i];
        }

        public int Count()
        {
            return _messages.Count;
        }

        IEnumerator<BaseMessage> IEnumerable<BaseMessage>.GetEnumerator()
		{
			return _messages.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _messages.GetEnumerator();
		}
	}
}

