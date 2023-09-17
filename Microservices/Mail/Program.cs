using Mail.Data;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x => {
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();

    var assembly = typeof(Program).Assembly;

    x.AddConsumers(assembly);
    x.AddSagaStateMachines(assembly);
    x.AddSagas(assembly);
    x.AddActivities(assembly);

    x.UsingRabbitMq((context, configuration) => {
        configuration.Host(builder.Configuration["RabbitMqSettings:Host"], "/", h => {
            h.Username(builder.Configuration["RabbitMqSettings:Username"]);
            h.Password(builder.Configuration["RabbitMqSettings:Password"]);
        });

        configuration.UseMessageRetry(x => {
            x.Interval(
                int.Parse(builder.Configuration["MailSettings:Retries"]), 
                TimeSpan.FromSeconds(int.Parse(builder.Configuration["MailSettings:DelayBeforeSendingLetterInSeconds"])));
        });

        configuration.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));

var app = builder.Build();

app.Run();