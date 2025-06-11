# Gelişmiş Programlama - Öğrenci Bilgi Sistemi

Bu proje, Gelişmiş Programlama dersi kapsamında geliştirilmiş bir Windows Forms uygulamasıdır. Uygulama, öğrenci bilgilerini yönetmek için tasarlanmış bir bilgi sistemidir.

## 📋 Proje Yapısı

- **Adv.Programming - MIDTERM/**: Vize projesi
- **Adv.Programming - FINAL/**: Final projesi (vize projesinin geliştirilmiş versiyonu)

## 🚀 Özellikler

### Vize Projesi Özellikleri:
- Öğrenci bilgileri girişi (Ad, Soyad, TC, İl, İlçe, Cinsiyet)
- Hobi seçenekleri (Müzik, Kitap, Sinema)
- Öğrenci ikonu seçimi
- ListView ile öğrenci listesi görüntüleme
- Farklı görünüm modları (Büyük İkon, Detay, Döşeme, Küçük İkon, Liste)
- Menü ve toolbar kontrolü

### Final Projesi Ek Özellikleri:
- **Veritabanı entegrasyonu** (SQL Server)
- Öğrenci bilgilerini veritabanına kaydetme
- Öğrenci bilgilerini güncelleme
- Öğrenci kaydı silme
- Veritabanından öğrenci listesi çekme
- TC Kimlik numarası doğrulaması
- Duplicate kayıt kontrolü

## 🛠️ Teknolojiler

- **Platform**: .NET Framework
- **UI Framework**: Windows Forms
- **Veritabanı**: SQL Server (LocalDB)
- **Programlama Dili**: C#

## 📦 Gereksinimler

- Visual Studio 2019 veya üzeri
- .NET Framework 4.7.2 veya üzeri
- SQL Server Express LocalDB (Final projesi için)

## 🔧 Kurulum ve Çalıştırma

### Vize Projesi:
1. `Adv.Programming - MIDTERM/Adv.Programming.sln` dosyasını Visual Studio ile açın
2. Projeyi derleyin ve çalıştırın

### Final Projesi:
1. `Adv.Programming - FINAL/Adv.Programming.sln` dosyasını Visual Studio ile açın
2. SQL Server Express LocalDB'nin kurulu olduğundan emin olun
3. `database_setup.sql` dosyasını çalıştırarak veritabanını oluşturun
4. Projeyi derleyin ve çalıştırın

## 🗄️ Veritabanı Kurulumu (Final Projesi)

Final projesi için veritabanı kurulumu:

```sql
-- database_setup.sql dosyasını SQL Server Management Studio'da çalıştırın
-- Veya uygulama ilk çalıştırıldığında otomatik olarak veritabanı oluşturulacaktır
```

Veritabanı bağlantı stringi:
```csharp
Data Source=.\SQLEXPRESS;Initial Catalog=VT_OGRENCILER;Integrated Security=True
```

## 📊 Veritabanı Şeması

```sql
CREATE TABLE ogrenci (
    tc NVARCHAR(11) PRIMARY KEY,
    adi NVARCHAR(50) NOT NULL,
    soyadi NVARCHAR(50) NOT NULL,
    ili NVARCHAR(50) NOT NULL,
    ilcesi NVARCHAR(50) NOT NULL,
    cinsiyet NVARCHAR(10) NOT NULL,
    ikon INT NOT NULL,
    muzik BIT NOT NULL,
    kitap BIT NOT NULL,
    sinema BIT NOT NULL
);
```

## 🎯 Kullanım

1. **Öğrenci Ekleme**: Form alanlarını doldurun ve "Ekle" butonuna tıklayın
2. **Öğrenci Güncelleme**: Listeden bir öğrenci seçin, bilgileri düzenleyin ve "Güncelle" butonuna tıklayın
3. **Öğrenci Silme**: Listeden bir öğrenci seçin ve "Sil" butonuna tıklayın
4. **Listeleme**: "Listele" butonu ile tüm öğrencileri görüntüleyebilirsiniz

## 📝 Notlar

- Öğrenci TC kimlik numaraları 11 haneli olmalıdır
- Her TC kimlik numarası benzersiz olmalıdır
- Final projesinde tüm veriler SQL Server veritabanında saklanır
- Vize projesinde veriler sadece uygulama çalışırken bellekte tutulur

## 🔒 Güvenlik

- Veritabanı bağlantısında Windows Authentication kullanılmaktadır
- Hassas bilgiler kod içerisinde saklanmamaktadır
- TC kimlik numarası doğrulaması yapılmaktadır

## 📸 Ekran Görüntüleri

Uygulama arayüzü ve özellikler hakkında detaylı bilgi için proje klasörlerindeki executable dosyaları çalıştırabilirsiniz.

## 👨‍💻 Geliştirici

Bu proje, Gelişmiş Programlama dersi kapsamında akademik amaçlarla geliştirilmiştir.

## 📄 Lisans

Bu proje eğitim amaçlı olarak geliştirilmiştir ve akademik kullanım için hazırlanmıştır. 