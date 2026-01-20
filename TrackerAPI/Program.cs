using Confluent.Kafka;

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

app.MapControllers();


app.Run();
