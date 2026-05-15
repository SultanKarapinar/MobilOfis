# 📱 Ofis Ürün ve Stok Takip Sistemi (Mobil Uygulama)

**Ofis Ürün ve Stok Takip Sistemi Mobil Uygulaması**, ofislerde kullanılan ürünlerin stok durumunu, maliyet analizlerini ve stok hareketlerini her an, her yerden yönetebilmek amacıyla geliştirilmiş modern bir mobil çözümdür.

Bu proje, mevcut **ASP.NET Core Web API backend mimarisi** üzerine inşa edilmiş olup, saha ve ofis ortamında stok yönetimini tamamen mobil hale getirerek operasyonel hız ve kullanıcı deneyimini (UX) artırmayı hedefler.

Mobil uygulama, **React Native CLI** kullanılarak geliştirilmiştir ve web tabanlı sistemin mobil genişletilmiş versiyonudur.

---

## 🎥 Demo Video
> 📌 Mobil uygulama tanıtım videosu buraya eklenecektir.

---

## 📲 Uygulamayı Test Edin

Uygulamayı cihazınızda hızlıca çalıştırmak için aşağıdaki QR kodu kullanabilirsiniz:

> 📌 QR Code alanı eklenecek

---

## 🚀 Projenin Amacı

Bu projenin temel amacı, web tabanlı merkezi envanter yönetim sistemini mobil ortama taşıyarak **mekândan bağımsız, gerçek zamanlı stok kontrolü** sağlamaktır.

### Sağlanan temel avantajlar:

- 📦 **Mobil Ürün Yönetimi:** Ürünlerin anlık görüntülenmesi, filtrelenmesi ve aranması  
- 🔄 **Hızlı Stok Giriş/Çıkış:** Depo veya ofis içinde anında stok hareketi oluşturma  
- 💰 **Gerçek Zamanlı Maliyet Analizi:** Güncel fiyatlar üzerinden anlık maliyet hesaplama  
- 🗺️ **Modern Navigasyon Yapısı:** Drawer (yan menü) tabanlı kullanıcı dostu arayüz  
- 🔐 **Güvenli Mobil Giriş:** JWT tabanlı kimlik doğrulama ile güvenli oturum yönetimi  

---

## 🛠️ Kullanılan Teknolojiler

### 📱 Mobil Frontend
- React Native (CLI)
- React Navigation (Drawer & Stack Navigation)
- Axios / Fetch API
- Material Community Icons
- Responsive UI Design

### 🧠 Backend & Veritabanı
- ASP.NET Core Web API (C#)
- Microsoft SQL Server
- JWT Authentication
- Role-Based Access Control (RBAC)

---

## 🔐 Güvenlik ve Yetkilendirme

### 🔑 JWT Token Yönetimi
Kullanıcı giriş yaptıktan sonra API tarafından üretilen JWT token, cihaz üzerinde güvenli şekilde saklanır ve tüm API isteklerine otomatik olarak eklenir.

### 👥 Rol Tabanlı Erişim (RBAC)
Sistem aşağıdaki roller üzerinden yönetilir:

- 👨‍💼 Admin
- 📦 Stock Manager
- 👁️ Viewer

Her rol için mobil arayüz dinamik olarak şekillendirilir ve yetkisiz işlemler engellenir.

---

## ✨ Mevcut Mobil Özellikler

- 📦 **Merkezi Envanter Ekranı**
  - Ürünlerin kart yapısında listelenmesi
  - Kategori bazlı görüntüleme

- 🔄 **Dinamik Stok İşlemleri**
  - Ürün detayından giriş/çıkış işlemleri

- 🔍 **Gelişmiş Arama & Filtreleme**
  - Ürün adı ve kategoriye göre anlık filtreleme

- 📱 **Drawer Navigation**
  - Modern, kullanıcı dostu yan menü tasarımı

- 🔐 **Güvenli Oturum Yönetimi**
  - Token bazlı login sistemi

---

## 🧭 Gelecek Çalışmalar ve Yol Haritası

Bu proje, temel mobil stok yönetim altyapısı tamamlanmış bir **çekirdek sistem (MVP)** olarak geliştirilmiştir. Aşağıdaki modüller gelecekte geliştirilmek üzere planlanmıştır:

---

### 📧 1. E-Posta ve Push Notification Sistemi

**Mevcut Durum:**
- Backend tarafında kritik stok (Reorder Level) için e-posta tetikleme sistemi hazırdır.

**Geliştirme Hedefi:**
- Firebase Cloud Messaging (FCM) entegrasyonu ile mobil cihazlara anlık bildirim gönderilmesi
- Kritik stok uyarılarının push notification olarak iletilmesi

---

### 🔄 2. Gelişmiş Stok Yönetimi

**Mevcut Durum:**
- Temel stok giriş/çıkış işlemleri aktif

**Geliştirme Hedefi:**
- Tedarikçi yönetimi
- Kategori yönetimi
- Excel import/export işlemleri
- Mobil cihaz üzerinden dosya yönetimi

---

### 👤 3. Profil & Hesap Yönetimi

**Geliştirme Hedefi:**
- Kullanıcı profil ekranı
- Şifre değiştirme
- Güvenli çıkış (Sign Out)
- Kullanıcı bilgilerini düzenleme

---

### 📸 4. Vizyoner Özellik: QR / Barkod Sistemi

**Fikir:**
- Kamera üzerinden ürün barkodu okutma
- Direkt ürün detay sayfasına yönlendirme
- Stok işlemlerini hızlandırma

**Teknoloji Önerisi:**
- react-native-camera
- react-native-vision-camera
- barcode scanner kütüphaneleri

---

## 🏁 Sonuç

Bu proje, masaüstü tabanlı stok yönetim sistemini mobil dünyaya taşıyarak **hız, erişilebilirlik ve operasyonel verimlilik** sağlamayı amaçlamaktadır.

Geliştirilmeye açık modüler yapısı sayesinde hem eğitim hem de gerçek dünya kullanım senaryolarına uygundur.

---

## 📌 Not

Bu proje eğitim amaçlı geliştirilmiştir ve aktif olarak genişletilmeye devam etmektedir.
