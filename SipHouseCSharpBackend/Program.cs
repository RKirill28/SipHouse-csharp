using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

using SipHouseCSharpBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Служба генерации Problem Details
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SipHouseContext>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sip House Api", Version = "v1" });
});
builder.Services.AddProblemDetails();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();
app.Run();


// using (SipHouseContext db = new())
// {
//     var project = new Project
//     {
//         Name = "test", Description = "Test", PriceDescription = "Test", IsPublic = true, PdfUrls = null, Price = 9999999999999.2m
//     };
//     var image = new Image
//     {
//         Name = "test image", Description = "Test image desc", IsMainImage = true, Project = project, Url = "test url to image",
//     };
//
//     db.Projects.Add(project);
//     db.Images.Add(image);
//     db.SaveChanges();
//     
//     foreach (var currentProject in db.Projects.Include(project => project.Images).ToArray())
//     {
//         foreach (var currImage in currentProject.Images)
//         {
//             Console.WriteLine(currImage.Name);
//         }
//     }
// }