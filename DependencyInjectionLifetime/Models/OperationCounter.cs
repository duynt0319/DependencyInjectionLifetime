namespace DependencyInjectionLifetime.Models;

// Interface chính
public interface IOperationCounter
{
    string GetId();
}

// Marker interfaces để phân biệt các lifetime
public interface IOperationTransient : IOperationCounter { }
public interface IOperationScoped : IOperationCounter { }
public interface IOperationSingleton : IOperationCounter { }

// Implementation duy nhất cho tất cả
public class OperationCounter : IOperationTransient, IOperationScoped, IOperationSingleton
{
    private readonly string _id;

    public OperationCounter()
    {
        _id = Guid.NewGuid().ToString();
    }

    public string GetId() => _id;
}
