using Confluent.Kafka;
using TrackerAPI.Hubs;
using TrackerAPI.Services;

var builder = WebApplication.CreateBuilder(args);


// 1. REGISTER KAFKA PRODUCER AS A SINGLETON
// We create one instance here, and the app will reuse it forever.
var producerConfig = new ProducerConfig
{
    // Kafka broker address (how to connect to it)
    BootstrapServers = "localhost:9092",
    ClientId = "TrackerAPI"
};

//Add a producer builder using the config
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    return new ProducerBuilder<Null, string>(producerConfig).Build();
});

// 1. Register SignalR
builder.Services.AddSignalR();

// 2. Register the Background Worker
// This tells .NET: "Start this class when the API boots up and keep it alive."
builder.Services.AddHostedService<KafkaConsumerService>();

// Add CORS (Cross-Origin Resource Sharing)
// Vital because React (localhost:3000) is a different "origin" than .NET (localhost:5xxx)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // We will build this tomorrow
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // SignalR requires credentials (cookies/headers) allowed
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");

app.MapControllers();

// 4. Map the SignalR Hub to a URL path
app.MapHub<TrackingHub>("/trackingHub");


app.Run();
