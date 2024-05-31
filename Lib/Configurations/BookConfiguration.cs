using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Lib.Models;
namespace Lib.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .HasOne(b => b.author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            builder
                .HasOne(b=>b.ReleaseYear)
                .WithMany(ry => ry.Books)
                .HasForeignKey(b=>b.YearId);

            builder
                .HasOne(b=>b.language)
                .WithMany(l => l.Books)
                .HasForeignKey(b=>b.LanguageId);

            builder
                .HasOne(b => b.publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId);

            builder
                .HasOne(b => b.country)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CountryId);

            builder
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b=>b.GenreId);
        }
    }
}
