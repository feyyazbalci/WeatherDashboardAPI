# 🌤️ Weather Dashboard API

Hava durumu verilerini takip eden, otomatik güncelleyen ve cache'leyen bir .NET Core API projesi.

OpenWeatherMap API'sinden veri çekiyor, Redis ile cache'liyor ve PostgreSQL'de saklıyor. Tüm sistem Docker ile containerize edilmiş durumda.

---

## 🎯 Neler Var?

### Temel Özellikler
- 🔐 **JWT Authentication** - Token bazlı güvenli giriş sistemi (Admin/User rolleri)
- 🏙️ **Şehir Yönetimi** - Şehir ekle, sil, güncelle, listele
- 🌡️ **Hava Durumu** - Gerçek zamanlı hava durumu verileri
- 📊 **Geçmiş Veriler** - Tarihsel hava durumu kayıtları
- ⚡ **Redis Cache** - Süper hızlı veri erişimi (100x performans artışı!)
- 🤖 **Background Service** - Her 30 dakikada otomatik hava durumu güncelleme
- 🐳 **Docker** - Tek komutla tüm sistem ayağa kalkıyor

### Teknik Stack
- **.NET 9.0** - Modern, performanslı
- **PostgreSQL** - Production-ready veritabanı
- **Redis** - In-memory cache
- **Entity Framework Core** - ORM
- **AutoMapper** - DTO mapping
- **Swagger** - API dokümantasyonu
- **Docker & Docker Compose** - Containerization

---

## 🚀 Hızlı Başlangıç

### Gereksinimler
- Docker Desktop (Mac/Windows/Linux)
- That's it! 🎉

### Kurulum
```bash