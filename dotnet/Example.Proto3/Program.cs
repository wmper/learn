using System;
using Google.Protobuf;
using Google.Protobuf.Examples.AddressBook;
using System.IO;

namespace Example.Proto3
{
    class Program
    {
        /// <summary>
        /// .\protoc -I=C:\workspace\learn\dotnet\Example.Proto3 --csharp_out=C:\workspace\learn\dotnet\Example.Proto3 C:\workspace\learn\dotnet\Example.Proto3/addressbook.proto
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            byte[] bytes;
            // Create a new person
            Person person = new Person
            {
                Id = 1,
                Name = "Foo",
                Email = "foo@bar",
                Phones = { new Person.Types.PhoneNumber { Number = "555-1212" } }
            };
            using (MemoryStream stream = new MemoryStream())
            {
                // Save the person to a stream
                person.WriteTo(stream);
                bytes = stream.ToArray();
            }
            Person copy = Person.Parser.ParseFrom(bytes);

            using (var output = File.Create("john.dat"))
            {
                person.WriteTo(output);
            }

            Person john;
            using (var input = File.OpenRead("john.dat"))
            {
                john = Person.Parser.ParseFrom(input);
            }

            AddressBook book = new AddressBook
            {
                People = { copy }
            };
            bytes = book.ToByteArray();
            // And read the address book back again
            AddressBook restored = AddressBook.Parser.ParseFrom(bytes);
            // The message performs a deep-comparison on equality:
            if (restored.People.Count != 1 || !person.Equals(restored.People[0]))
            {
                throw new Exception("There is a bad person in here!");
            }

            Console.WriteLine("end.");

            Console.Read();
        }
    }
}
