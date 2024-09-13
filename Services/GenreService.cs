
namespace DevC_Core5_Api_Jwt.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDBContext _Context; // represent database => so can maintain it and change it from one place

        public GenreService(ApplicationDBContext context)
        {
            _Context = context;
        }

        public async Task<Genre> Create(Genre GenreTOAdd)
        {
             await _Context.Genres.AddAsync(GenreTOAdd);   
            _Context.SaveChanges();
            return GenreTOAdd;
        }

        

        public Genre Delete(byte id)
        {
            var GnreToDelete = _Context.Genres.FirstOrDefault(g => g.Id == id);
            if (GnreToDelete == null)
            {
                return null;
            }
            _Context.Remove(GnreToDelete);
            _Context.SaveChanges();
            return (GnreToDelete);
        }

        //one Get For GetAll and GetById
        public async Task<IEnumerable<Genre>> Get(byte id = 0)
        {
            return await _Context.Genres.Where(x => x.Id == id ||id==0).ToListAsync();
        }

        public async Task<List<Genre>> GetAll()
        {
            return await _Context.Genres.OrderBy(x => x.Name).ToListAsync(); ;
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _Context.Genres.SingleOrDefaultAsync(x => x.Id == id) ;
        }

       public Genre Update( Genre GenreToUpdate)
        {
            _Context.Update(GenreToUpdate);
            _Context.SaveChanges();
            return GenreToUpdate;
        }
    }
}
