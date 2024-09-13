
namespace DevC_Core5_Api_Jwt.Models
{
    public class ApplicationDBContext:DbContext
    {
        //kk=>1-manually override onconfigraion mehtod and add UseSql("ConnString")
        //2-best practice =>a- add connString to appSettings / b- addDBContext Service to Program / c-add Constructor to ApplicationDBContext with DBContextOptions to reterive ConnectionService
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) :base(opt){}

        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
    }
}
