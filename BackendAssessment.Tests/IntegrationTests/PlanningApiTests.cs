using System.Net;
using System.Net.Http.Json;
using BackendAssessment.API;
using BackendAssessment.API.DTOs;
using BackendAssessment.Persistence.DbContext;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BackendAssessment.Tests.IntegrationTests;

public class PlanningApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PlanningApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_ValidPlanning_ShouldSaveToDatabase_AndReturn201()
    {
        var client = _factory.CreateClient();
        var request = new PlanningRequestDto
        {
            RequestCode = "INT-TEST-001",
            Slots = new List<int> { 4, 5, 1, 7, 6, 4, 0 },
            CandidateToken = "VEH-EDI_BACKEND"
        };

        var response = await client.PostAsJsonAsync("/api/planning", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var saved = db.Plannings.Include(p => p.Slots)
            .FirstOrDefault(p => p.RequestCode == "INT-TEST-001");

        Assert.NotNull(saved);
        Assert.Equal("PROCESSED", saved.Status);
        Assert.Equal(7, saved.Slots.Count);
        Assert.Equal(27, saved.Slots.Sum(s => s.OriginalQuantity));
    }

    [Fact]
    public async Task Post_DuplicateRequestCode_ShouldNotCreateNewData_AndReturnOk()
    {
        var client = _factory.CreateClient();
        var request = new PlanningRequestDto
        {
            RequestCode = "INT-TEST-DUP",
            Slots = new List<int> { 1, 2, 3 },
            CandidateToken = "VEH-EDI_BACKEND"
        };

        await client.PostAsJsonAsync("/api/planning", request);

        var response2 = await client.PostAsJsonAsync("/api/planning", request);

        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        var body = await response2.Content.ReadAsStringAsync();
        Assert.Contains("RequestCode sudah diproses", body);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var count = db.Plannings.Count(p => p.RequestCode == "INT-TEST-DUP");
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Post_NegativeSlot_ShouldReturnBadRequest_WithDetailedError()
    {
        var client = _factory.CreateClient();
        var request = new PlanningRequestDto
        {
            RequestCode = "INT-TEST-ERR",
            Slots = new List<int> { 4, -2, 5 }
        };

        var response = await client.PostAsJsonAsync("/api/planning", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Slot ke-2 tidak boleh negatif", body);
    }
}