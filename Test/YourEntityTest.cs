using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using backend;
/*
namespace Test
{
    [Collection("Non-Parallel Tests")] // Apply the collection attribute
    public class YourEntityTests : IClassFixture<TestSetup>
    {
        private readonly TestSetup _testSetup;

        public YourEntityTests(TestSetup testSetup)
        {
            _testSetup = testSetup;
        }

        private async Task ResetDatabase()
        {
            await _testSetup.ResetDatabaseAsync(); // Ensure a fresh database state before each test
        }

        [Fact]
        public async Task Test_Questions_Exist_InDatabase()
        {
            await ResetDatabase(); // Reset the database state

            var expectedQuestions = new[]
            {
                "What is the capital of France?",
                "What is the largest ocean on Earth?"
            };

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(_testSetup.GetConnectionString()); // Call the method to get the connection string

            using (var context = new DataContext(optionsBuilder.Options))
            {
                foreach (var expectedQuestion in expectedQuestions)
                {
                    var questionExists = await context.Questions.AnyAsync(q => q.Text == expectedQuestion);
                    Assert.True(questionExists, $"Expected question '{expectedQuestion}' to exist in the database.");
                }
            }
        }

        [Fact]
        public async Task Test_Answers_Exist_InDatabase()
        {
            await ResetDatabase(); // Reset the database state

            var expectedAnswers = new[]
            {
                "Paris",
                "Berlin",
                "Madrid",
                "Pacific Ocean",
                "Atlantic Ocean"
            };

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(_testSetup.GetConnectionString()); // Call the method to get the connection string

            using (var context = new DataContext(optionsBuilder.Options))
            {
                foreach (var expectedAnswer in expectedAnswers)
                {
                    var answerExists = await context.Answers.AnyAsync(a => a.Text == expectedAnswer);
                    Assert.True(answerExists, $"Expected answer '{expectedAnswer}' to exist in the database.");
                }
            }
        }

        [Fact]
        public async Task Test_EntityCount()
        {
            await ResetDatabase(); // Reset the database state

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(_testSetup.GetConnectionString()); // Call the method to get the connection string

            using (var context = new DataContext(optionsBuilder.Options))
            {
                var questionCount = await context.Questions.CountAsync();
                var answerCount = await context.Answers.CountAsync();

                Assert.Equal(2, questionCount);
                Assert.Equal(5, answerCount);
            }
        }
    }
} */
