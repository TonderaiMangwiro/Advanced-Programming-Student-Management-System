-- Database setup script for Student Information System
-- This script will create the database, table structure, and populate with sample data

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'VT_OGRENCILER')
BEGIN
    CREATE DATABASE VT_OGRENCILER;
END
GO

USE VT_OGRENCILER;
GO

-- Create table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ogrenci' AND xtype='U')
BEGIN
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
END
GO

-- Clear existing data if needed (comment out if you want to keep existing data)
-- DELETE FROM ogrenci;
-- GO

-- Insert sample records
-- Check if data already exists before inserting
IF NOT EXISTS (SELECT * FROM ogrenci)
BEGIN
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('12345678901', 'Ahmet', 'Yılmaz', 'İstanbul', 'Kadıköy', 'Erkek', 1, 1, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('23456789012', 'Ayşe', 'Kaya', 'Ankara', 'Sincan', 'Kadın', 2, 1, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('34567890123', 'Mehmet', 'Demir', 'İzmir', 'Konak', 'Erkek', 0, 0, 1, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('45678901234', 'Fatma', 'Şahin', 'Eskişehir', 'Merkez', 'Kadın', 3, 1, 1, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('56789012345', 'Ali', 'Arslan', 'İstanbul', 'Beşiktaş', 'Erkek', 4, 0, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('67890123456', 'Zeynep', 'Çelik', 'Ankara', 'Gülbaşı', 'Kadın', 1, 1, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('78901234567', 'Mustafa', 'Yıldız', 'İzmir', 'Bornova', 'Erkek', 2, 1, 0, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('89012345678', 'Emine', 'Aydın', 'Eskişehir', 'Odunpazarı', 'Kadın', 3, 0, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('90123456789', 'Hasan', 'Öztürk', 'İstanbul', 'Bakırköy', 'Erkek', 0, 0, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('01234567890', 'Hatice', 'Korkmaz', 'Ankara', 'Polatlı', 'Kadın', 4, 1, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('13579246801', 'Hüseyin', 'Kılıç', 'İzmir', 'Çeşme', 'Erkek', 1, 1, 1, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('24680135792', 'Aysel', 'Aksoy', 'Eskişehir', 'Sivrihisar', 'Kadın', 2, 0, 1, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('36914725803', 'İbrahim', 'Doğan', 'İstanbul', 'Şişli', 'Erkek', 3, 1, 0, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('48026937514', 'Seda', 'Özdemir', 'Ankara', 'Mamak', 'Kadın', 0, 1, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('59137048625', 'Osman', 'Aslan', 'İzmir', 'Karşıyaka', 'Erkek', 4, 0, 0, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('60248159736', 'Sevgi', 'Erdoğan', 'Eskişehir', 'Çifteler', 'Kadın', 1, 0, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('71359260847', 'Emre', 'Koç', 'İstanbul', 'Beyoğlu', 'Erkek', 2, 1, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('82460371958', 'Merve', 'Yıldırım', 'Ankara', 'Beypazarı', 'Kadın', 3, 0, 0, 1);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('93571482069', 'Burak', 'Kurt', 'İzmir', 'Urla', 'Erkek', 0, 1, 1, 0);
    INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema) VALUES ('11223344556', 'Elif', 'Güneş', 'Eskişehir', 'Seyitgazi', 'Kadın', 4, 1, 1, 1);
END
GO

PRINT 'Database setup completed successfully.'; 