namespace DevC_Core5_Api_Jwt.Dtos
{
    public class MovieDto
    {
        //same model without => id as auto generated + Navigation property (Genre)
        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        //poster is pic from user => its type must be IFormFile
        public IFormFile? PostePic { get; set; }

        public byte GenreId { get; set; }
    }
}
