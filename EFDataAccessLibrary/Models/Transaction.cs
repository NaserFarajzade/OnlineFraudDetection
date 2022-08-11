using System.ComponentModel.DataAnnotations;

namespace EFDataAccessLibrary.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string OriginCard { get; set; }
    
    [Required]
    [MaxLength(16)]
    public string DestinationCard { get; set; }
    
    public DateTime TransactionDateTime { get; set; }
    public long TransactionAmount { get; set; }
    public string TransactionReference { get; set; }
    public string TransactionId { get; set; }
}