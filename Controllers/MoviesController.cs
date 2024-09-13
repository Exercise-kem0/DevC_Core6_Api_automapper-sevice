using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace DevC_Core5_Api_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDBContext _context; 
        // represent Database
        // as it repeated with every control making maintance and code harder
        // => perfered save it in service or repositoryPattern
        private readonly List<string> _allowedPosterPicExtensions = new() { ".jpg", ".jpeg", ".png" };
        private readonly long _allowedMaxSizeInBytes = 1048576;
        public MoviesController(ApplicationDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var AllMovies = await _context.Movies
                .Include(x => x.Genre)
                .OrderByDescending(x => x.Rate)
                .ToListAsync();
            if (AllMovies == null)
            {
                return NotFound("No Movies Exist , add one");
            }
            return Ok(AllMovies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var foundMovie = await _context.Movies.Include(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
            if (foundMovie == null)
            {
                return NotFound($"Not found Movie with id : {id} ");
            }
            return Ok(foundMovie);
        }
        [HttpGet("GetByGenreIdAsync")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            if (!await _context.Genres.AnyAsync(x => x.Id == genreId))
            {
                return BadRequest($"Noo Genre with {genreId}");
            }
            var foundMovies = await _context.Movies
                .Where(x => x.GenreId == genreId)
                .Include(x => x.Genre)
                .OrderByDescending(x => x.Rate)
                .ToListAsync();
            if (foundMovies == null)
            {
                return NotFound($"not found Movies with Genre : {genreId} ");
            }
            return Ok(foundMovies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsyn([FromForm, FromBody] MovieDto MDto)
        {
            if (MDto.PostePic == null)
            {
                return BadRequest("PostePic Is Required When Creation");
            }
            //check size and exyension of poster pic and CategoryId
            if (!_allowedPosterPicExtensions.Contains(Path.GetExtension(MDto.PostePic.FileName).ToLower()))
            {
                return BadRequest($"Allowd Extensions only {String.Join(",", _allowedPosterPicExtensions)}");
            }
            if (MDto.PostePic.Length > _allowedMaxSizeInBytes)
            {
                return BadRequest($"Max allowed size {_allowedMaxSizeInBytes / (1024 * 1024)}");
            }

            var IsGenreIdExist = await _context.Genres.AnyAsync(x => x.Id == MDto.GenreId);
            if (!IsGenreIdExist)
            {
                return BadRequest($"Not Found Genre with id : {MDto.GenreId}");
            }
            //to chanege IformFile coming in Dto to byte[] that can be saved in database
            using var dataStream = new MemoryStream();  //create Stream(empty Byte array in memory) of in memory 
            await MDto.PostePic.CopyToAsync(dataStream); // cast IformFile to dataStrem(ArrayOfByte)

            var MovieToAdd = new Movie
            {
                Title = MDto.Title,
                Year = MDto.Year,
                Rate = MDto.Rate,
                Storyline = MDto.Storyline,
                GenreId = MDto.GenreId,
                PostePic = dataStream.ToArray(),
            };
            await _context.AddAsync(MovieToAdd);
            _context.SaveChanges();
            return Ok(MovieToAdd);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] MovieDto mDto)
        {
            var MovieToEdit = _context.Movies.Find(id);
            if (mDto.PostePic != null)
            {
                if (!_allowedPosterPicExtensions.Contains(Path.GetExtension(mDto.PostePic.FileName)))
                {
                    return BadRequest($"Allowed Extensions : {string.Join("|", _allowedPosterPicExtensions)}");
                }
                if (mDto.PostePic.Length > _allowedMaxSizeInBytes)
                {
                    return BadRequest($"Max allowed size : {_allowedMaxSizeInBytes / (1_048_576)} MB");
                }
                using MemoryStream posterPicStream = new();
                await mDto.PostePic.CopyToAsync(posterPicStream);
                MovieToEdit.PostePic = posterPicStream.ToArray();
            }
            if (!await _context.Genres.AnyAsync(x => x.Id == mDto.GenreId))
            {
                return BadRequest($"Not Found Genre with id : {mDto.GenreId}");
            }
            if (MovieToEdit == null) { return NotFound($"noo Movie with id : {id}"); }
            MovieToEdit.Title = mDto.Title;
            MovieToEdit.Rate = mDto.Rate;
            MovieToEdit.Year = mDto.Year;
            MovieToEdit.Storyline = mDto.Storyline;
            MovieToEdit.GenreId = mDto.GenreId;
            _context.SaveChanges();
            return Ok(MovieToEdit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            var foundMovie = await _context.Movies.FindAsync(id);
            if (foundMovie == null)
            {
                return NotFound($"Noo Movie with id {id}");
            }
            _context.Movies.Remove(foundMovie);
            _context.SaveChanges();
            return Ok($"Movie with id : {foundMovie.Id} => {foundMovie.Title} ==> Deleted Successfully");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteByGenreIdAsyn(byte genreId)
        {
            if (!await _context.Genres.AnyAsync(x => x.Id == genreId))
            {
                return BadRequest($"Noo Genre With id : {genreId}");
            }
            var foundMovies = await _context.Movies
                                    .Where(x => x.GenreId == genreId)
                                    .Include(x => x.Genre)
                                    .ToListAsync();
            if (foundMovies == null)
            {
                return NotFound($"Noo Movies To Delete in genre {genreId}");
            }
            _context.Movies.RemoveRange(foundMovies);
            _context.SaveChanges();
            return Ok($"{foundMovies.Count()} Movies => {string.Join(" | ", foundMovies.Select(x => x.Title))} \n Deleted Successfully from Genre : {string.Join(",", _context.Genres.Where(x => x.Id == genreId).Select(x => x.Name))}");
        }
    }
}
