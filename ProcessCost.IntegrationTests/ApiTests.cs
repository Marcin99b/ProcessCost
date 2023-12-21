using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProcessCost.Domain;
using ProcessCost.Domain.Handlers;
using ProcessCost.Domain.Models;

namespace ProcessCost.IntegrationTests;

[TestFixture]
public class ApiTests
{
    private readonly WebApplicationFactory<Program> _factory = new();

    private readonly List<Stage> _stagesInRepository =
    [
        new("A", 01, new(10M, Currency.PLN)),
        new("A", 05, new(10M, Currency.PLN)),
        new("A", 10, new(100M, Currency.PLN)),
        new("A", 12, new(-80M, Currency.PLN)),
        new("A", 12, new(50M, Currency.PLN)),
        new("A", 15, new(200M, Currency.PLN)),
        new("A", 16, new(-30M, Currency.PLN)),
        new("A", 19, new(-5M, Currency.PLN)),
        new("A", 21, new(10M, Currency.PLN)),
    ];

    private readonly StageGroup _stageGroupInRepository = new("TestGroup");

    [Test]
    public async Task GetStages_Default_ShouldReturnData()
    {
        //Arrange
        var client = this.CreateClient(x => { x.AddScoped(_ => this.MockStagesRepository().Object); });

        //Act
        var response = await client.GetAsync("/v1.0/stages");
        var result = await response.ParseTo<GetStagesResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Stages.Should().HaveCount(9).And.BeEquivalentTo(this._stagesInRepository);
    }

    [TestCase(1, 10_00)]
    [TestCase(3, 10_00)]
    [TestCase(5, 20_00)]
    [TestCase(12, 90_00)]
    [TestCase(15, 290_00)]
    public async Task GetStateAtDay_Default_ShouldCalculate(int day, int expectedAmount)
    {
        //Arrange
        var client = this.CreateClient(x => { x.AddScoped(_ => this.MockStagesRepository().Object); });

        //Act
        var response = await client.GetAsync($"/v1.0/state/{day}");
        var result = await response.ParseTo<GetStateAtSelectedDayResponse>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Balance.CalculationAmount.Should().Be(expectedAmount);
    }

    [Test]
    public async Task CreateStageGroup_Default_ShouldCreate()
    {
        //Arrange
        var mockGroupsRepository = this.MockStagesGroupsRepository();
        var client = this.CreateClient(x => { x.AddScoped(_ => mockGroupsRepository.Object); });
        var input = new CreateStageGroupRequest("GroupName");

        //Act
        var response = await client.SendPost("/v1.0/stages/groups", input);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockGroupsRepository
            .Verify(
                x => x.Create(It.Is<StageGroup>(item => item.Name == "GroupName")),
                Times.Once);
    }

    [Test]
    public async Task DeleteStageGroup_Default_ShouldDelete()
    {
        //Arrange
        var mockGroupsRepository = this.MockStagesGroupsRepository();
        var client = this.CreateClient(x => { x.AddScoped(_ => mockGroupsRepository.Object); });

        //Act
        var response = await client.DeleteAsync($"/v1.0/stages/groups/{this._stageGroupInRepository.Id}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockGroupsRepository
            .Verify(
                x => x.Delete(this._stageGroupInRepository.Id),
                Times.Once);
    }

    [Test]
    public async Task AddStageToGroup_Default_ShouldAdd()
    {
        //Arrange
        var mockGroupsRepository = this.MockStagesGroupsRepository();
        var client = this.CreateClient(x =>
        {
            x.AddScoped(_ => this.MockStagesRepository().Object);
            x.AddScoped(_ => mockGroupsRepository.Object);
        });
        var input = new AddStageToGroupRequest(this._stageGroupInRepository.Id, this._stagesInRepository.Last().Id);

        //Act
        var response = await client.SendPost("/v1.0/stages/groups/add", input);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockGroupsRepository.Verify(x => x
                .Update(It.Is<StageGroup>(item =>
                    item.Id == this._stageGroupInRepository.Id &&
                    item.StagesIds.Contains(this._stagesInRepository.Last().Id))),
            Times.Once);
    }

    [Test]
    public async Task RemoveStageFromGroup_Default_ShouldRemove()
    {
        //Arrange
        var mockGroupsRepository = this.MockStagesGroupsRepository();
        var client = this.CreateClient(x =>
        {
            x.AddScoped(_ => this.MockStagesRepository().Object);
            x.AddScoped(_ => mockGroupsRepository.Object);
        });
        var input = new RemoveStageFromGroupRequest(this._stageGroupInRepository.Id, this._stagesInRepository[0].Id);

        //Act
        var response = await client.SendPost("/v1.0/stages/groups/remove", input);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockGroupsRepository.Verify(x => x
                .Update(It.Is<StageGroup>(item =>
                    item.Id == this._stageGroupInRepository.Id
                    && !item.StagesIds.Contains(this._stagesInRepository[0].Id)
                    && item.StagesIds.Contains(this._stagesInRepository[1].Id))),
            Times.Once);
    }

    private HttpClient CreateClient(Action<IServiceCollection>? registerServices = null)
    {
        var client = this._factory
            .WithWebHostBuilder(builder => { builder.ConfigureServices(x => { registerServices?.Invoke(x); }); })
            .CreateDefaultClient();
        return client;
    }

    private Mock<IStagesRepository> MockStagesRepository()
    {
        var mock = new Mock<IStagesRepository>();

        mock
            .Setup(x => x.GetAllStagesOfUser(It.IsAny<Guid>()))
            .Returns(this._stagesInRepository);

        mock
            .Setup(x => x.GetStageById(It.IsAny<Guid>()))
            .Returns<Guid>(x => Task.FromResult(this._stagesInRepository.FirstOrDefault(item => item.Id == x)));

        return mock;
    }

    private Mock<IStagesGroupsRepository> MockStagesGroupsRepository()
    {
        if (!this._stageGroupInRepository.StagesIds.Any())
        {
            this._stageGroupInRepository.AddStage(this._stagesInRepository[0]);
            this._stageGroupInRepository.AddStage(this._stagesInRepository[1]);
        }

        var mock = new Mock<IStagesGroupsRepository>();
        mock
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(this._stageGroupInRepository)!);

        mock
            .Setup(x => x.GetById(this._stageGroupInRepository.Id))
            .Returns(Task.FromResult(this._stageGroupInRepository)!);

        return mock;
    }
}