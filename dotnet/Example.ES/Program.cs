using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Example.ES
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var conn = new ElasticSearchConnection();

            var client = conn.Client("people");

            var person1 = new Person
            {
                Id = 1,
                FirstName = "Martijn",
                LastName = "Laarman",
                Tid = "05e42becec2e479089a58885221bc1571",
                Age = 18,
                Money = 199.90m,
                Status = 2
            };

            var person2 = new Person
            {
                Id = 2,
                FirstName = "John",
                LastName = "Smith",
                Tid = "1486d013c4f64606baffcab3978836e2",
                Age = 19,
                Money = 1599.05m,
                Status = 3
            };

            var person3 = new Person
            {
                Id = 3,
                FirstName = "Martijn",
                LastName = "Smith",
                Tid = "3436c7790359fb75e050a8c002642eda",
                Age = 20,
                Money = 4199.33m,
                Status = 4
            };

            var peoples = new[] { person1, person2, person3 };

            //client.DeleteMany(peoples);

            //client.Index(person1, i => i.Index("people"));
            //client.Index(new IndexRequest<Person>(person1, "people"));

            //var response = client.IndexMany(peoples);
            //if (response.Errors)
            //{
            //    foreach (var item in response.ItemsWithErrors)
            //    {
            //        Console.WriteLine("Failed to index document {0}: {1}", item.Id, item.Error);
            //    }
            //}

            //client.IndexDocument(person1);
            //client.IndexDocument(person2);
            //client.IndexDocument(person3);

            //var response = client.IndexDocument(person1);
            //Console.WriteLine(JsonConvert.SerializeObject(response));

            //var searchRequest = new SearchRequest<Person>(Nest.Indices.AllIndices);
            //var searchRequest = new SearchRequest<Person>(Nest.Indices.AllIndices)
            //{
            //    From = 0,
            //    Size = 10,
            //    Query = new MatchQuery()
            //    {
            //        Field = Infer.Field<Person>(f => f.LastName),
            //        Query = "Smith"
            //    }
            //};

            //var searchResponse = client.Search<Person>(searchRequest);

            // unstructured search
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


            // structured search
            //var searchResponse = client.Search<Person>(s => s
            //                                                .Query(q => q
            //                                                    .MatchAll()
            //                                                )
            //                                            );

            //var searchResponse = client.Search<Person>(s => s
            //                                                .Query(q => q
            //                                                    .Bool(b => b
            //                                                        .Must(
            //                                                            //sd => sd.Match(m => m.Field(f => f.LastName).Query("Smith"))
            //                                                            //sd => sd.MatchPhrase(m => m.Field(f => f.LastName).Query("Smith"))
            //                                                            sd => sd.Term(t => t.Tid, "3436c7790359fb75e050a8c002642eda")
            //                                                        )
            //                                                    )
            //                                                )
            //                                            );

            //var where = new List<Func<QueryContainerDescriptor<Person>, QueryContainer>>
            //{
            //    //x => x.Fuzzy(f => f.Field(t=>t.LastName).Value("Smith")),
            //    //x =>x.Term(t=>t.Field(f=>f.Id).Value(1))
            //    x=>x.Ids(t=>t.Values(1,2))
            //};

            //var searchResponse = client.Search<Person>(s => s.From(0).Size(10).Query(q => q.Term(t => t.Tid, "3436c7790359fb75e050a8c002642eda")));

            //var searchResponse = client.Search<Person>(s => s.Query(q => q.MatchPhrase(m => m.Field(f => f.LastName).Query("Smith"))));

            //var person = client.Get<Person>(1).Source;
            //Console.WriteLine(JsonConvert.SerializeObject(person));

            var searchResponse = client.Search<Person>(s=>s.Query(q=>q.Terms(t=>t.Field(f=>f.Id).Terms(1,2))));

            var people = searchResponse.Documents;
            Console.WriteLine(JsonConvert.SerializeObject(people));

            // var searchResponse2 = client.Search<Person>(s => s
            //                             .Query(q => q
            //                                  .MatchAll()
            //                             )
            //                             .Aggregations(a => a
            //                                 .Terms("lastName", ta => ta
            //                                     .Field(f => f.LastName)
            //                                 )
            //                             )
            //                         );

            // var termsAggregation = searchResponse2.Aggregations.Terms("lastName");
            // Console.WriteLine(JsonConvert.SerializeObject(termsAggregation));

            Console.WriteLine("The End.");
            //Console.ReadLine();
        }
    }
}
