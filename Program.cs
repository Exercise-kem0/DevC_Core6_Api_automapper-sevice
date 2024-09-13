
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//--------kk=>Add DBContext Service => to Add Connection string 
var ConnStringVar = builder.Configuration.GetConnectionString(name: "DefaultConnection");
builder.Services.AddDbContext<ApplicationDBContext>(opt =>
opt.UseSqlServer(ConnStringVar));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//kk------------=> can customize the Swagger service 
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc(name: "v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "KemoAPI",
        Description = "Devcreed core Api => Movies project",
        Contact = new OpenApiContact
        {
            Name = "Kareem Zarif",
            Email = "eng.kareem.zarif@gmail.com",
            Url = new Uri("https://www.facebook.com/kareem.zarif.5"),
        },
        License = new OpenApiLicense
        {
            Name = "Kemo License",
            Url = new Uri("https://github.com/Exercise-kem0/DevC-Crud-Frame-mvcAndApi")
        }
    });
    //kk=>to add Security to all Apis name Bearer if use jwt
    opt.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT key Starts With Bearer keyword"
    });
    //kk=>to add security to specific Api
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });


});

//-----------kk=> add CORS service so I can request/response Apis from other networks as react(any frontend) network
builder.Services.AddCors();
//-------kk=>add your Customized Services
builder.Services.AddTransient<IGenreService, GenreService>();
//--------kk=> add autoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//---------kk=>CORS preferd ba added before Authorization
app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
