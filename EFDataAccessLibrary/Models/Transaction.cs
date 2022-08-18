using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataAccessLibrary.Models;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
}