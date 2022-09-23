using MassTransit;
using Rabbit.Models;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(mt =>
{
    mt.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.Message<StudentReport>(e => e.SetEntityName("report-requests")); // name of the primary exchange
        cfg.Publish<StudentReport>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
        cfg.Send<StudentReport>(e =>
        {
            e.UseRoutingKeyFormatter(context => context.Message.Provider); // route by provider (email or fax)
        });

        cfg.Message<Report>(e => e.SetEntityName("send-report")); // name of the primary exchange
        cfg.Publish<Report>(e => e.ExchangeType = ExchangeType.Topic); // primary exchange type
        cfg.Send<Report>(e =>
        {
            e.UseRoutingKeyFormatter(context =>
            {
                var sharedState = context.Message.IsPublic ? "public" : "private";
                return $"{sharedState}.{context.Message.Provider}";
            });
        });
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
