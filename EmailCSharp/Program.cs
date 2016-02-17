using System;
using System.IO;

namespace EmailCSharp
{
	class Program
	{
		static Settings settings = new Settings();
		static Contacts contacts = new Contacts();
		static Messages messages = new Messages();

		static bool running = true;
        static int msgCount = 0;

		public static void Main (string[] args)
		{
			string[] files = { "settings.json", "contacts.xml", "messages.bin" };

			settings = JsonSerialization.ReadFromJsonFile<Settings>(files[0]);
			contacts = XmlSerialization.ReadFromXmlFile<Contacts>(files[1]);
			messages = BinarySerialization.ReadFromBinaryFile<Messages>(files[2]);

            foreach (var message in messages)
            {
                if (message.Read == false)
                    msgCount++;
            }

			while (running) {
				BuildMenu (new string[] { "New Message", "List Messages (" + msgCount + " unread)", "Settings" });

				switch (Console.ReadKey (true).Key) {

					case ConsoleKey.D1:
						NewMessage ();
						break;

					case ConsoleKey.D2:
						ListMessages();
						break;

					case ConsoleKey.D3:
						Settings();
						break;

					case ConsoleKey.D0:
					case ConsoleKey.Escape:
						running = false;
						break;

					default:
						InvalidOption ();
						continue;
				}
			}

			WriteFiles(files);
		}

		private static void NewMessage()
		{
			BaseMessage message = new BaseMessage();

			message.Time = DateTime.Now.ToString();
			message.Sender = settings.Sender;

			FormatText(new string[] { "Choose Receiver" });

			int key = contacts.returnContactKey(contacts);
            
            if (contacts.Get(key).Server)
            {
                ClientMessage clientMessage = new ClientMessage();

                clientMessage.Name = message.Receiver;
                
                clientMessage.messages = new BaseMessage[messages.Count()];

                for (int i = 0; i < messages.Count(); i++)
                    clientMessage.messages[i] = messages.Get(i);

                Client client = new Client();

                client.StartClient(clientMessage);

                Console.ReadKey();
            }
            else
            {
			    message.Receiver = contacts.Get(key).Name;

			    FormatText(new string[] { "Input the Subject" });

			    message.Subject = Console.ReadLine();

			    FormatText(new string[] { "Input the Message" });

			    message.Message = Console.ReadLine();

                message.Read = false;

			    FormatText(new string[] { "Time: " + message.Time, 
									    "Sender: " + message.Sender, 
									    "Receiver: " + message.Receiver, 
									    "Subject: " + message.Subject, 
									    "Message: " + message.Message });

                messages.Add(message);

                msgCount++;

			    Console.ReadKey(true);

            }
        }

		private static void ListMessages()
		{
            Console.Clear();

            foreach (var message in messages)
            {
				FormatText(new string[] { "Time: " + message.Time, 
									"Sender: " + message.Sender, 
									"Receiver: " + message.Receiver, 
									"Subject: " + message.Subject, 
									"Message: " + message.Message,
                                    "Read: " + (message.Read ? "true" : "false")}, false);
                message.Read = true;
                msgCount = 0;
            }

            Console.ReadKey(true);
		}

		private static void Settings()
		{
			FormatText(new string[] { "Sender: " + settings.Sender, "1: Change Sender", "2: View Contacts" });

			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.D1:
					settings.Sender = Console.ReadLine();
					break;

				case ConsoleKey.D2:
					Contacts();
					break;

				case ConsoleKey.D0:
                case ConsoleKey.Escape:
                    running = false;
                    break;

                default:
                	InvalidOption();
                	break;
			}
		}

		private static void Contacts()
		{
			FormatText(new string[] { "Choose action to take with Contacts" });
			BuildMenu(new string[] { "Add", "Remove", "List", "Update" }, false);

			int key;

			switch (Console.ReadKey(true).Key)
			{
				case ConsoleKey.D1:
					FormatText(new string[] { "Write the name of the new contact" });

                    string Name = Console.ReadLine();

                    FormatText(new string[] { "Is the contact the server? (y/n)" });

                    if (Console.ReadLine() == "y")
                        contacts.Add(new Contact { Name = Name, Server = true });
                    else
                        contacts.Add(new Contact { Name = Name, Server = false });

                    break;

				case ConsoleKey.D2:
					FormatText(new string[] { "Select which contact to choose" });

					key = contacts.returnContactKey(contacts);

                    if (contacts.Count() < key)
                    {
                        FormatText(new string[] { "Invalid choice" });
                        break;
                    }
                    else
                        contacts.Remove(key);

					break;

				case ConsoleKey.D3:
					FormatText(new string[] { "Contacts are:" });

					foreach (var contact in contacts)
						Console.WriteLine(contact.Name);

					Console.ReadKey();

					break;

				case ConsoleKey.D4:
					FormatText(new string[] { "Select which contact to choose" });

					key = contacts.returnContactKey(contacts);

					FormatText(new string[] { "Input new name" });

					contacts.Get(key).Name = Console.ReadLine();

					break;

				case ConsoleKey.D0:
                case ConsoleKey.Escape:
                    running = false;
                    break;

                default: 
                	InvalidOption();
                	break;
			}
		}

		private static void WriteFiles(string[] files)
		{
			JsonSerialization.WriteToJsonFile<Settings>(files[0], settings);

			XmlSerialization.WriteToXmlFile<Contacts>(files[1], contacts);

			BinarySerialization.WriteToBinaryFile<Messages>(files[2], messages);
		}

		private static void FormatText(string[] texts, bool clear = true)
		{
			if (clear)
				Console.Clear();

			foreach (var text in texts)
				Console.WriteLine(text);

            Console.Write("\n");
		}

		private static void InvalidOption ()
		{
			Console.Clear ();
			Console.WriteLine ("Invalid option");
			Console.ReadKey (true);
		}

		private static void CheckFile (string[] files)
		{
			foreach (var file in files) {
				if (!File.Exists (file))
					new StreamWriter(file);
			}
		}

		private static void BuildMenu (string[] options, bool clear = true)
		{
			if (clear)
				Console.Clear ();

			int i = 1;

			foreach (var option in options)
				Console.WriteLine (i++ + ". " + option);

			Console.WriteLine ("0. Exit(Esc)");
		}
	}
}
