using CosmosDbFramework.IntegrationTests.Documents;
using CosmosDbFramework.Internal.Builders;
using CosmosDbFramework.Options;

namespace CosmosDbFramework.IntegrationTests.Contexts
{
    public class VideoStoreContext : CosmosDbContext
    {
        public VideoStoreContext(CosmosDbOptions<VideoStoreContext> options)
            : base(options)
        {

        }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Document<Actor>()
                .WithDatabase("videostore")
                .WithCollection("actors")
                .WithThroughput(400)
                .WithPartitionKey(c => c.Country);

            modelBuilder.Document<Movie>()
                .WithDatabase("videostore")
                .WithCollection("movies")
                .WithThroughput(400)
                .WithPartitionKey(c => c.Category);
        }

        public ICosmosCollection<Movie> Movies { get; set; }
        public ICosmosCollection<Actor> Actors { get; set; }
    }
}
