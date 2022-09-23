using MassTransit;
using OrderService.Consumers;
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

        cfg.ReceiveEndpoint("email-reports", re =>
        {
            // turns off default fanout settings
            re.ConfigureConsumeTopology = false;

            // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
            re.SetQuorumQueue();

            // enables a lazy queue for more stable cluster with better predictive performance.
            // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
            re.SetQueueArgument("declare", "lazy");

            re.Consumer<EmailReportConsumer>();
            re.Bind("report-requests", e =>
            {
                e.RoutingKey = "email";
                e.ExchangeType = ExchangeType.Direct;
            });
        });

        cfg.ReceiveEndpoint("fax-reports", re =>
        {
            re.ConfigureConsumeTopology = false;
            re.Consumer<FaxReportConsumer>();
            re.Bind("report-requests", e =>
            {
                e.RoutingKey = "fax";
                e.ExchangeType = ExchangeType.Direct;
            });
        });
        cfg.ReceiveEndpoint("messageQueue", ep =>
        {
            ep.Consumer<MessageConsumer>();
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
        });
        cfg.ReceiveEndpoint("private-reports", re =>
        {
            // turns off default fanout settings
            re.ConfigureConsumeTopology = false;

            // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
            re.SetQuorumQueue();

            // enables a lazy queue for more stable cluster with better predictive performance.
            // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
            re.SetQueueArgument("declare", "lazy");

            re.Consumer<EmailReportConsumer>();
            re.Consumer<FaxReportConsumer>();
            re.Bind("send-report", e =>
            {
                e.RoutingKey = "private.*";
                e.ExchangeType = ExchangeType.Topic;
            });
        });
        cfg.ReceiveEndpoint("public-reports", re =>
        {
            // turns off default fanout settings
            re.ConfigureConsumeTopology = false;

            // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
            re.SetQuorumQueue();

            // enables a lazy queue for more stable cluster with better predictive performance.
            // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
            re.SetQueueArgument("declare", "lazy");

            re.Consumer<CloudReportConsumer>();
            re.Bind("send-report", e =>
            {
                e.RoutingKey = "public.*";
                e.ExchangeType = ExchangeType.Topic;
            });
        });
    });
});

/*builder.Services.Configure<MassTransitHostOptions>(options =>
{
    options.WaitUntilStarted = true;
    options.StartTimeout = TimeSpan.FromSeconds(30);
    options.StopTimeout = TimeSpan.FromMinutes(1);
});*/
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
