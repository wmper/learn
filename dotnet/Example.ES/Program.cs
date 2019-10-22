using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System;

namespace Example.ES
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var uris = new[]
            //{
            //    new Uri("http://192.168.180.93:9200")
            //};
            //var connectionPool = new SniffingConnectionPool(uris);

            //var settings = new ConnectionSettings(connectionPool);

            var settings = new ConnectionSettings(new Uri("http://192.168.180.93:9200"));
            settings.DefaultIndex("people");

            var client = new ElasticClient(settings);

            //var person = new Person
            //{
            //    Id = 1,
            //    FirstName = "Martijn",
            //    LastName = "Laarman"
            //};

            //var person = new Person
            //{
            //    Id = 2,
            //    FirstName = "John",
            //    LastName = "Smith"
            //};

            //var person = new Person
            //{
            //    Id = 2,
            //    FirstName = "Martijn",
            //    LastName = "Smith"
            //};

            //var response = client.IndexDocument(person);
            //Console.WriteLine(JsonConvert.SerializeObject(response));

            //var searchResponse = client.Search<Person>(s => s
            //                            .From(0)
            //                            .Size(10)
            //                            .Query(q => q
            //                                 .Match(m => m
            //                                    .Field(f => f.LastName)
            //                                    .Query("Smith")
            //                                 )
            //                            )
            //                        );

            //var people = searchResponse.Documents;
            //Console.WriteLine(JsonConvert.SerializeObject(people));

            var searchResponse = client.Search<Person>(s => s
                                        .Size(0)
                                        .Query(q => q
                                             .Match(m => m
                                                .Field(f => f.FirstName)
                                                .Query("Martijn")
                                             )
                                        )
                                        .Aggregations(a => a
                                            .Terms("last_names", ta => ta
                                                .Field(f => f.LastName)
                                            )
                                        )
                                    );

            var termsAggregation = searchResponse.Aggregations.Terms("last_names");

            Console.WriteLine(JsonConvert.SerializeObject(termsAggregation));

            Console.WriteLine("The End.");
            Console.ReadLine();
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
