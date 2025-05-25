using Xunit;
using Microsoft.EntityFrameworkCore;
using Tutorial11.API.Services;
using Tutorial11.API.DTOs;
using Tutorial11.API.Data;
using Tutorial11.API.Models;
using FluentAssertions;

namespace Tutorial11.APITest
{
    public class PrescriptionServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly PrescriptionService _service;

        public PrescriptionServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            SeedDatabase(_context);

            _service = new PrescriptionService(_context);
        }

        private void SeedDatabase(ApplicationDbContext context)
        {
            context.Medicaments.AddRange(
                new Medicament { IdMedicament = 1, Name = "Paracetamol", Description = "Painkiller", Type = "Tablet" },
                new Medicament { IdMedicament = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Tablet" }
            );

            context.Doctors.Add(new Doctor
            {
                IdDoctor = 1,
                FirstName = "Ahmet",
                LastName = "Yılmaz",
                Email = "ahmet@hospital.com"
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task AddPrescriptionAsync_Should_Add_When_Valid()
        {
            var dto = new NewPrescriptionDto
            {
                Patient = new PatientDto
                {
                    FirstName = "Mehmet",
                    LastName = "Kara",
                    Birthdate = new DateTime(1990, 5, 10)
                },
                Medicaments = new List<MedicamentDto>
                {
                    new MedicamentDto
                    {
                        IdMedicament = 1,
                        Dose = 2,
                        Description = "Use after meal"
                    }
                },
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(3)
            };
            
            await _service.AddPrescriptionAsync(dto);
            
            var prescription = _context.Prescriptions.Include(p => p.PrescriptionMedicaments).FirstOrDefault();
            prescription.Should().NotBeNull();
            prescription.PrescriptionMedicaments.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddPrescriptionAsync_Should_Throw_When_TooManyMedics()
        {
            var tooMany = Enumerable.Range(1, 11).Select(i => new MedicamentDto
            {
                IdMedicament = 1,
                Dose = 1,
                Description = "Test"
            }).ToList();

            var dto = new NewPrescriptionDto
            {
                Patient = new PatientDto { FirstName = "Ali", LastName = "Veli", Birthdate = DateTime.Now.AddYears(-20) },
                Medicaments = tooMany,
                Date = DateTime.Now,
                DueDate = DateTime.Now
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddPrescriptionAsync(dto));
        }

        [Fact]
        public async Task GetPatientWithPrescriptionsAsync_Should_Return_Details()
        {
            var patient = new Patient
            {
                FirstName = "Zeynep",
                LastName = "Demir",
                Birthdate = new DateTime(1995, 3, 15)
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var prescription = new Prescription
            {
                IdPatient = patient.IdPatient,
                IdDoctor = 1,
                Date = DateTime.Today,
                DueDate = DateTime.Today.AddDays(2),
                PrescriptionMedicaments = new List<PrescriptionMedicament>
                {
                    new()
                    {
                        IdMedicament = 1,
                        Dose = 5,
                        Details = "Before sleep"
                    }
                }
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();


            var result = await _service.GetPatientWithPrescriptionsAsync(patient.IdPatient);

     
            result.Should().NotBeNull();
            result.FirstName.Should().Be("Zeynep");
            result.Prescriptions.Should().HaveCount(1);
            result.Prescriptions[0].Medicaments[0].Name.Should().Be("Paracetamol");
        }
    }
}
