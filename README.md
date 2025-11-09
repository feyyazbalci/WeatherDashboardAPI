# 🌤️ Weather Dashboard API

Hava durumu verileri çeken, cache'leyen ve otomatik güncelleyen bir .NET Core projesi.

---

## Ne İşe Yarıyor?

Şehirlerin hava durumlarını OpenWeatherMap'ten çekip saklıyor. Redis cache kullanarak hızlı yanıt veriyor. Her 30 dakikada bir arka planda verileri otomatik güncelliyor.

JWT authentication var, admin/user rolleri ile yetkilendirme yapılıyor.

---

## Kullanılan Teknolojiler

- .NET 9.0
- PostgreSQL
- Redis (cache)
- Entity Framework Core
- Docker

---

## Nasıl Çalıştırılır?
```bash
docker-compose up
```

Hepsi bu. 3 container ayağa kalkar:
- API → http://localhost:5000
- PostgreSQL → localhost:5432  
- Redis → localhost:6379
- Adminer (DB UI) → http://localhost:8080

---

## API Nasıl Kullanılır?

Swagger'a git: http://localhost:5000/swagger

### Hızlı Test:
```bash
# 1. Kayıt ol
POST /api/auth/register
{
  "email": "test@test.com",
  "password": "Test123!",
  "role": "User"
}

# 2. Login yap (token al)
POST /api/auth/login
{
  "email": "test@test.com",
  "password": "Test123!"
}

# 3. Şehir ekle (Admin gerekir)
POST /api/cities
Authorization: Bearer <token>
{
  "name": "Istanbul",
  "country": "TR",
  "latitude": 41.0082,
  "longitude": 28.9784
}

# 4. Hava durumunu getir
GET /api/weather/current/1
Authorization: Bearer <token>
```

---

## Önemli Özellikler

### Cache Sistemi
İlk istek API'den veri çeker (~1 saniye). Sonraki istekler Redis'ten gelir (~10ms). 100x daha hızlı.

### Background Service  
Her 30 dakikada tüm şehirlerin hava durumunu otomatik güncelliyor. Manuel güncelleme de yapabilirsin.

### Authentication
- **Admin:** Şehir ekle/sil/güncelle, manuel sync
- **User:** Sadece hava durumu görüntüle

Token 24 saat geçerli.

---

## Adminer'a Bağlanma

http://localhost:8080

- System: PostgreSQL
- Server: postgres
- Username: postgres  
- Password: postgres123
- Database: weatherdb

---

## Faydalı Docker Komutları
```bash
# Başlat
docker-compose up

# Arka planda başlat
docker-compose up -d

# Logları izle
docker-compose logs -f api

# Durdur
docker-compose down

# Redis'e gir
docker exec -it weather-redis redis-cli
127.0.0.1:6379> KEYS *
127.0.0.1:6379> GET city:1:weather
```

---

## Proje Yapısı
```
Controllers/    → API endpoint'leri
Services/       → İş mantığı (auth, city, weather, cache, background)
Repositories/   → Database işlemleri (Repository Pattern + UnitOfWork)
Models/         → Entity'ler (User, City, WeatherRecord)
DTOs/           → API giriş/çıkış şemaları
Helpers/        → JWT helper
```

---

---

## Local'de Çalıştırma (Docker'sız)
```bash
# PostgreSQL ve Redis kur
brew install postgresql redis
brew services start postgresql
brew services start redis

# appsettings.json'ı güncelle (localhost:5432, localhost:6379)

# Çalıştır
dotnet run
```

---

## Sorun mu Var?

**Port 5000 meşgul:**
```bash
pkill -9 dotnet
```

**Redis bağlanamıyor:**
```bash
docker ps | grep redis
docker logs weather-redis
```

**Migration hatası:**
```bash
dotnet ef database update
```

---

## TODO

- [ ] Unit testler
- [ ] Rate limiting middleware
- [ ] Health check endpoint'leri
- [ ] CI/CD pipeline

---

Sorular için issue aç!

**Happy coding!** ☕