using Nest;

namespace Example.ES
{
    public static class ElasticSearchConnectionExtensions
    {
        public static IElasticClient Client(this ElasticSearchConnection conn, string index)
        {
            conn.DefaultIndex(index);

            return new ElasticClient(conn);
        }
    }
}
