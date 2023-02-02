using eShop.API.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.API.Entities.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Games",
                },
                new Category
                {
                    Id = 2,
                    Name = "Electronics"
                },
                new Category
                {
                    Id = 3,
                    Name = "Auto"
                },
                new Category
                {
                    Id = 4,
                    Name = "Clothes"
                });
        }
    }
}
