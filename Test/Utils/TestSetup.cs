using backend;
using backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Test
{
    public class TestSetup : IAsyncLifetime
    {
        private readonly string _connectionString;

        public TestSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = config.GetConnectionString("TestConnection");
        }

        public async Task InitializeAsync()
        {
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task ResetDatabaseAsync()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(_connectionString);

            using (var context = new DataContext(optionsBuilder.Options))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                SeedData(context);
            }
        }

        private void SeedData(DataContext context)
        {
            var questions = new List<Question>
            {
                new Question
                {
                    Text = "How would you handle a guest who wants to check in early?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Check availability and accommodate if possible.", User = "test" },
                        new Answer { Text = "Inform them about the hotel's check-in policy.", User = "test" },
                        new Answer { Text = "Offer to store their luggage until check-in.", User = "test" }
                    }
                },
                new Question
                {
                    Text = "What should you do if a guest complains about their room?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Apologize and offer to change their room.", User = "test" },
                        new Answer { Text = "Take note and escalate to the management.", User = "test" },
                        new Answer { Text = "Attempt to fix the issue personally.", User = "test" }
                    }
                },
                new Question
                {
                    Text = "How do you ensure a smooth check-out process?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Review the bill with the guest before finalizing.", User = "test" },
                        new Answer { Text = "Ask about their stay and ensure satisfaction.", User = "test" },
                        new Answer { Text = "Inquire if they need assistance with transportation.", User = "test" }
                    }
                }
            };

            context.Questions.AddRange(questions);
            context.SaveChanges();
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
