using Microsoft.EntityFrameworkCore;
using Rental.Models;

namespace Rental.Context
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> myContext) : base(myContext)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Car> cars { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }


        public DbSet<Category> categories { get; set; }
        public DbSet<Transmission> Transmissions { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }



        //for connection to db
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionBuilder)
        {
            base.OnConfiguring(dbContextOptionBuilder);
            //read from appsetting.json
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            var conString = config.GetConnectionString("MyDbConnectionString");
            //establish conncetion useSqlServer
            dbContextOptionBuilder.UseSqlServer(conString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
