using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDbFramework.IntegrationTests.Documents
{
    public class Actor
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string Locality { get; set; }
    }
}
