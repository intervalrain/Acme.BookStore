# EdgeSync.Common.Net

一個 .NET 函式庫，提供標準化的例外處理，遵循 RFC 9457 Problem Details 規範。

## 功能特色

- **EdgeSync 例外處理**：自訂例外自動轉換為適當的 HTTP 回應，並遵循 RFC 9457 Problem Details 格式
- **201 Created 支援**：簡單的屬性式方法來回傳 201 Created 狀態碼
- **框架支援**：同時支援純 ASP.NET Core 和 ABP Framework 應用程式

## 安裝

在專案中加入參考：

```xml
<ProjectReference Include="..\EdgeSync.Common.Net\EdgeSync.Common.Net.csproj" />
```

## 快速開始

### ASP.NET Core 應用程式

```csharp
// 在 Program.cs 或 Startup.cs 中
builder.Services.AddEdgeSyncExceptionHandling();

// 或使用自訂選項
builder.Services.ConfigureEdgeSyncExceptionHandling(options =>
{
    options.ProblemTypeBaseUrl = "https://yourdomain.com/errors/";
    options.IncludeExceptionDetails = builder.Environment.IsDevelopment();
});
```

### ABP Framework 應用程式

```csharp
// 在你的 Module 類別中
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpEdgeSyncExceptionHandling();
}
```

## 使用方式

### 在控制器/應用服務中使用 EdgeSync 例外

```csharp
public class UserAppService : ApplicationService
{
    // 使用屬性回傳 201 Created
    [CreatedResponse]
    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new EdgeSyncBadRequestException("使用者名稱為必填");
        }
        
        var user = new UserDto { Id = 123, Name = input.Name };
        // 狀態碼將自動設為 201 Created
        return user;
    }

    public UserDto GetUser(int id)
    {
        var user = _repository.FindById(id);
        if (user == null)
        {
            // 這將自動回傳 404 並使用 Problem Details
            throw new EdgeSyncNotFoundException($"找不到 ID 為 {id} 的使用者");
        }
        return user;
    }

    public void DeleteUser(int id)
    {
        if (!HasPermission())
        {
            // 這將自動回傳 403 並使用 Problem Details
            throw new EdgeSyncForbiddenException("您沒有刪除使用者的權限");
        }
        // 刪除邏輯...
    }
}
```

## 屬性

| 屬性 | 說明 |
|------|------|
| `[CreatedResponse]` | 自動將 HTTP 狀態碼設為 201 Created，用於成功建立資源時 |

## 例外型別

| 例外 | HTTP 狀態碼 | 使用時機 |
|------|------------|----------|
| `EdgeSyncBadRequestException` | 400 | 無效的請求資料 |
| `EdgeSyncAuthorizationException` | 401 | 使用者未驗證 |
| `EdgeSyncForbiddenException` | 403 | 使用者缺少權限 |
| `EdgeSyncNotFoundException` | 404 | 資源不存在 |
| `EdgeSyncConflictException` | 409 | 重複或衝突的資源 |
| `EdgeSyncUnprocessableEntityException` | 422 | 商業規則驗證失敗 |

## Problem Details 回應格式

所有錯誤回應都遵循 RFC 9457 Problem Details：

```json
{
  "type": "https://example.com/probs/not-found",
  "title": "Not Found",
  "status": 404,
  "detail": "找不到 ID 為 123 的使用者",
  "instance": "/api/users/123"
}
```

## 設定選項

```csharp
services.ConfigureEdgeSyncExceptionHandling(options =>
{
    // Problem Type URI 的基礎 URL
    options.ProblemTypeBaseUrl = "https://api.example.com/errors/";
    
    // 在開發環境中包含堆疊追蹤
    options.IncludeExceptionDetails = environment.IsDevelopment();
    
    // 處理所有例外，不只是 EdgeSync 例外
    options.HandleAllExceptions = true;
    
    // 新增自訂例外對應
    options.CustomProblemTypeMappings[typeof(CustomException)] = "custom-error";
});
```

## 最佳實踐

1. **使用 [CreatedResponse] 來回傳 201 狀態**：建立資源時，使用 `[CreatedResponse]` 屬性來自動設定 201 Created 狀態碼

2. **在所有錯誤情境使用例外**：在商業邏輯的任何地方拋出 EdgeSync 例外，來獲得適當的 HTTP 回應與 RFC 9457 Problem Details

3. **一致的錯誤訊息**：保持錯誤訊息對使用者友善且在整個 API 中保持一致

4. **例外優先的方法**：優先使用例外而非手動狀態碼處理，以獲得更乾淨、更易維護的程式碼

## 進階用法

### 與 ABP Application Service 整合

```csharp
public class BookAppService : ApplicationService
{
    [CreatedResponse]
    public async Task<BookDto> CreateAsync(CreateBookDto input)
    {
        // 驗證
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new EdgeSyncBadRequestException("書名不能為空");
        }

        // 建立書籍
        var book = await _bookRepository.InsertAsync(
            new Book(GuidGenerator.Create(), input.Name)
        );

        var bookDto = ObjectMapper.Map<Book, BookDto>(book);
        
        // 將自動回傳 201 Created
        return bookDto;
    }

    public async Task<BookDto> GetAsync(Guid id)
    {
        var book = await _bookRepository.FindAsync(id);
        if (book == null)
        {
            throw new EdgeSyncNotFoundException($"找不到 ID 為 {id} 的書籍");
        }
        
        return ObjectMapper.Map<Book, BookDto>(book);
    }
}
```

### 自訂 Problem Type 對應

```csharp
// 建立自訂例外
public class BusinessRuleViolationException : Exception
{
    public BusinessRuleViolationException(string message) : base(message) { }
}

// 設定對應
services.ConfigureEdgeSyncExceptionHandling(options =>
{
    options.CustomProblemTypeMappings[typeof(BusinessRuleViolationException)] = 
        "business-rule-violation";
});
```

## 授權

MIT