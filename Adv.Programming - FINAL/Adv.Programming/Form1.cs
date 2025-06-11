using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // SQL Server için
using System.IO;

namespace ARASINAV
{
    public partial class Form1 : Form
    {
        // Veritabanı bağlantı stringi - kendi ortamınıza göre değiştirin
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=VT_OGRENCILER;Integrated Security=True";
        private SqlConnection connection;
        private int selectedStudentId = -1; // Seçili öğrencinin ID'si
        private string selectedStudentTc = ""; // Seçili öğrencinin TC'si

        public Form1()
        {
            InitializeComponent();

            // Veritabanı bağlantısını oluştur
            connection = new SqlConnection(connectionString);

            // Bağlantıyı test et
            TestConnection();
            
            // Form boyutu değiştiğinde sütunları yeniden ayarla
            this.Resize += Form1_Resize;
        }

        private void TestConnection()
        {
            try
            {
                // Veritabanı bağlantısını aç
                connection.Open();

                // Bağlantı başarılı mesajı
                toolStripStatusLabel1.Text = "Veritabanı bağlantısı başarılı.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı kurulamadı: " + ex.Message +
                               "\nLütfen SQL Server'da VT_OGRENCILER veritabanının ve ogrenci tablosunun oluşturulduğundan emin olun.",
                               "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void LoadAllStudents()
        {
            try
            {
                // ListView'i temizle
                listView1.Items.Clear();
                
                // Veritabanı bağlantısını kontrol et ve aç
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                
                // Tüm öğrencileri getiren sorgu
                string query = "SELECT * FROM ogrenci ORDER BY adi, soyadi";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Veritabanından gelen verileri al
                            string tc = reader.GetString(0);
                            string adi = reader.GetString(1);
                            string soyadi = reader.GetString(2);
                            string ili = reader.GetString(3);
                            string ilcesi = reader.GetString(4);
                            string cinsiyet = reader.GetString(5);
                            int ikonIndex = reader.GetInt32(6);
                            bool muzik = reader.GetBoolean(7);
                            bool kitap = reader.GetBoolean(8);
                            bool sinema = reader.GetBoolean(9);
                            
                            // ListView'e ekle
                            ListViewItem item = new ListViewItem(tc);
                            item.ImageIndex = ikonIndex;
                            item.SubItems.Add(adi);
                            item.SubItems.Add(soyadi);
                            item.SubItems.Add(ili);
                            item.SubItems.Add(ilcesi);
                            item.SubItems.Add(cinsiyet);
                            item.SubItems.Add(muzik ? "Evet" : "Hayır");
                            item.SubItems.Add(kitap ? "Evet" : "Hayır");
                            item.SubItems.Add(sinema ? "Evet" : "Hayır");
                            
                            // Öğrenci TC'sini Tag özelliğinde sakla
                            item.Tag = tc;
                            
                            listView1.Items.Add(item);
                        }
                    }
                }
                
                // Bağlantıyı kapat
                connection.Close();
                
                // İşlem başarılı mesajı
                toolStripStatusLabel1.Text = "Tüm kayıtlar başarıyla listelendi.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğrenciler yüklenirken hata oluştu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void AddStudent()
        {
            try
            {
                // Gerekli kontrolleri yap
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) ||
                    comboBox1.SelectedItem == null ||
                    listBox1.SelectedItem == null)
                {
                    MessageBox.Show("Bilgilerde eksiklik bulunmaktadır!", "Hata!", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // TC Kimlik doğrulaması yap - 11 haneli olmalı
                string tc = textBox3.Text.Trim();
                if (tc.Length != 11 || !long.TryParse(tc, out _))
                {
                    MessageBox.Show("TC Kimlik numarası 11 haneli rakam olmalıdır!", "Hata!",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Öğrenci bilgilerini al
                string ad = textBox1.Text;
                string soyad = textBox2.Text;
                string il = comboBox1.SelectedItem.ToString();
                string ilce = listBox1.SelectedItem.ToString();

                // Cinsiyet bilgisini al
                string cinsiyet = radioButton1.Checked ? "Erkek" : "Kadın";

                // Hobi bilgilerini al
                bool muzik = checkBox1.Checked;
                bool kitap = checkBox2.Checked;
                bool sinema = checkBox3.Checked;

                // Seçilen ikonu belirle
                int ikonIndex = 0;
                if (radioButton3.Checked) ikonIndex = 1;
                else if (radioButton4.Checked) ikonIndex = 2;
                else if (radioButton5.Checked) ikonIndex = 3;
                else if (radioButton6.Checked) ikonIndex = 4;
                
                // Veritabanı bağlantısını aç
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                
                // TC kontrolü yap - aynı TC ile kayıt varsa uyar
                string checkTcQuery = "SELECT COUNT(*) FROM ogrenci WHERE tc = @Tc";
                using (SqlCommand checkCommand = new SqlCommand(checkTcQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Tc", tc);
                    int count = (int)checkCommand.ExecuteScalar();
                    
                    if (count > 0)
                    {
                        MessageBox.Show("Bu TC Kimlik numarası ile kayıtlı bir öğrenci zaten var!",
                                       "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        connection.Close();
                        return;
                    }
                }
                
                // Ekleme sorgusu
                string query = @"INSERT INTO ogrenci (tc, adi, soyadi, ili, ilcesi, cinsiyet, ikon, muzik, kitap, sinema)
                                VALUES (@Tc, @Adi, @Soyadi, @Ili, @Ilcesi, @Cinsiyet, @Ikon, @Muzik, @Kitap, @Sinema)";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@Tc", tc);
                    command.Parameters.AddWithValue("@Adi", ad);
                    command.Parameters.AddWithValue("@Soyadi", soyad);
                    command.Parameters.AddWithValue("@Ili", il);
                    command.Parameters.AddWithValue("@Ilcesi", ilce);
                    command.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                    command.Parameters.AddWithValue("@Ikon", ikonIndex);
                    command.Parameters.AddWithValue("@Muzik", muzik);
                    command.Parameters.AddWithValue("@Kitap", kitap);
                    command.Parameters.AddWithValue("@Sinema", sinema);
                    
                    // Sorguyu çalıştır
                    command.ExecuteNonQuery();
                }
                
                // Bağlantıyı kapat
                connection.Close();
                
                // Başarı mesajı
                toolStripStatusLabel1.Text = "Öğrenci başarıyla eklendi.";
                
                // Formu temizle
                ClearForm();
                
                // Listeyi güncelle - bağlantı kapalı olduğundan emin olarak
                if (connection.State == ConnectionState.Closed)
                {
                    LoadAllStudents();
                }
                else
                {
                    MessageBox.Show("Veritabanı bağlantısı kapatılamadı. Lütfen uygulamayı yeniden başlatın.",
                                  "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğrenci eklenirken hata oluştu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void UpdateStudent()
        {
            try
            {
                // Seçili öğrenci var mı kontrol et
                if (string.IsNullOrEmpty(selectedStudentTc))
                {
                    MessageBox.Show("Lütfen güncellenecek bir öğrenci seçin!", "Uyarı",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Gerekli kontrolleri yap
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text) ||
                    comboBox1.SelectedItem == null ||
                    listBox1.SelectedItem == null)
                {
                    MessageBox.Show("Bilgilerde eksiklik bulunmaktadır!", "Hata!", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // TC Kimlik doğrulaması yap - 11 haneli olmalı
                string tc = textBox3.Text.Trim();
                if (tc.Length != 11 || !long.TryParse(tc, out _))
                {
                    MessageBox.Show("TC Kimlik numarası 11 haneli rakam olmalıdır!", "Hata!",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Öğrenci bilgilerini al
                string ad = textBox1.Text;
                string soyad = textBox2.Text;
                string il = comboBox1.SelectedItem.ToString();
                string ilce = listBox1.SelectedItem.ToString();

                // Cinsiyet bilgisini al
                string cinsiyet = radioButton1.Checked ? "Erkek" : "Kadın";

                // Hobi bilgilerini al
                bool muzik = checkBox1.Checked;
                bool kitap = checkBox2.Checked;
                bool sinema = checkBox3.Checked;

                // Seçilen ikonu belirle
                int ikonIndex = 0;
                if (radioButton3.Checked) ikonIndex = 1;
                else if (radioButton4.Checked) ikonIndex = 2;
                else if (radioButton5.Checked) ikonIndex = 3;
                else if (radioButton6.Checked) ikonIndex = 4;
                
                // Veritabanı bağlantısını kontrol et ve aç
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                
                // TC değiştiyse kontrol et - aynı TC ile başka kayıt varsa uyar
                if (tc != selectedStudentTc)
                {
                    string checkTcQuery = "SELECT COUNT(*) FROM ogrenci WHERE tc = @Tc";
                    using (SqlCommand checkCommand = new SqlCommand(checkTcQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Tc", tc);
                        int count = (int)checkCommand.ExecuteScalar();
                        
                        if (count > 0)
                        {
                            MessageBox.Show("Bu TC Kimlik numarası ile kayıtlı bir öğrenci zaten var!",
                                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            connection.Close();
                            return;
                        }
                    }
                }
                
                // Güncelleme sorgusu
                string query = @"UPDATE ogrenci 
                                SET tc = @NewTc, adi = @Adi, soyadi = @Soyadi, ili = @Ili, ilcesi = @Ilcesi, 
                                    cinsiyet = @Cinsiyet, ikon = @Ikon, muzik = @Muzik, kitap = @Kitap, 
                                    sinema = @Sinema
                                WHERE tc = @OldTc";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@OldTc", selectedStudentTc);
                    command.Parameters.AddWithValue("@NewTc", tc);
                    command.Parameters.AddWithValue("@Adi", ad);
                    command.Parameters.AddWithValue("@Soyadi", soyad);
                    command.Parameters.AddWithValue("@Ili", il);
                    command.Parameters.AddWithValue("@Ilcesi", ilce);
                    command.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
                    command.Parameters.AddWithValue("@Ikon", ikonIndex);
                    command.Parameters.AddWithValue("@Muzik", muzik);
                    command.Parameters.AddWithValue("@Kitap", kitap);
                    command.Parameters.AddWithValue("@Sinema", sinema);
                    
                    // Sorguyu çalıştır
                    command.ExecuteNonQuery();
                }
                
                // Bağlantıyı kapat
                connection.Close();
                
                // Güncelleme başarılı mesajı
                MessageBox.Show("Güncelleme işlemi başarı ile gerçekleşti.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Durum çubuğu mesajı
                toolStripStatusLabel1.Text = "Öğrenci başarıyla güncellendi.";
                
                // Formu temizle ve seçili öğrenciyi sıfırla
                ClearForm();
                selectedStudentTc = "";
                
                // Listeyi güncelle - bağlantı kapalı olduğundan emin olarak
                if (connection.State == ConnectionState.Closed)
                {
                    LoadAllStudents();
                }
                else
                {
                    MessageBox.Show("Veritabanı bağlantısı kapatılamadı. Lütfen uygulamayı yeniden başlatın.",
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğrenci güncellenirken hata oluştu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void DeleteStudent()
        {
            try
            {
                // ListView'den seçili bir öğe var mı kontrol et
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Lütfen silinecek bir öğrenci seçin!", "Uyarı",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Seçili öğrencinin TC'sini al
                string studentTc = listView1.SelectedItems[0].Tag.ToString();
                
                // Kullanıcıya silme işlemini onaylat
                DialogResult result = MessageBox.Show($"{studentTc} TC kimlik numaralı kaydı silmek istediğinize emin misiniz?",
                                                     "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.Yes)
                {
                    // Veritabanı bağlantısını kontrol et ve aç
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    
                    // Silme sorgusu
                    string query = "DELETE FROM ogrenci WHERE tc = @Tc";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekle
                        command.Parameters.AddWithValue("@Tc", studentTc);
                        
                        // Sorguyu çalıştır
                        command.ExecuteNonQuery();
                    }
                    
                    // Bağlantıyı kapat
                    connection.Close();
                    
                    // Başarı mesajı
                    toolStripStatusLabel1.Text = "Öğrenci başarıyla silindi.";
                    
                    // Listeyi güncelle - bağlantı kapalı olduğundan emin olarak
                    if (connection.State == ConnectionState.Closed)
                    {
                        LoadAllStudents();
                    }
                    else
                    {
                        MessageBox.Show("Veritabanı bağlantısı kapatılamadı. Lütfen uygulamayı yeniden başlatın.",
                                     "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    // Formu temizle
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Öğrenci silinirken hata oluştu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form ikonunu ayarla
            try
            {
                this.Icon = new Icon("../../Icons/Application.ico");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uygulama ikonu yüklenirken hata oluştu: " + ex.Message,
                                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Şehirleri comboBox'a ekle
            comboBox1.Items.Add("Ankara");
            comboBox1.Items.Add("Eskişehir");
            comboBox1.Items.Add("İstanbul");
            comboBox1.Items.Add("İzmir");

            // comboBox1'in düzenleme seçeneğini kapalı yap
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // ListView'nin sütunlarını oluştur
            listView1.Columns.Clear(); // Önceki sütunları temizle
            listView1.Columns.Add("TC", 120);
            listView1.Columns.Add("Adı", 100);
            listView1.Columns.Add("Soyadı", 100);
            listView1.Columns.Add("İli", 90);
            listView1.Columns.Add("İlçesi", 100);
            listView1.Columns.Add("Cinsiyet", 70);
            listView1.Columns.Add("Müzik", 65);
            listView1.Columns.Add("Kitap", 65);
            listView1.Columns.Add("Sinema", 65);
            
            // ListView özellikleri
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.View = View.Details;
            
            // Form yüklendiğinde sütunları tam genişliğe ayarla
            AdjustListViewColumnWidths();
            
            // ListView'in sütunlarının tamamen görünür olması için 
            // yatay kaydırma özelliğini etkinleştir
            listView1.Scrollable = true;
            
            // groupBox5 (ListView'ı içeren) boyutu değiştiğinde sütunları yeniden ayarla
            groupBox5.SizeChanged += (s, args) => AdjustListViewColumnWidths();

            // Görünüm türü comboBox'a görünüm tiplerini ekle
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Büyük İkon (Large Icon)");
            comboBox2.Items.Add("Detay (Details)");
            comboBox2.Items.Add("Döşeme(Tile)");
            comboBox2.Items.Add("Küçük İkon (Small Icon)");
            comboBox2.Items.Add("Liste (List)");

            // comboBox2'nin düzenleme seçeneğini kapalı yap
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            // Varsayılan olarak "Detay" görünümünü seç
            comboBox2.SelectedIndex = 1; // Detay (Details)

            // Erkek radyo dümesini varsayılan olarak seç
            radioButton1.Checked = true;

            // İkon 1 radyo dümesini varsayılan olarak seç
            radioButton3.Checked = true;

            // Durum çubuğuna hoşgeldin mesajı ekle
            toolStripStatusLabel1.Text = "Hoşgeldiniz!";

            // ImageList'leri ayarla
            imageList1.ImageSize = new Size(20, 20); // Küçük ikon boyutu
            imageList1.ColorDepth = ColorDepth.Depth32Bit;

            imageList2.ImageSize = new Size(32, 32); // Büyük ikon boyutu
            imageList2.ColorDepth = ColorDepth.Depth32Bit;

            try
            {
                // Resimleri ImageList'e ekle
                // Resimleri temizleyelim ve yeniden ekleyelim
                imageList1.Images.Clear();
                imageList2.Images.Clear();

                // Küçük ikonlar (Small ImageList)
                imageList1.Images.Add(Image.FromFile("../../Icons/person1-small.jpg"));
                imageList1.Images.Add(Image.FromFile("../../Icons/person2-small.jpg"));
                imageList1.Images.Add(Image.FromFile("../../Icons/person3-small.jpg"));
                imageList1.Images.Add(Image.FromFile("../../Icons/person4-small.png"));

                // Büyük ikonlar (Large ImageList)
                imageList2.Images.Add(Image.FromFile("../../Icons/person1-large.jpg"));
                imageList2.Images.Add(Image.FromFile("../../Icons/person2-large.jpg"));
                imageList2.Images.Add(Image.FromFile("../../Icons/person3-large.jpg"));
                imageList2.Images.Add(Image.FromFile("../../Icons/person4-large.png"));

                // ToolStrip butonlarına ikonları ata
                toolStripButton1.Image = Image.FromFile("../../Icons/Toolbar-ClearStudentInfo.ico");
                toolStripButton2.Image = Image.FromFile("../../Icons/Toolbar-ClearStudentList.ico");
                toolStripButton3.Image = Image.FromFile("../../Icons/Toolbar-About.ico");

                // PictureBox'lara ikonları ata
                pictureBox1.Image = Image.FromFile("../../Icons/person1-small.jpg");
                pictureBox2.Image = Image.FromFile("../../Icons/person2-small.jpg");
                pictureBox3.Image = Image.FromFile("../../Icons/person3-small.jpg");
                pictureBox4.Image = Image.FromFile("../../Icons/person4-small.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("İkonlar yüklenirken hata oluştu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ImageList'leri ListView'e ata
            listView1.SmallImageList = imageList1;
            listView1.LargeImageList = imageList2;

            // ListView'deki tıklama olayını ekle - öğrenci seçimi için
            listView1.ItemSelectionChanged += ListView1_ItemSelectionChanged;

            // Uygulamayı başlatırken tüm öğrencileri yükle
            LoadAllStudents();
        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                try
                {
                    // Seçili öğrencinin TC'sini al
                    selectedStudentTc = e.Item.Tag.ToString();
                    
                    // Veritabanı bağlantısını kontrol et ve aç
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    
                    // Seçili öğrenciyi getiren sorgu
                    string query = "SELECT * FROM ogrenci WHERE tc = @Tc";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekle
                        command.Parameters.AddWithValue("@Tc", selectedStudentTc);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Veritabanından gelen verileri al
                                string tc = reader.GetString(0);
                                string adi = reader.GetString(1);
                                string soyadi = reader.GetString(2);
                                string ili = reader.GetString(3);
                                string ilcesi = reader.GetString(4);
                                string cinsiyet = reader.GetString(5);
                                int ikonIndex = reader.GetInt32(6);
                                bool muzik = reader.GetBoolean(7);
                                bool kitap = reader.GetBoolean(8);
                                bool sinema = reader.GetBoolean(9);
                                
                                // Formdaki kontrollere değerleri yükle
                                textBox3.Text = tc;
                                textBox3.ReadOnly = true; // TC kimlik numarası alanını salt okunur yap
                                textBox1.Text = adi;
                                textBox2.Text = soyadi;
                                
                                // İli seç
                                comboBox1.SelectedItem = ili;
                                
                                // İlçeyi seç
                                if (listBox1.Items.Contains(ilcesi))
                                    listBox1.SelectedItem = ilcesi;
                                
                                // Cinsiyeti seç
                                radioButton1.Checked = (cinsiyet == "Erkek");
                                radioButton2.Checked = (cinsiyet == "Kadın");
                                
                                // Hobileri seç
                                checkBox1.Checked = muzik;
                                checkBox2.Checked = kitap;
                                checkBox3.Checked = sinema;
                                
                                // İkonu seç
                                switch (ikonIndex)
                                {
                                    case 1: radioButton3.Checked = true; break;
                                    case 2: radioButton4.Checked = true; break;
                                    case 3: radioButton5.Checked = true; break;
                                    case 4: radioButton6.Checked = true; break;
                                }
                            }
                        }
                    }
                    
                    // Bağlantıyı kapat
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Öğrenci bilgileri yüklenirken hata oluştu: " + ex.Message,
                                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // İlçe listBox'ını temizle
            listBox1.Items.Clear();

            // comboBox1'de seçili bir şehr yoksa metottan çık
            if (comboBox1.SelectedItem == null)
                return;

            // Seçilen şehire göre ilçeleri ekle
            string selectedCity = comboBox1.SelectedItem.ToString();

            if (selectedCity == "Ankara")
            {
                listBox1.Items.Add("Ankara");
                listBox1.Items.Add("Beypazarı");
                listBox1.Items.Add("Polatlı");
                listBox1.Items.Add("Gülbaşı");
                listBox1.Items.Add("Sincan");
                listBox1.Items.Add("Mamak");
            }
            else if (selectedCity == "Eskişehir")
            {
                listBox1.Items.Add("Alpu");
                listBox1.Items.Add("Çifteler");
                listBox1.Items.Add("Merkez");
                listBox1.Items.Add("Odunpazarı");
                listBox1.Items.Add("Sancakaya");
                listBox1.Items.Add("Seyitgazi");
                listBox1.Items.Add("Sivrihisar");
            }
            else if (selectedCity == "İstanbul")
            {
                listBox1.Items.Add("Beşiktaş");
                listBox1.Items.Add("Bakırköy");
                listBox1.Items.Add("Beyoğlu");
                listBox1.Items.Add("Beylikdüzü");
                listBox1.Items.Add("Eyüp");
                listBox1.Items.Add("Kadıköy");
                listBox1.Items.Add("Şişli");
                listBox1.Items.Add("Şekerpaşa");
                listBox1.Items.Add("Zeytinburnu");
            }
            else if (selectedCity == "İzmir")
            {
                listBox1.Items.Add("Bornova");
                listBox1.Items.Add("Çeşme");
                listBox1.Items.Add("Dikili");
                listBox1.Items.Add("Foça");
                listBox1.Items.Add("Karşıyaka");
                listBox1.Items.Add("Konak");
                listBox1.Items.Add("Torbalı");
                listBox1.Items.Add("Urla");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox2'de seçili bir şehr yoksa metottan çık
            if (comboBox2.SelectedIndex == -1)
                return;

            // Görünüm türünü seçilen değere göre değiştir
            switch (comboBox2.SelectedIndex)
            {
                case 0: // Büyük İkon (Large Icon)
                    listView1.View = View.LargeIcon;
                    break;
                case 1: // Detay (Details)
                    listView1.View = View.Details;
                    break;
                case 2: // Döşeme (Tile)
                    listView1.View = View.Tile;
                    break;
                case 3: // Küçük İkon (Small Icon)
                    listView1.View = View.SmallIcon;
                    break;
                case 4: // Liste (List)
                    listView1.View = View.List;
                    break;
            }
        }

        // Listeye Ekle butonuna tıklandığında
        private void button1_Click(object sender, EventArgs e)
        {
            AddStudent();
        }

        // Sil butonuna tıklandığında
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteStudent();
        }

        // Güncelle butonuna tıklandığında
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateStudent();
        }

        // Tüm Kayıtları Listele butonuna tıklandığında
        private void buttonListAll_Click(object sender, EventArgs e)
        {
            LoadAllStudents();
            toolStripStatusLabel1.Text = "Tüm kayıtlar başarıyla listelendi.";
        }

        // Menü - Bilgileri Temizle
        private void bilgileriTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        // Menü - Listeyi Temizle
        private void listeyiTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearList();
        }

        // Menü - Çıkış
        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Menü - Hakkında
        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        // ToolStrip butonları
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearForm(); // Bilgileri temizle
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ClearList(); // Listeyi temizle
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog(); // Hakkında formunu göster
        }

        // ToolStrip ItemClicked olayını işle
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Bu metot toolStrip'teki herhangi bir öğeye tıklandığında çalışır
            // Şu anda özel bir işlem yapmasına gerek yok
        }

        // MenuStrip ItemClicked olayını işle
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Bu metot menuStrip'teki herhangi bir öğeye tıklandığında çalışır
            // Şu anda özel bir işlem yapmasına gerek yok
        }

        // Öğrenci bilgilerini temizleme metodu
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox3.ReadOnly = false; // Form temizlendiğinde TC kimlik numarası alanını düzenlenebilir yap
            comboBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            radioButton1.Checked = true; // Erkek seçili
            radioButton3.Checked = true; // İkon 1 seçili
            checkBox1.Checked = false;   // Müzik seçili değil
            checkBox2.Checked = false;   // Kitap seçili değil
            checkBox3.Checked = false;   // Sinema seçili değil
        }

        // Öğrenci listesini temizleme metodu
        private void ClearList()
        {
            // Sadece ListView'ı temizle, veritabanına dokunma
            listView1.Items.Clear();
            
            // Bilgilendirme mesajı
            toolStripStatusLabel1.Text = "Liste görünümü temizlendi. Tüm kayıtları görüntülemek için 'Tüm Kayıtları Listele' butonunu kullanabilirsiniz.";
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        // ListView sütunlarını tam genişlikte ayarlayan yeni metod
        private void AdjustListViewColumnWidths()
        {
            // ListView'ın görünür genişliği
            int totalWidth = listView1.ClientSize.Width - 4; // 4 piksel kenar boşluğu için çıkardık
            
            if (totalWidth <= 0 || listView1.Columns.Count == 0)
                return;
                
            // Sütun genişliklerinin toplamını hesapla
            int currentTotalWidth = 0;
            foreach (ColumnHeader column in listView1.Columns)
            {
                currentTotalWidth += column.Width;
            }
            
            // Sütunların genişliğini mevcut genişliğe oranla ayarla
            float ratio = (float)totalWidth / currentTotalWidth;
            
            // TC sütununu önce ayarla, daha geniş olması için
            listView1.Columns[0].Width = Math.Max(120, (int)(listView1.Columns[0].Width * ratio));
            
            // Toplam genişliği yeniden hesapla ve kalan boşluğu diğer sütunlar arasında dağıt
            int remainingWidth = totalWidth - listView1.Columns[0].Width;
            int remainingColumns = listView1.Columns.Count - 1;
            
            if (remainingColumns > 0 && remainingWidth > 0)
            {
                // TC sütunundan sonraki sütunları eşit dağıt
                int columnWidth = remainingWidth / remainingColumns;
                
                for (int i = 1; i < listView1.Columns.Count; i++)
                {
                    listView1.Columns[i].Width = columnWidth;
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Form boyutu değiştiğinde ListView sütunlarını yeniden ayarla
            if (this.WindowState != FormWindowState.Minimized)
            {
                AdjustListViewColumnWidths();
            }
        }
    }
}