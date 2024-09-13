namespace DevC_Core5_Api_Jwt.Services
{
    public interface IGenreService
    {
        //contains all Method signutres deals with database in Genre
        public Task<List<Genre>> GetAll();
        Task<Genre> GetById(byte id);

        //one Get method with optional parameter
        public Task<IEnumerable<Genre>> Get(byte id=0);
        Task<Genre> Create(Genre GenreTOCreate);
        Genre Update( Genre genreToUpdate);
        Genre Delete(byte id);


    }
}
