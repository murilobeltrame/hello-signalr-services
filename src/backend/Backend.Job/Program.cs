using Backend.Job.Hubs;
using Backend.Job.Jobs;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// SETUP SIGNALR
builder.Services
    .AddSignalR()
    .AddAzureSignalR();

builder.Services
    .AddHangfire(configuration => configuration.UseInMemoryStorage())
    .AddHangfireServer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseRouting();

app.MapHub<MessagingHub>("/messaging");

app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<SendMessageJob>((job) => job.SendAsync("Hello from Hangfire"), "*/1 * * * *");

app.Run();
