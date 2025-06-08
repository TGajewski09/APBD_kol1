using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium.Models;

[Table("Group")]
public class Group
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(10)]
    public string Name { get; set; }
    
    public virtual ICollection<GroupAssigment> GroupAssigments { get; set; } = null!;

}