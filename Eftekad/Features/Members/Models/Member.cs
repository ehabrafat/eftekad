using System.ComponentModel.DataAnnotations.Schema;
using Eftekad.Features.AcademicStages;
using Eftekad.Features.Users;

namespace Eftekad.Features.Members;

using System;

public class Member
{
    public int Id { get; set; } 

    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    public User? User { get; set; }
    
    [NotMapped]
    public int? Age 
    { 
        get 
        {
            if (!DateOfBirth.HasValue)
                return null; 
            
            var today = DateOnly.FromDateTime(DateTime.Today);
        var birthDate = DateOfBirth.Value; // Ensure we only use the date part
        var age = today.Year - birthDate.Year;
        
        // Check if birthday has occurred this year
        if (birthDate > today.AddYears(-age))
            age--;
        
        return age;
        }
    }

    public string Status { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public int? AcademicStageId { get; set; }
    
    public AcademicStage? AcademicStage { get; set; } 
    
    public string Address { get; set; } = string.Empty;

    public string Area { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string Building { get; set; } = string.Empty;

    public string Floor { get; set; } = string.Empty;

    public string Apartment { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;
}