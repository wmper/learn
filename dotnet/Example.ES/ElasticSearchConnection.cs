using Elasticsearch.Net;
using Nest;
using System;

namespace Example.ES
{
    public class ElasticSearchConnection : ConnectionSettings
    {
        private static readonly Uri[] uris = new[]
        {
            new Uri("http://192.168.180.93:9200")
        };
        private static readonly IConnectionPool pool = new StickyConnectionPool(uris);

        public ElasticSearchConnection() : base(pool)
        {

        }
    }
}
