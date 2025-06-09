using GroupB.Data;
using GroupB.Exceptions;
using GroupB.Models;
using Microsoft.EntityFrameworkCore;
using GroupB.DTOs;

namespace GroupB.Services;

public class DbService : IDbService
{
    private readonly AppDbContext _context;

    public DbService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RespondCustomerPurchasesDto> GetTicketsForCustomerAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.PurchasedTickets)
            .ThenInclude(pt => pt.TicketConcert)
            .ThenInclude(tc => tc.Ticket)
            .Include(c => c.PurchasedTickets)
            .ThenInclude(pt => pt.TicketConcert)
            .ThenInclude(tc => tc.Concert)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (customer is null)
            throw new NotFoundException("Customer not found");

        var dto = new RespondCustomerPurchasesDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Purchases = customer.PurchasedTickets.Select(pt => new PurchaseDto
            {
                Date = pt.PurchaseDate,
                Price = pt.TicketConcert.Price,
                Ticket = new TicketDto
                {
                    Serial = pt.TicketConcert.Ticket.SerialNumber,
                    SeatNumber = pt.TicketConcert.Ticket.SeatNumber
                },
                Concert = new ConcertDto
                {
                    Name = pt.TicketConcert.Concert.Name,
                    Date = pt.TicketConcert.Concert.Date
                }
            }).ToList()
        };

        return dto;
    }

    public async Task AddCustomerWithPurchasesAsync(RequestCustomerPurchaseDto dto)
    {
        if (dto?.Customer == null || dto.Purchases == null || !dto.Purchases.Any())
        {
            throw new ValidationException("Customer and at least one purchase are required.");
        }

        if (await _context.Customers.AnyAsync(c => c.CustomerId == dto.Customer.Id)){
            throw new ConflictException("Customer already exists");
        }

        var customer = new Customer {
            CustomerId  = dto.Customer.Id,
            FirstName   = dto.Customer.FirstName,
            LastName    = dto.Customer.LastName,
            PhoneNumber = dto.Customer.PhoneNumber
        };
            _context.Customers.Add(customer);

    
        foreach (var p in dto.Purchases)
        {
        
            var concert = await _context.Concerts
                .FirstOrDefaultAsync(c => c.Name == p.ConcertName);
            if (concert == null)
                throw new NotFoundException($"Concert '{p.ConcertName}' not found"); 

            var existingCount = await _context.PurchasedTickets
                .Include(pt => pt.TicketConcert)
                .ThenInclude(tc => tc.Concert)
                .Where(pt => pt.CustomerId == customer.CustomerId)
                .CountAsync(pt => pt.TicketConcert.Concert.Name == p.ConcertName);
            if (existingCount >= 5)
                throw new ConflictException(
                    $"Customer cannot purchase more than 5 tickets for '{p.ConcertName}'");  

            var ticket = new Ticket {
                SerialNumber = Guid.NewGuid().ToString(), 
                SeatNumber   = p.SeatNumber
            };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var ticketConcert = new TicketConcert {
                TicketId   = ticket.TicketId,
                ConcertId  = concert.ConcertId,
                Price      = p.Price
            };
            _context.TicketConcerts.Add(ticketConcert);
            await _context.SaveChangesAsync();
            
            var purchased = new PurchasedTicket {
                CustomerId       = customer.CustomerId,
                TicketConcertId  = ticketConcert.TicketConcertId,
                PurchaseDate     = DateTime.UtcNow
            };
            _context.PurchasedTickets.Add(purchased);
        }

        await _context.SaveChangesAsync();
    }
}

