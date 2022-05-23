using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using inventory_control_of_dep_dal.Repository;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_dal.Extensions
{
    public static class DALExtensions
    {
        public static void ConfigureDALServices(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<DbContext, DBContext>();
            services.AddDbContext<DBContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<Department>, Repository<Department>>();
            services.AddScoped<IRepository<Faculty>, Repository<Faculty>>();
            services.AddScoped<IRepository<InventoryBook>, Repository<InventoryBook>>();
            services.AddScoped<IRepository<MaterialValue>, Repository<MaterialValue>>();
            services.AddScoped<IRepository<OperationsType>, Repository<OperationsType>>();
            services.AddScoped<IRepository<Position>, Repository<Position>>();
            services.AddScoped<IRepository<Room>, Repository<Room>>();


            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireLowercase = false;
                //options.Password.RequireUppercase = false;
                //options.Password.RequireDigit = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DBContext>();
        }
    }
}
