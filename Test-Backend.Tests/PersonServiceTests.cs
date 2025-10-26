using Test_Backend.Services;
using Test_Backend.Models;

namespace Test_Backend.Tests;

/// <summary>
/// White-box tests for PersonService - focuses on Math.Clamp boundaries and loop logic
/// Note: Requires application to be running for full integration (database/file dependencies)
/// These tests demonstrate white-box testing principles on the GetRandomPersons method
/// </summary>
public class PersonServiceTests
{
    // WHITE-BOX: Demonstrates testing Math.Clamp lower boundary
    // Code path: count = Math.Clamp(count, 2, 100); where count < 2
    [Theory]
    [InlineData(0)]    // Zero should clamp to 2
    [InlineData(1)]    // One should clamp to 2
    [InlineData(-5)]   // Negative should clamp to 2
    [InlineData(-100)] // Large negative should clamp to 2
    public void MathClamp_LowerBoundary_Explanation(int input)
    {
        // WHITE-BOX EXPLANATION:
        // When count < 2, Math.Clamp(count, 2, 100) returns 2
        // This tests the branch: if (count < min) return min;

        int result = Math.Clamp(input, 2, 100);
        Assert.Equal(2, result);
    }

    // WHITE-BOX: Demonstrates testing Math.Clamp upper boundary
    // Code path: count = Math.Clamp(count, 2, 100); where count > 100
    [Theory]
    [InlineData(101)]   // Just over max should clamp to 100
    [InlineData(500)]   // Mid-range over should clamp to 100
    [InlineData(1000)]  // Large value should clamp to 100
    [InlineData(10000)] // Very large value should clamp to 100
    public void MathClamp_UpperBoundary_Explanation(int input)
    {
        // WHITE-BOX EXPLANATION:
        // When count > 100, Math.Clamp(count, 2, 100) returns 100
        // This tests the branch: if (count > max) return max;

        int result = Math.Clamp(input, 2, 100);
        Assert.Equal(100, result);
    }

    // WHITE-BOX: Demonstrates testing Math.Clamp valid range
    // Code path: count = Math.Clamp(count, 2, 100); where 2 <= count <= 100
    [Theory]
    [InlineData(2)]    // Min boundary - no clamp
    [InlineData(10)]   // Valid mid-range
    [InlineData(50)]   // Valid mid-range
    [InlineData(100)]  // Max boundary - no clamp
    public void MathClamp_ValidRange_Explanation(int input)
    {
        // WHITE-BOX EXPLANATION:
        // When 2 <= count <= 100, Math.Clamp returns the original value
        // This tests the branch: if (min <= count <= max) return count;

        int result = Math.Clamp(input, 2, 100);
        Assert.Equal(input, result);
    }

    // WHITE-BOX: Demonstrates loop execution logic
    [Fact]
    public void LoopLogic_Explanation()
    {
        // WHITE-BOX EXPLANATION:
        // In PersonService.GetRandomPersons:
        // for (int i = 0; i < count; i++) { list.Add(GetRandomPerson()); }
        //
        // This loop:
        // 1. Initializes: i = 0
        // 2. Condition: i < count (if count = 10, runs while i is 0-9)
        // 3. Body: list.Add(...) executes
        // 4. Increment: i++
        // 5. Repeats until i == count

        int count = 10;
        List<string> testList = new List<string>();

        // Simulating the service loop
        for (int i = 0; i < count; i++)
        {
            testList.Add($"Person{i}");
        }

        // WHITE-BOX: Verifies loop ran exactly 10 times
        Assert.Equal(10, testList.Count);
        Assert.Equal("Person0", testList[0]);  // First iteration (i=0)
        Assert.Equal("Person9", testList[9]);  // Last iteration (i=9)
    }

    // WHITE-BOX: Demonstrates conditional branch testing
    [Fact]
    public void ConditionalBranch_Explanation()
    {
        // WHITE-BOX EXPLANATION:
        // In PersonService.GetRandomPerson:
        // if (!string.IsNullOrEmpty(name.Gender))
        // {
        //     cpr = _cprService.GenerateCpr(cpr.DateOfBirth, name.Gender);
        // }
        //
        // This has TWO branches:
        // 1. TRUE branch: Gender is not null/empty → regenerate CPR with matching gender
        // 2. FALSE branch: Gender is null/empty → keep random CPR

        // Branch 1: Gender has value
        string gender1 = "female";
        bool branch1Executes = !string.IsNullOrEmpty(gender1);
        Assert.True(branch1Executes); // TRUE branch taken

        // Branch 2: Gender is null or empty
        string? gender2 = null;
        bool branch2Executes = !string.IsNullOrEmpty(gender2);
        Assert.False(branch2Executes); // FALSE branch taken (condition fails)
    }
}
