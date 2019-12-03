using Nest;

namespace Example.ES
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        [Keyword]
        public string LastName { get; set; }
        [Keyword]
        public string Tid { get; set; }
        public int Age { get; set; }
        public decimal Money { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
