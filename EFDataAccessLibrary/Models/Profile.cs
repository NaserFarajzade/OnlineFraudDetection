using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models;

public class Profile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string CardNumber { get; set; }
    public long TransactionsAmountAverage { get; set; }
    public int AccountHolderCardsCount { get; set; }
}