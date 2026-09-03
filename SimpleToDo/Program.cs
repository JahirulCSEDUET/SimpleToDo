using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Application.Common.Behaviors;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Application.Mappings;
using SimpleToDo.Domain.Interfaces;
using SimpleToDo.Infrastructure.Data;
using SimpleToDo.Infrastructure.Identity;
using SimpleToDo.Infrastructure.Repositories;
using SimpleToDo.Infrastructure.Services;
using SimpleToDo.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddDbContext<SimpleToDoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
builder.Services.AddScoped<IQueryRepository, QueryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

var applicationAssembly = typeof(TodoMappingProfile).Assembly;

builder.Services.AddAutoMapper(cfg => { }, applicationAssembly);
builder.Services.AddValidatorsFromAssembly(applicationAssembly);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(applicationAssembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});


builder.Services.AddScoped<IFileService>(provider =>
{
    var environment = provider.GetRequiredService<IWebHostEnvironment>();
    return new LocalFileService(environment.WebRootPath);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<SimpleToDoDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapHub<ChatHub>("/chatHub");
app.MapHub<NotificationHub>("/notificationHub");
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}")
    .WithStaticAssets();


app.Run();
