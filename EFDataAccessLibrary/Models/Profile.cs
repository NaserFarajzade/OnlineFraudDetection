using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models;

public class Profile
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; }
    public long TransactionsAmountAverage { get; set; }
    public int AccountHolderCardsCount { get; set; }
}