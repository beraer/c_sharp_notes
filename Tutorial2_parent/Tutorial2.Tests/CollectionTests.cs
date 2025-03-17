using Tutorial2.Collections.Data;

namespace Tutorial2.Tests;

public class CollectionTests
{
    [Fact]
    public void ProcessGrades_ValidStudentList_ReturnsCorrectResults()
    {
        Type classType = typeof(Collections.Collections);
        var methodInfo = classType.GetMethod("ProcessGrades");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessGrades method does not exist.");
            return; 
        }
        
        // Arrange
        var students = new List<Student>
        {
            new Student { Name = "Katarzyna", Grade = 95 },
            new Student { Name = "Jan", Grade = 85 },
            new Student { Name = "Anna", Grade = 40 },
            new Student { Name = "Adam", Grade = 75 }
        };

        // Act
        var instance = Activator.CreateInstance(classType);
        var results = methodInfo.Invoke(instance, new object[] { students }) as List<StudentResult>;

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.Equal("Exemption", results[0].Result);
        Assert.Equal("Passing", results[1].Result);
        Assert.Equal("Needs Improvement", results[2].Result);
        Assert.Equal("Passing", results[3].Result);
    }

    [Fact]
    public void ProcessGrades_EmptyStudentList_ReturnsEmptyList()
    {
        Type classType = typeof(Collections.Collections);
        var methodInfo = classType.GetMethod("ProcessGrades");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessGrades method does not exist.");
            return; 
        }
        
        // Arrange
        var students = new List<Student>();

        // Act
        var instance = Activator.CreateInstance(classType);
        var results = methodInfo.Invoke(instance, new object[] { students }) as List<StudentResult>;

        // Assert
        Assert.NotNull(results);
        Assert.Empty(results);
    }

    [Fact]
    public void ProcessGrades_AllStudentsBelow50_ReturnsNeedsImprovement()
    {
        Type classType = typeof(Collections.Collections);
        var methodInfo = classType.GetMethod("ProcessGrades");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessGrades method does not exist.");
            return; 
        }
        
        // Arrange
        var students = new List<Student>
        {
            new Student { Name = "Katarzyna", Grade = 45 },
            new Student { Name = "Jan", Grade = 35 },
            new Student { Name = "Anna", Grade = 40 },
            new Student { Name = "Adam", Grade = 45 }
        };

        // Act
        var instance = Activator.CreateInstance(classType);
        var results = methodInfo.Invoke(instance, new object[] { students }) as List<StudentResult>;

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.All(results, result => Assert.Equal("Needs Improvement", result.Result));
    }

    [Fact]
    public void ProcessGrades_AllStudentsWithHighGrades_ReturnsExemption()
    {
        Type classType = typeof(Collections.Collections);
        var methodInfo = classType.GetMethod("ProcessGrades");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessGrades method does not exist.");
            return; 
        }
        
        // Arrange
        var students = new List<Student>
        {
            new Student { Name = "Katarzyna", Grade = 95 },
            new Student { Name = "Jan", Grade = 92 },
            new Student { Name = "Anna", Grade = 100 },
            new Student { Name = "Adam", Grade = 95 }
        };

        // Act
        var instance = Activator.CreateInstance(classType);
        var results = methodInfo.Invoke(instance, new object[] { students }) as List<StudentResult>;

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.All(results, result => Assert.Equal("Exemption", result.Result));
    }

    [Fact]
    public void ProcessGrades_MixedStudentGrades_ReturnsCorrectResults()
    {
        Type classType = typeof(Collections.Collections);
        var methodInfo = classType.GetMethod("ProcessGrades");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessGrades method does not exist.");
            return; 
        }
        
        // Arrange
        var students = new List<Student>
        {
            new Student { Name = "Katarzyna", Grade = 95 },
            new Student { Name = "Jan", Grade = 85 },
            new Student { Name = "Anna", Grade = 40 },
            new Student { Name = "Adam", Grade = 45 }
        };

        // Act
        var instance = Activator.CreateInstance(classType);
        var results = methodInfo.Invoke(instance, new object[] { students }) as List<StudentResult>;

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results);
        Assert.Equal("Exemption", results[0].Result);
        Assert.Equal("Passing", results[1].Result);
        Assert.Equal("Needs Improvement", results[2].Result);
        Assert.Equal("Needs Improvement", results[3].Result);
    }
}