using System.Reflection;
using Tutorial2.Exceptions;
using Tutorial2.Exceptions.Data;

namespace Tutorial2.Tests;

public class ExceptionTests
{
    [Fact]
    public void ProcessOrder_NullOrdersList_ThrowsArgumentNullException()
    {
        // Arrange
        Type classType = typeof(OrderProcessor);
        var methodInfo = classType.GetMethod("ProcessOrder");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessOrder method does not exist.");
            return;
        }
        
        var instance = Activator.CreateInstance(classType);

        // Act & Assert
        Action action = () => methodInfo.Invoke(instance, new object[] { null });
    
        var exception = Assert.Throws<TargetInvocationException>(action);
        Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("orders", ((ArgumentNullException)exception.InnerException).ParamName);
    }

    [Fact]
    public void ProcessOrder_InvalidOrderAmount_ThrowsArgumentException()
    {
        Type classType = typeof(OrderProcessor);
        var methodInfo = classType.GetMethod("ProcessOrder");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessOrder method does not exist.");
            return; 
        }
        
        // Arrange
        var processor = new OrderProcessor();
        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = -50, Status = "Pending" }
        };

        // Act & Assert
        var instance = Activator.CreateInstance(classType);
        
        Action action = () => methodInfo.Invoke(instance, new object[] { orders });
    
        var exception = Assert.Throws<TargetInvocationException>(action);
        Assert.IsType<ArgumentException>(exception.InnerException);
        Assert.Equal("Order 1 has an invalid amount.", exception.InnerException.Message);
    }

    [Fact]
    public void ProcessOrder_InvalidOrderStatus_ThrowsInvalidOperationException()
    {
        Type classType = typeof(OrderProcessor);
        var methodInfo = classType.GetMethod("ProcessOrder");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessOrder method does not exist.");
            return; 
        }
        
        // Arrange
        var processor = new OrderProcessor();
        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = 100, Status = "Shipped" }
        };

        // Act & Assert
        var instance = Activator.CreateInstance(classType);
        
        Action action = () => methodInfo.Invoke(instance, new object[] { orders });
    
        var exception = Assert.Throws<TargetInvocationException>(action);
        Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Equal("Order 1 cannot be processed because its status is Shipped.", exception.InnerException.Message);
    }

    [Fact]
    public void ProcessOrder_ValidOrders_ProcessesCorrectly()
    {
        Type classType = typeof(OrderProcessor);
        var methodInfo = classType.GetMethod("ProcessOrder");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessOrder method does not exist.");
            return; 
        }
        
        // Arrange
        var processor = new OrderProcessor();
        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = 100, Status = "Pending" },
            new Order { Id = 2, Amount = 50, Status = "Pending" }
        };

        // Act
        var instance = Activator.CreateInstance(classType);
        
        methodInfo.Invoke(instance, new object[] { orders });
        
        Assert.All(orders, order => Assert.Equal("Processed", order.Status));
    }

    [Fact]
    public void ProcessOrder_MixedValidAndInvalidOrders_StopsAtInvalidOrder()
    {
        Type classType = typeof(OrderProcessor);
        var methodInfo = classType.GetMethod("ProcessOrder");

        if (methodInfo == null)
        {
            Assert.Fail("ProcessOrder method does not exist.");
            return; 
        }
        
        // Arrange
        var processor = new OrderProcessor();
        var orders = new List<Order>
        {
            new Order { Id = 1, Amount = 100, Status = "Pending" },
            new Order { Id = 2, Amount = -50, Status = "Pending" },
            new Order { Id = 3, Amount = 200, Status = "Pending" } 
        };

        // Act & Assert
        var instance = Activator.CreateInstance(classType);
        Action action = () => methodInfo.Invoke(instance, new object[] { orders });
    
        var exception = Assert.Throws<TargetInvocationException>(action);
        Assert.IsType<ArgumentException>(exception.InnerException);
        Assert.Equal("Order 2 has an invalid amount.", exception.InnerException.Message);
        Assert.Equal("Processed", orders[0].Status);
        Assert.Equal("Pending", orders[1].Status);
        Assert.Equal("Pending", orders[2].Status);
    }
}