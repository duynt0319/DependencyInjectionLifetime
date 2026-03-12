# Dependency Injection Lifetime Demo

## Mục đích

Dự án demo minh họa sự khác biệt giữa ba lifetime trong Dependency Injection của ASP.NET Core:
- **Transient**: Tạo instance mới mỗi lần inject
- **Scoped**: Sử dụng cùng instance trong một HTTP request
- **Singleton**: Sử dụng cùng instance xuyên suốt vòng đời ứng dụng

## Cấu trúc dự án

```
DependencyInjectionLifetime/
├── Models/
│   └── OperationCounter.cs       # Interface và implementation
├── Controllers/
│   └── HomeController.cs         # Controller inject 6 instances
├── Views/
│   └── Home/
│       └── Index.cshtml           # Hiển thị kết quả so sánh
└── Program.cs                     # Cấu hình DI
```

## Giải thích code

### 1. OperationCounter.cs

Định nghĩa:
- `IOperationCounter`: Interface chính với method `GetId()`
- `IOperationTransient`, `IOperationScoped`, `IOperationSingleton`: Marker interfaces để phân biệt lifetime
- `OperationCounter`: Class implementation duy nhất cho cả 3 interfaces

Mỗi instance được tạo ra sẽ có một GUID duy nhất được sinh trong constructor.

### 2. Program.cs - Đăng ký DI

```csharp
builder.Services.AddTransient<IOperationTransient, OperationCounter>();
builder.Services.AddScoped<IOperationScoped, OperationCounter>();
builder.Services.AddSingleton<IOperationSingleton, OperationCounter>();
```

Cùng một class `OperationCounter` được đăng ký với 3 lifetime khác nhau thông qua các marker interfaces.

### 3. HomeController.cs - Inject instances

Controller inject 6 instances:
- 2 instances của `IOperationTransient`
- 2 instances của `IOperationScoped`
- 2 instances của `IOperationSingleton`

Việc inject 2 instances của mỗi loại cho phép so sánh xem chúng có cùng ID hay không trong cùng một request.

### 4. Index.cshtml - Hiển thị kết quả

View hiển thị ID của 6 instances dưới dạng 3 cards với màu sắc khác nhau, giúp người dùng dễ dàng nhận biết sự khác biệt.

## Cách chạy

1. Mở terminal tại thư mục gốc của solution
2. Chạy lệnh:
```bash
dotnet run --project DependencyInjectionLifetime
```
3. Mở browser và truy cập `https://localhost:xxxx` (port sẽ được hiển thị trong terminal)
4. Quan sát kết quả và nhấn nút "Refresh Page" để thấy sự thay đổi

## Kết quả quan sát

### Transient
- **Instance 1** và **Instance 2** có ID khác nhau trong cùng một request
- Mỗi lần refresh, cả 2 ID đều thay đổi
- **Kết luận**: Mỗi lần inject tạo ra một instance mới hoàn toàn

### Scoped
- **Instance 1** và **Instance 2** có cùng ID trong một request
- Khi refresh trang (request mới), ID thay đổi nhưng 2 instances vẫn giống nhau
- **Kết luận**: Instance được tạo một lần cho mỗi HTTP request và dùng chung trong request đó

### Singleton
- **Instance 1** và **Instance 2** có cùng ID
- ID không thay đổi dù refresh trang bao nhiêu lần
- **Kết luận**: Instance được tạo một lần duy nhất khi ứng dụng khởi động và dùng chung cho tất cả requests

## Khi nào dùng lifetime nào?

### Transient
Sử dụng cho:
- Lightweight, stateless services
- Services không lưu trạng thái
- Operations không cần share data giữa các components

Ví dụ: Logger, HTTP clients, utility services

### Scoped
Sử dụng cho:
- Database contexts (Entity Framework DbContext)
- Services cần maintain state trong một request
- Unit of Work pattern

Ví dụ: DbContext, Repository patterns, Request-specific caching

### Singleton
Sử dụng cho:
- Services tốn tài nguyên để khởi tạo
- Services cần share state xuyên suốt application
- Thread-safe services

Ví dụ: Configuration, Caching services, Connection pools

**Lưu ý quan trọng**: Singleton services phải thread-safe vì có thể được truy cập đồng thời từ nhiều requests.

## Yêu cầu hệ thống

- .NET 10 SDK
- Browser hỗ trợ HTML5 và CSS3
