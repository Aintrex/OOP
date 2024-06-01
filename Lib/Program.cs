using Lib.Services;
using System.Text.Json.Serialization;
using Lib.Context;
using Lib.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<LibContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//using (var lib = new LibContext())
//{
//    Author aut1 = new Author { Name = "Jo Nesbe" };
//    Author aut2 = new Author { Name = "Stephen King" };
//    Author aut3 = new Author { Name = "Alexandr Pushkin" };
//    lib.Authors.AddRange(aut1, aut2, aut3);
//    lib.SaveChanges();
//    Country cou1 = new Country { Name = "Norway" };
//    Country cou2 = new Country { Name = "England" };
//    Country cou3 = new Country { Name = "Russia" };
//    lib.Countries.AddRange(cou1, cou2, cou3);
//    lib.SaveChanges();
//    Genre gen1 = new Genre { Name = "Detective" };
//    Genre gen2 = new Genre { Name = "Horror" };
//    Genre gen3 = new Genre { Name = "Poem" };
//    lib.Genres.AddRange(gen1, gen2, gen3);
//    lib.SaveChanges();
//    Language lan1 = new Language { Name = "English" };
//    Language lan2 = new Language { Name = "Russian" };
//    lib.Languages.AddRange(lan1, lan2);
//    lib.SaveChanges();
//    Publisher publisher1 = new Publisher { Name = "Some random publisher" };
//    Publisher publisher2 = new Publisher { Name = "Some famous publisher" };
//    lib.Publishers.AddRange(publisher1, publisher2);
//    lib.SaveChanges();
//    ReleaseYear ry1 = new ReleaseYear { Year = 1987 };
//    ReleaseYear ry2 = new ReleaseYear { Year = 2005 };
//    ReleaseYear ry3 = new ReleaseYear { Year = 1830 };
//    ReleaseYear ry4 = new ReleaseYear { Year = 1818 };
//    lib.Releaseyears.AddRange(ry1, ry2, ry3, ry4);
//    lib.SaveChanges();
//    Book book1 = new Book { Title = "Misery", AuthorId = aut2.Id, YearId = ry1.Id, GenreId = gen2.Id, CountryId = cou2.Id, LanguageId = lan1.Id, PublisherId = publisher2.Id };
//    Book book = new Book { Title = "The savior", AuthorId = 1, YearId = 2, GenreId = 1, CountryId = 1, LanguageId = 2, PublisherId = 1 };
//    Book book2 = new Book { Title = "Ruslan and Ludmila", AuthorId = 3, YearId = 4, GenreId = 3, CountryId = 3, LanguageId = 2, PublisherId = 1 };

//    lib.Books.AddRange(book, book1, book2);
//    lib.SaveChanges();

//    User user1 = new User { Login = "Aintrex", Password = "Password", Role = "user" };
//    lib.Users.Add(user1);
//    lib.SaveChanges();
//}
builder.Services.AddCors();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    // Serialize enums as strings in api responses (e.g. Gender)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    // Ignore possible object cycles
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapFallbackToFile("/login.html");
});

app.Run();

