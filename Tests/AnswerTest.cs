using backend;
using backend.Controllers;
using backend.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

// TODO Baiturana: move all tests to separate test project
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
    public async Task Can_Create_Answer()
    {
        //Arrange
        var answer = CreateNewAnswer(1);
        
        //Act
        await _sut.AddAnswer(answer);
        
        //Assert
        var result = await _sut.GetAnswer(1);
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task Can_Get_Answer_By_Id()
    {
        //Arrange
        PersistAnswerList(CreateNewAnswerList(10));
        
        //Act
        var result = await _sut.GetAnswer(7);
        
        //Assert
        result.Value.Id.Should().Be(7);
    }
    
    [Fact]
    public async Task Can_Get_All_Answers()
    {
        //Arrange
        PersistAnswerList(CreateNewAnswerList(18));
        
        //Act
        var result = await _sut.GetAnswers();
        
        //Assert
        result.Value.Count().Should().Be(18);
    }
    
    [Fact]
    public async Task Can_Update_Answer()
    {
        //Arrange
        PersistAnswerList(CreateNewAnswerList(10));
        var answer = new UpdateAnswer()
        {
            Text = "New Text",
            User = "New User",
        };
        
        //Act
        await _sut.ChangeAnswer(7, answer);
        
        //Assert
        var result = await _sut.GetAnswer(7);
        
        result.Value.Id.Should().Be(7);
        result.Value.Text.Should().Be("New Text");
        result.Value.User.Should().Be("New User");
        
    }
    
    [Fact]
    public async Task Can_Delete_Answer()
    {
        //Arrange
        PersistAnswerList(CreateNewAnswerList(10));
        
        //Act
        await _sut.DeleteAnswer(7);
        
        //Assert
        var result = await _sut.GetAnswers();
        
        result.Value.Count().Should().Be(9);
        result.Value.Should().NotContain(x => x.Id == 7);
    }

    
    private Answer CreateNewAnswer(int index)
    {
        Answer answer = new Answer()
        {
            Text = $"Text{index}",
            User = $"User{index}"
        };
        
        return answer;
    }
    private List<Answer> CreateNewAnswerList(int count)
    {
        List<Answer> answers = new List<Answer>();
        for (int i = 1; i <= count; i++)
        {
            answers.Add(CreateNewAnswer(i));
        }

        return answers;
    }
    private async void PersistAnswerList(List<Answer> answers)
    {
        foreach (var answer in answers)
        {
            await _sut.AddAnswer(answer);
        }
    } 
}