using System.ComponentModel.DataAnnotations.Schema;

namespace DevC_Core5_Api_Jwt.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year {  get; set; }
        public double Rate {  get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        public byte[] PostePic { get; set; }
        [ForeignKey(nameof(Genre))]
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }


    }
}
