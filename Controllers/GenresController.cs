namespace DevC_Core5_Api_Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {//action in mvc Eqivalent to end point in Api
        private readonly IGenreService _genreService;
        private readonly IMapper _mapper;



        //using Depency(interface) not the concrete class(GenreService) as
        //1-Loose Coupling=> not change in controller if wanna change in Implemntation 
        //2-Testability => When you use an interface like IGenreService, you can easily mock or replace it in unit tests without involving the actual database or service logic
        public GenresController(IGenreService genre, IMapper mapper)
        {
            _genreService = genre;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            //var genres =await _genreService.GetAll();
            var genres =await _genreService.Get();
            if (genres == null) {
                return NotFound("No Genres Exist"); 
            }
            return Ok(genres);
        }
     
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(byte id)
        {
            //kk=> find() => if id is int // if byte use firstOrDefault
            //var foundGenre =await _genreService.GetById(id);
            var foundGenre=await _genreService.Get(id);
            if (foundGenre == null || !foundGenre.Any()) { return NotFound($"Not Found genre with id : {id}"); }
            return Ok(foundGenre);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody]GenreDto genreDto)
        {
            //var GenreTOCreate =new Genre {  Name = genreDto.Name }; 
            var AutoMapperData = _mapper.Map<Genre>(genreDto);
            var CreatedGenre =await _genreService.Create(AutoMapperData);
            return Ok(CreatedGenre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(byte id, [FromBody] GenreDto ContainEditDto)
        {
            var foundGenre =await _genreService.GetById(id);
            if (foundGenre==null)
            {
                return NotFound($"Noo Genre with id : {id}");
            }
            //foundGenre.Name = ContainEditDto.Name;
            _mapper.Map(genreDto,foundGenre); //_mapper.Map(source,destination)
           var GenreToUpdate =  _genreService.Update(foundGenre);
            return Ok(GenreToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsyn(byte id)
        {
            var genreToDelete = _genreService.Delete(id);
            if (genreToDelete == null) { return NotFound($"Noo Genre with id : {id}"); }
            return Ok(genreToDelete);
        }
    }
}
