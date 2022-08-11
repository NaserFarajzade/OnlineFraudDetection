using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models;

public class AccountHolder
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(10)]
    public string NationalCode { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; }
}