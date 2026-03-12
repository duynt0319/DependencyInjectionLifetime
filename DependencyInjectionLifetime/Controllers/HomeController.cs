using DependencyInjectionLifetime.Models;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionLifetime.Controllers;

public class HomeController : Controller
{
    private readonly IOperationTransient _t1;
    private readonly IOperationTransient _t2;
    private readonly IOperationScoped _s1;
    private readonly IOperationScoped _s2;
    private readonly IOperationSingleton _singleton1;
    private readonly IOperationSingleton _singleton2;

    public HomeController(
        IOperationTransient t1,
        IOperationTransient t2,
        IOperationScoped s1,
        IOperationScoped s2,
        IOperationSingleton singleton1,
        IOperationSingleton singleton2)
    {
        _t1 = t1;
        _t2 = t2;
        _s1 = s1;
        _s2 = s2;
        _singleton1 = singleton1;
        _singleton2 = singleton2;
    }

    public IActionResult Index()
    {
        var model = new
        {
            Transient1 = _t1.GetId(),
            Transient2 = _t2.GetId(),
            Scoped1 = _s1.GetId(),
            Scoped2 = _s2.GetId(),
            Singleton1 = _singleton1.GetId(),
            Singleton2 = _singleton2.GetId()
        };

        return View(model);
    }
}
