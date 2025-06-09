using Microsoft.EntityFrameworkCore;
using GroupB.Models;

namespace GroupB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<TicketConcert> TicketConcerts      { get; set; }
        public DbSet<Ticket> Tickets                    { get; set; }
        public DbSet<Concert> Concerts                  { get; set; }
        public DbSet<Customer> Customers                { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Ticket>().HasData(
                new Ticket { TicketId = 1, SerialNumber = "TK2034/S4531/12", SeatNumber = 124 },
                new Ticket { TicketId = 2, SerialNumber = "TK2027/S4831/133", SeatNumber = 330 }
            );

            mb.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John",  LastName = "Doe",   PhoneNumber = null },
                new Customer { CustomerId = 2, FirstName = "Johny", LastName = "Brown", PhoneNumber = "123456789" }
            );

            mb.Entity<Concert>().HasData(
                new Concert { ConcertId = 1, Name = "Concert 1",  Date = DateTime.Parse("2025-06-07T09:00:00") },
                new Concert { ConcertId = 2, Name = "Concert 14", Date = DateTime.Parse("2025-06-10T09:00:00") }
            );

            mb.Entity<TicketConcert>().HasData(
                new TicketConcert { TicketConcertId = 1, TicketId = 1, ConcertId = 1, Price = 33.4m },
                new TicketConcert { TicketConcertId = 2, TicketId = 2, ConcertId = 2, Price = 48.4m }
            );

            mb.Entity<PurchasedTicket>().HasData(
                new PurchasedTicket 
                { 
                    TicketConcertId = 1, 
                    CustomerId      = 1, 
                    PurchaseDate    = DateTime.Parse("2025-06-03T09:00:00") 
                },
                new PurchasedTicket 
                { 
                    TicketConcertId = 2, 
                    CustomerId      = 1, 
                    PurchaseDate    = DateTime.Parse("2025-06-03T09:00:00") 
                }
            );
        }
    }
}
