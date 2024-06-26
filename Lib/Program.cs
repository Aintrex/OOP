using Lib.Services;
using System.Text.Json.Serialization;
using Lib.Context;
using Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<LibContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//using (var lib = new LibContext())
//{
//    var year = lib.Releaseyears.FirstOrDefault(x => x.Id == 5);
//    lib.Releaseyears.Remove(year);
//    lib.SaveChanges();
//    var aut = lib.Authors.FirstOrDefault(x => x.Id == 5);
//var delu = await lib.Books.FirstOrDefaultAsync(x => x.Id == 4);
//var delu1 = await lib.Books.FirstOrDefaultAsync(x => x.Id == 5);
//var delu2 = await lib.Books.FirstOrDefaultAsync(x => x.Id == 6);
//var delu3 = await lib.Books.FirstOrDefaultAsync(x => x.Id == 7);
//var delu4 = await lib.Books.FirstOrDefaultAsync(x => x.Id == 8);
//lib.Books.RemoveRange(delu,delu1,delu2,delu3,delu4);
//lib.SaveChanges();
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
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/user/login";
                options.LogoutPath = "/user/logout";
            }); 

            
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICountryInterface, CountryService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IPublisherInterface, PublisherService>();
builder.Services.AddScoped<IReleaseYearInterface, ReleaseYearService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();
bool cookieDel = false;
app.Use(async (context, next) =>
{
    if (!cookieDel)
    {
        foreach (var cookie in context.Request.Cookies.Keys)
        {
            context.Response.Cookies.Delete(cookie);
        }
        cookieDel = true;
    }

    await next.Invoke();
}); 
app.Use(async (context, next) =>
{
    if (!context.Request.Cookies.ContainsKey("SessionStarted"))
    {
        context.Response.Cookies.Append("SessionStarted", "true");
        context.Response.Redirect("/login.html");
        return;
    }

    await next.Invoke();
});
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








