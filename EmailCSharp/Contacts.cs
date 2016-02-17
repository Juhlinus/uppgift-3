using System;
using System.Collections;
using System.Collections.Generic;

namespace EmailCSharp
{
	[Serializable]
	public class Contacts : IEnumerable<Contact>
	{
		private List<Contact> Contact;

		public Contacts() 
		{ 
			Contact = new List<Contact>(); 
		}

		public void Add(object contact) 
		{ 
			Contact.Add(contact as Contact); 
		}

		public void Remove(int index) 
		{ 
			Contact.Remove(Contact[index]); 
		}

        public int Count()
        {
            return Contact.Count;
        }

        public Contact Get(int index)
		{
			return Contact[index];
		}

		public int returnContactKey(Contacts contacts)
		{
			int i = 1;

			foreach (var contact in contacts)
				Console.WriteLine(i++ + ": " + contact.Name);

			char _key = Console.ReadKey(true).KeyChar;

			if (_key == '0')
				return -1;

			int key = Convert.ToInt32(Char.GetNumericValue(_key));

			if (key > i)
				return -1;

			return --key;
		}

		IEnumerator<Contact> IEnumerable<Contact>.GetEnumerator()
		{
			return this.Contact.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Contact.GetEnumerator();
		}
	}
}

