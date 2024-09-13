
using System.ComponentModel.DataAnnotations.Schema;

namespace DevC_Core5_Api_Jwt.Models
{
    public class Genre
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
