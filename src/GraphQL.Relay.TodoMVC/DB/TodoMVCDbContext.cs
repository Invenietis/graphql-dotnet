using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace GraphQL.Relay.TodoMVC.DB
{
    public class TodoMVCDbContext : DbContext
    {
        public TodoMVCDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );
            optionsBuilder.UseSqlServer( "Server=.;Database=GraphQLRelay;Integrated Security=SSPI;" );

        }

        public DbSet<Todo> Todos { get; set; }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public User()
        {
            Todos = new HashSet<Todo>();
        }

        internal static readonly User Current = new User() { Id = 1 };

        public int Id { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }
    }

    public class Todo
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Completed { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
