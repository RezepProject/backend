using backend.Controllers;
using backend.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace backend.Test;

public class AnswerTest
{
    private AnswerController _sut;
    
    public AnswerTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: new Guid().ToString())
            .Options;

        var ctx = new DataContext(options);
        _sut = new AnswerController(ctx);
        ctx.Database.EnsureDeleted();
        
    }
    
    [Fact]
    public async Task AddAnswer_GetAnswers_CheckSize()
    {
        Answer answer = new Answer();
        answer.Text = "Test";
        answer.User = "Test";
        
        await _sut.AddAnswer(answer);
        
        var result = await _sut.GetAnswers();
        result.Value.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task Check_Independency_of_Databases()
    {
        Answer answer = new Answer();
        answer.Text = "Test";
        answer.User = "Test";
        
        await _sut.AddAnswer(answer);
        
        var result = await _sut.GetAnswers();
        result.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAnswer_GetAnswers_CheckCount()
    {
        Answer answer = new Answer();
        answer.Text = "Test";
        answer.User = "Test";
        
        await _sut.AddAnswer(answer);
        
        var result = await _sut.GetAnswers();
        result.Value.Should().HaveCount(1);
        
        await _sut.DeleteAnswer(answer.Id);
        
        result = await _sut.GetAnswers();
        result.Value.Should().HaveCount(0);
    }
    
    public Answer CreateAnswer()
    {
        Answer answer = new Answer();
        answer.Text = "Text";
        answer.User = "User";
        
        return answer;
    }
}