# JWT 認證調試指南

## 可能的401錯誤原因

### 1. 檢查API路由
ABP自動生成的API路由可能是：
- `POST /api/app/auth/login` (而不是 `/api/auth/login`)
- `GET /api/app/auth/current-user` 
- `GET /api/app/auth/protected-data`

### 2. 檢查JWT Token格式
確保生成的token包含正確的Claims和格式

### 3. 檢查認證方案
可能需要使用默認的Bearer認證方案而不是指定TempJwt

## 調試步驟

### 步驟1：啟動應用程式並檢查Swagger
```bash
cd src/Acme.BookStore.Web
dotnet run --urls="http://localhost:5001"
```

訪問：http://localhost:5001/swagger
查看實際的API路由

### 步驟2：測試登入
```bash
# 測試ABP自動生成的路由
curl -X POST "http://localhost:5001/api/app/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password123",
    "tenant": "default"
  }'
```

### 步驟3：測試JWT Token
```bash
# 將獲得的token替換到這裡
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# 測試受保護的端點
curl -X GET "http://localhost:5001/api/app/auth/current-user" \
  -H "Authorization: Bearer $TOKEN"
```

### 步驟4：檢查認證方案
如果仍然401，嘗試移除指定的認證方案：

在AuthAppService中將：
```csharp
[Authorize(AuthenticationSchemes = "TempJwt")]
```

改為：
```csharp
[Authorize]
```

這樣會使用默認的認證方案，可能更容易與ABP整合。

## 常見問題解決

1. **路由問題**：ABP會自動將`AuthAppService`映射到`/api/app/auth/`路徑
2. **認證方案問題**：可能需要使用Bearer而不是TempJwt
3. **Claims問題**：確保JWT包含ABP需要的Claims
4. **中間件順序問題**：確保認證中間件在正確位置

## 檢查日誌
啟動應用程式時查看控制台輸出，特別是：
- "JWT Authentication failed: ..."
- "JWT Token validated for user: ..."