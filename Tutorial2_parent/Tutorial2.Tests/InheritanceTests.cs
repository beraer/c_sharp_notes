using System.Reflection;
using Tutorial2.Inheritance;
using Xunit.Sdk;

namespace Tutorial2.Tests;

public class InheritanceTests
{
    [Fact]
    public void ClassA_ShouldImplement_NewInterface_IfItExists()
    {
        // Arrange
        Type classType = typeof(A);
        string interfaceName = "IMyInterface";

        // Act
        Type? interfaceType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == interfaceName && t.IsInterface);

        if (interfaceType == null)
        {
            Assert.Fail("Interface IMyInterface doesn't exist");
            return;
        }

        bool implementsInterface = interfaceType.IsAssignableFrom(classType);

        // Assert
        Assert.True(implementsInterface, $"Class A does not implement {interfaceName}.");
    }

    [Fact] public void Method_Should_Be_Virtual()
    {
        MethodInfo? methodInfo = typeof(A).GetMethod("NewMethod");
        
        Assert.NotNull(methodInfo);
        bool isVirtual = methodInfo.IsVirtual && !methodInfo.IsFinal;
        Assert.True(isVirtual, "NewMethod is not virtual");
    }
    
    [Fact] public void Method_In_Class_A_Should_Return_1()
    {
        MethodInfo methodInfo = typeof(A).GetMethod("NewMethod");

        Assert.NotNull(methodInfo);
        Assert.Equal(typeof(int), methodInfo.ReturnType);

        Type? classAType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == "A");
        
        var constructor = classAType.GetConstructor(new[] { typeof(string) });
        Assert.NotNull(constructor);
        
        object instance = constructor.Invoke(new object[] { "test" });
        var result = (int)methodInfo.Invoke(instance, null);
        
        Assert.Equal(1, result);
    }
    
    [Fact] public void Method_In_Class_B_Should_Return_2()
    {
        MethodInfo m = typeof(A).GetMethod("NewMethod");
        Type? classBType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == "B");

        if (classBType == null)
        {
            Assert.Fail("Class B does not exist!");
            return;
        }

        MethodInfo? methodInfo = classBType.GetMethod("NewMethod");

        Assert.NotNull(methodInfo); 
        Assert.Equal(typeof(int), methodInfo.ReturnType);

        var constructor = classBType.GetConstructor(new[] { typeof(string), typeof(int) });
        Assert.NotNull(constructor);
        
        object instance = constructor.Invoke(new object[] { "TestName", 10 });
        int result = (int)methodInfo.Invoke(instance, null);
        
        Assert.Equal(2, result);
    }
    
    [Fact]
    public void ClassB_ShouldInheritFrom_ClassA()
    {
        Type? classBType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name == "B");
        
        if (classBType == null)
        {
            Assert.Fail("Class B does not exist!");
            return;
        }
        
        Assert.True(typeof(A).IsAssignableFrom(classBType), "Class B does not inherit from A");
    }

}