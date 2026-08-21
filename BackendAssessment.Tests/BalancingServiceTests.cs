using System;
using System.Collections.Generic;
using System.Linq;
using BackendAssessment.Domain.Services;
using Xunit;

namespace BackendAssessment.Tests;

public class BalancingServiceTests
{
	[Fact]
	public void Test_Sample_Case()
	{
		var input = new List<int> { 4, 5, 1, 7, 6, 4, 0 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());
		Assert.Equal(input.Count, result.Count);
		Assert.Equal(0, result[6]);

		var activeResults = result.Where((value, index) => input[index] > 0).ToList();

		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	[Fact]
	public void Test_Total_Divisible()
	{
		var input = new List<int> { 2, 4, 6 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		var activeResults = result.Where((value, index) => input[index] > 0).ToList();
		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	[Fact]
	public void Test_Total_With_Remainder()
	{
		var input = new List<int> { 4, 5, 1, 7, 6, 4, 0 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		var activeResults = result.Where((value, index) => input[index] > 0).ToList();
		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	[Fact]
	public void Test_All_Zeros()
	{
		var input = new List<int> { 0, 0, 0, 0 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());
		Assert.All(result, value => Assert.Equal(0, value));
	}

	[Fact]
	public void Test_Only_One_Active()
	{
		var input = new List<int> { 0, 0, 10, 0, 0 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		for (var i = 0; i < input.Count; i++)
		{
			if (input[i] == 0)
			{
				Assert.Equal(0, result[i]);
			}
		}

		Assert.Equal(10, result[2]);
	}

	[Fact]
	public void Test_Tie_Priority()
	{
		var input = new List<int> { 3, 3, 3 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		var activeResults = result.Where((value, index) => input[index] > 0).ToList();

		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	[Fact]
	public void Test_Invalid_Negative()
	{
		var input = new List<int> { 1, -2, 3 };

		Assert.Throws<ArgumentException>(
			() => BalancingService.Balance(input)
		);
	}

	// Edge Case 1:
	[Fact]
	public void Edge_Mixed_Active_And_Inactive()
	{
		var input = new List<int> { 10, 0, 0, 5, 0, 8 };
		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		for (var i = 0; i < input.Count; i++)
		{
			if (input[i] == 0)
			{
				Assert.Equal(0, result[i]);
			}
		}

		var activeResults = result
			.Where((value, index) => input[index] > 0)
			.ToList();

		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	// Edge Case 2:
	[Fact]
	public void Edge_Large_Numbers()
	{
		var input = new List<int>
		{
			1_000_000,
			2_000_000,
			3_000_000
		};

		var result = BalancingService.Balance(input);

		Assert.Equal(input.Sum(), result.Sum());

		var activeResults = result
			.Where((value, index) => input[index] > 0)
			.ToList();

		Assert.True(activeResults.Max() - activeResults.Min() <= 1);
	}

	// Edge Case 3:
	[Fact]
	public void Edge_Empty_List()
	{
		var input = new List<int>();

		var result = BalancingService.Balance(input);

		Assert.Empty(result);
	}
}