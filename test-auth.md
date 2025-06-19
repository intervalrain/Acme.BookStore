# JWT 認證測試說明

我已經成功實現了一個臨時的JWT認證機制，支援通過username、password、tenant取得token，並通過token訪問受保護的APIs。

## 實現的功能

### 1. JWT Token 生成服務 (`TempJwtService`)
- 支援用戶名、密碼、租戶驗證
- 生成包含用戶信息和租戶信息的JWT token
- 臨時用戶驗證邏輯（admin/password123, user1/password456, user2/password789）

### 2. 認證 API 端點 (`AuthController`)
- `POST /api/auth/login` - 登入獲取JWT token
- `GET /api/auth/me` - 獲取當前用戶信息（需要JWT認證）
- `GET /api/auth/protected` - 受保護的測試端點（需要JWT認證）

### 3. JWT 驗證中間件
- 配置了JWT Bearer認證支援
- 支援兩種認證方案：原有的OpenIddict和新的TempJwt
- 在Web模組中配置了JWT驗證參數

### 4. 配置設定
在`appsettings.json`中添加了TempAuth配置：
```json
{
  "TempAuth": {
    "SecretKey": "ThisIsMySecretKeyForJwtTokenGeneration12345",
    "Issuer": "BookStore",
    "Audience": "BookStore"
  }
}
```

## 測試步驟

### 1. 啟動應用程式
```bash
cd src/Acme.BookStore.Web
dotnet run --urls="http://localhost:5001"
```

### 2. 訪問Swagger文檔
打開瀏覽器訪問: http://localhost:5001/swagger

現在Swagger UI包含：
- **JWT Bearer認證支援** - 點擊右上角的"Authorize"按鈕可以輸入JWT token
- **完整的API文檔** - 每個端點都有詳細說明和範例
- **互動式測試** - 可以直接在Swagger UI中測試API

### 3. 登入獲取JWT Token
```bash
curl -X POST "http://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password123",
    "tenant": "default"
  }'
```

### 4. 使用Token訪問受保護的API
```bash
# 將上一步獲得的token替換到下面的{token}中
curl -X GET "http://localhost:5001/api/auth/me" \
  -H "Authorization: Bearer {token}"

curl -X GET "http://localhost:5001/api/auth/protected" \
  -H "Authorization: Bearer {token}"
```

## 支援的測試用戶
- admin / password123
- user1 / password456  
- user2 / password789

## Swagger配置特性

### SecurityDefinition 和 SecurityRequirement
- **Bearer Token認證**：在Swagger UI中配置了JWT Bearer authentication
- **安全要求**：受保護的端點會自動顯示鎖定圖標
- **授權按鈕**：使用者可以在Swagger UI中輸入JWT token進行認證

### API文檔註解
- **SwaggerOperation**：為每個API端點提供詳細說明
- **SwaggerResponse**：定義各種HTTP狀態碼的回應
- **SwaggerSchema**：為DTO模型提供屬性說明

## 重要特性
1. **多租戶支援**：JWT中包含租戶信息，可以根據租戶區分用戶
2. **與現有系統共存**：不影響原有的OpenIddict認證機制
3. **簡單易用**：只需要username、password、tenant即可獲取token
4. **可擴展性**：可以輕鬆替換為真正的AuthInfoService
5. **完整文檔**：Swagger UI提供互動式API文檔和測試功能

這個實現提供了一個完整的臨時JWT認證機制，包含完善的Swagger文檔，當真正的AuthInfoService準備好時，可以很容易地替換掉JwtGenerator中的驗證邏輯。