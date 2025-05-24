using Tutorial11.API.DTOs;

namespace Tutorial11.API.Services;

public interface IPrescriptionService
{
    Task AddPrescriptionAsync(NewPrescriptionDto dto);
    Task<PatientDetailsDto> GetPatientWithPrescriptionsAsync(int id);
}