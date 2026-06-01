
# 📱 Ofis Ürün ve Stok Takip Sistemi (Mobil Uygulama)

Ofis Ürün ve Stok Takip Sistemi Mobil Uygulaması; ofislerde kullanılan ürünlerin stok durumunu, maliyet analizlerini ve stok hareketlerini her an, her yerden yönetebilmek amacıyla ortak eğitim programı kapsamında geliştirilmiş bir mobil uygulamadır.

Bu proje, mevcut ASP.NET Core Web API backend yapısını temel alarak, saha içinde veya ofis ortamında stok süreçlerini tamamen mobilize etmek, operasyonel hızı ve taşınabilir kullanıcı deneyimini (UX) artırmak amacıyla React Native CLI kullanılarak geliştirilmiştir.

---

## 🎥 Demo Video
https://youtube.com/shorts/0uoveQLmdP0?feature=share

---

## 📲 Uygulamayı Test Edin

<img width="450" height="450" alt="mobil_uyg" src="https://github.com/user-attachments/assets/cbe2fbae-7fd3-4c67-b32e-891968dcfb15" />


---

## 🚀 Projenin Amacı ve Mobil Avantajları

Projenin temel amacı, web panelindeki merkezi envanter yönetimini cebe taşıyarak masa bağımsız bir kontrol mekanizması oluşturmaktır. Mobil uygulama ile sağlanan çözümler:

- 📦 **Mobil Ürün Yönetimi:** Saha içinde ürünlerin anlık listelenmesi ve aranması  
- 🔄 **Hızlı Stok Giriş ve Çıkış Takibi:** Depo veya ofis içinde anında stok hareketi oluşturma  
- 💰 **Hareket Halinde Maliyet Hesaplama:** Güncel alış fiyatları üzerinden anlık maliyet analizi  
- 🗺️ **Gelişmiş Navigasyon:** Mobil uyumlu özel Drawer (Yan Menü) yapısı  
- 🔐 **Mobil Güvenli Giriş:** JWT token entegrasyonu ile cihaz tabanlı güvenli oturum yönetimi  

---

## 🛠️ Kullanılan Teknolojiler

### 📱 Mobil Frontend
- React Native (Native CLI)
- React Navigation (Drawer & Stack)
- Material Community Icons
- Responsive UI Design
- Axios / Fetch API

### 🧠 Backend & Veritabanı
- C# / ASP.NET Core Web API
- JWT Authentication
- Microsoft SQL Server

---

## 🔐 Güvenlik ve Mobil Yetkilendirme

### JWT Token Saklama
Kullanıcı giriş yaptıktan sonra API'den dönen JWT Token, mobil cihaz üzerinde güvenli bir şekilde saklanır ve sonraki tüm API isteklerine (Header) otomatik olarak eklenir.

---

### 🔑 Rol Tabanlı Erişim Kontrolü (RBAC)
Giriş yapan kullanıcının rolüne göre (Admin, StockManager, Viewer) mobil arayüzdeki menüler ve işlem butonları dinamik olarak kısıtlanır veya yetkilendirilir.

---

## ✨ Tamamlanan Mobil Özellikler

- ✅ Merkezi Envanter Ekranı: Tüm ürünlerin kategorize edilerek kart yapısında listelenmesi  
- ✅ Dinamik Stok Yönetimi: Ürün detay sayfalarından hızlı stok giriş/çıkış işlemleri  
- ✅ Gelişmiş Arama & Filtreleme: Ürün adı ve kategoriye göre arama  
- ✅ Özel Drawer Menü: Kullanıcı dostu ve responsive yan menü tasarımı  

---

## 📌 Gelecek Çalışmalar ve Yol Haritası (Stajyer Arkadaşlarımın Dikkatine 🚀)

Ortak eğitim programı (staj) süremin sonuna gelmiş bulunmaktayım. Projenin mobil dönüşüm mimarisini kurup temel stok yönetim ekranlarını tamamladım. Projeyi benden sonra devralacak stajyer arkadaşların sistemi daha da geliştirmesi için aşağıdaki modüller planlanmıştır:

---

### 📧 1. E-Posta ve Anlık Bildirim (Push Notification) Sistemi

**Mevcut Durum:**  
Backend’de ReorderLevel (kritik stok) altına düşen ürünler için mail tetikleme altyapısı hazırdır.

**Geliştirme Hedefi:**  
Firebase Cloud Messaging (FCM) entegrasyonu ile kritik stok bildirimlerinin mobil cihazlara anlık push notification olarak gönderilmesi.

---

### 🔄 2. Detaylı Stok Yönetimi Geliştirmeleri

**Mevcut Durum:**  
Temel stok giriş/çıkış işlemleri çalışmaktadır.

**Geliştirme Hedefi:**
- Tedarikçi yönetimi
- Kategori yönetimi
- Excel import/export işlemleri
- Mobil dosya indirme/yükleme desteği

---

### 👤 3. Profil ve Ayarlar Ekranı

**Mevcut Durum:**  
Kullanıcı bilgileri JWT üzerinden alınmaktadır.

**Geliştirme Hedefi:**
- Profil ekranı
- Şifre değiştirme
- Güvenli çıkış (Sign Out)
- Kullanıcı bilgilerinin güncellenmesi

---

### 📸 4. Bonus: QR / Barkod Tarayıcı Entegrasyonu

**Fikir:**  
React Native kamera kütüphaneleri (react-native-camera / barcode scanner) kullanılarak ürün barkodlarının okutulması ve direkt ürün detayına yönlendirme yapılması.

Bu özellik sistemin hızını ve kullanıcı deneyimini ciddi şekilde artıracaktır.
