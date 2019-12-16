using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDbFramework.UnitTests.Documents
{
    public class Actor
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string Locality { get; set; }
    }
}
