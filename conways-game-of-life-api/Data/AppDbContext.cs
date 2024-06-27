using Microsoft.EntityFrameworkCore;
using conways_game_of_life_api.Models;
using Newtonsoft.Json;

namespace conways_game_of_life_api.Data
{
    /// <summary>
    /// The application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Alternate constructor for tests.
        /// </summary>
        public AppDbContext() { }

        /// <summary>
        /// Gets or sets the collection of boards.
        /// </summary>
        public DbSet<Board> Boards { get; set; }

        /// <summary>
        /// Configures the database context.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=conways_game_of_life.db");
        }

        /// <summary>
        /// Configures the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .Property(b => b.LiveCells)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<(int X, int Y)>>(v));
        }
    }
}
