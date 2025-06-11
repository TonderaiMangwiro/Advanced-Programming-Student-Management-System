using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARASINAV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                MessageBox.Show("Uygulama ikonu yüklenirken hata oluþtu: " + ex.Message,
                                "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Ýli comboBox'ýna þehirleri ekle
            comboBox1.Items.Add("Ankara");
            comboBox1.Items.Add("Eskiþehir");
            comboBox1.Items.Add("Ýstanbul");
            comboBox1.Items.Add("Ýzmir");

            // comboBox1'in düzenleme seçeneði kapalý
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // ListView'ýn sütunlarýný oluþtur
            listView1.Columns.Clear(); // Önceki sütunlarý temizle
            listView1.Columns.Add("Adý", 80);
            listView1.Columns.Add("Soyadý", 80);
            listView1.Columns.Add("Ýli", 70);
            listView1.Columns.Add("Ýlçesi", 80);
            listView1.Columns.Add("Cinsiyet", 60);
            listView1.Columns.Add("Müzik", 60);
            listView1.Columns.Add("Kitap", 60);
            listView1.Columns.Add("Sinema", 60);

            // ListView özellikleri
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.View = View.Details;

            // ListView'in sütunlarýnýn tamamen görünür olmasý için 
            // yatay kaydýrma özelliðini etkinleþtir
            listView1.Scrollable = true;

            // Görünüm tipi comboBox'ýna görünüm tiplerini ekle
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Büyük Ýkon (Large Icon)");
            comboBox2.Items.Add("Detay (Details)");
            comboBox2.Items.Add("Döþeme(Tile)");
            comboBox2.Items.Add("Küçük Ýkon (Small Icon)");
            comboBox2.Items.Add("Liste (List)");

            // comboBox2'nin düzenleme seçeneði kapalý
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            // Varsayýlan olarak "Detay" görünümünü seç
            comboBox2.SelectedIndex = 1; // Detay (Details)

            // Erkek radyo düðmesini varsayýlan olarak seç
            radioButton1.Checked = true;

            // Ýkon 1 radyo düðmesini varsayýlan olarak seç
            radioButton3.Checked = true;

            // Durum çubuðuna hoþgeldin mesajý ekle
            toolStripStatusLabel1.Text = "Hoþgeldiniz!";

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

                // ToolStrip butonlarýna ikonlarý ata
                toolStripButton1.Image = Image.FromFile("../../Icons/Toolbar-ClearStudentInfo.ico");
                toolStripButton2.Image = Image.FromFile("../../Icons/Toolbar-ClearStudentList.ico");
                toolStripButton3.Image = Image.FromFile("../../Icons/Toolbar-About.ico");

                // PictureBox'lara ikonlarý ata
                pictureBox1.Image = Image.FromFile("../../Icons/person1-small.jpg");
                pictureBox2.Image = Image.FromFile("../../Icons/person2-small.jpg");
                pictureBox3.Image = Image.FromFile("../../Icons/person3-small.jpg");
                pictureBox4.Image = Image.FromFile("../../Icons/person4-small.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ýkonlar yüklenirken hata oluþtu: " + ex.Message,
                                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ImageList'leri ListView'e ata
            listView1.SmallImageList = imageList1;
            listView1.LargeImageList = imageList2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ýlçe listBox'ýný temizle
            listBox1.Items.Clear();

            // comboBox1'de seçili bir öðe yoksa metottan çýk
            if (comboBox1.SelectedItem == null)
                return;

            // Seçilen þehre göre ilçeleri ekle
            string selectedCity = comboBox1.SelectedItem.ToString();

            if (selectedCity == "Ankara")
            {
                listBox1.Items.Add("Çankaya");
                listBox1.Items.Add("Beypazarý");
                listBox1.Items.Add("Polatlý");
                listBox1.Items.Add("Gölbaþý");
                listBox1.Items.Add("Sincan");
                listBox1.Items.Add("Mamak");
            }
            else if (selectedCity == "Eskiþehir")
            {
                listBox1.Items.Add("Alpu");
                listBox1.Items.Add("Çifteler");
                listBox1.Items.Add("Merkez");
                listBox1.Items.Add("Odunpazarý");
                listBox1.Items.Add("Sancakaya");
                listBox1.Items.Add("Seyitgazi");
                listBox1.Items.Add("Sivrihisar");
            }
            else if (selectedCity == "Ýstanbul")
            {
                listBox1.Items.Add("Beþiktaþ");
                listBox1.Items.Add("Bakýrköy");
                listBox1.Items.Add("Beyoðlu");
                listBox1.Items.Add("Beylikdüzü");
                listBox1.Items.Add("Eyüp");
                listBox1.Items.Add("Kadýköy");
                listBox1.Items.Add("Þiþli");
                listBox1.Items.Add("Üsküdar");
                listBox1.Items.Add("Zeytinburnu");
            }
            else if (selectedCity == "Ýzmir")
            {
                listBox1.Items.Add("Bornova");
                listBox1.Items.Add("Çeþme");
                listBox1.Items.Add("Dikili");
                listBox1.Items.Add("Foça");
                listBox1.Items.Add("Karþýyaka");
                listBox1.Items.Add("Konak");
                listBox1.Items.Add("Torbalý");
                listBox1.Items.Add("Urla");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // comboBox2'de seçili bir öðe yoksa metottan çýk
            if (comboBox2.SelectedIndex == -1)
                return;

            // Görünüm türünü seçilen deðere göre deðiþtir
            switch (comboBox2.SelectedIndex)
            {
                case 0: // Büyük Ýkon (Large Icon)
                    listView1.View = View.LargeIcon;
                    break;
                case 1: // Detay (Details)
                    listView1.View = View.Details;
                    break;
                case 2: // Döþeme (Tile)
                    listView1.View = View.Tile;
                    break;
                case 3: // Küçük Ýkon (Small Icon)
                    listView1.View = View.SmallIcon;
                    break;
                case 4: // Liste (List)
                    listView1.View = View.List;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Listeye Ekle butonuna basýldýðýnda
            try
            {
                // Gerekli kontrolleri yap
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    comboBox1.SelectedItem == null ||
                    listBox1.SelectedItem == null)
                {
                    MessageBox.Show("Bilgilerde eksiklik bulunmaktadýr!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Öðrenci bilgilerini al
                string ad = textBox1.Text;
                string soyad = textBox2.Text;
                string il = comboBox1.SelectedItem.ToString();
                string ilce = listBox1.SelectedItem.ToString();

                // Cinsiyet bilgisini al
                string cinsiyet = radioButton1.Checked ? "Erkek" : "Kadýn";

                // Hobi bilgilerini al
                string muzik = checkBox1.Checked ? "Evet" : "Hayýr";
                string kitap = checkBox2.Checked ? "Evet" : "Hayýr";
                string sinema = checkBox3.Checked ? "Evet" : "Hayýr";

                // Seçilen ikonu belirle
                int ikonIndex = 0;
                if (radioButton3.Checked) ikonIndex = 0;
                else if (radioButton4.Checked) ikonIndex = 1;
                else if (radioButton5.Checked) ikonIndex = 2;
                else if (radioButton6.Checked) ikonIndex = 3;

                // Listeye yeni öðe ekle
                ListViewItem item = new ListViewItem(ad);
                item.ImageIndex = ikonIndex; // Ýkon indeksi
                item.SubItems.Add(soyad);
                item.SubItems.Add(il);
                item.SubItems.Add(ilce);
                item.SubItems.Add(cinsiyet);
                item.SubItems.Add(muzik);
                item.SubItems.Add(kitap);
                item.SubItems.Add(sinema);

                listView1.Items.Add(item);

                // Formu temizle
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bilgilerde eksiklik bulunmaktadýr! Hata: " + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Öðrenci bilgilerini temizleme metodu
        private void ClearForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            radioButton1.Checked = true; // Erkek seçili
            radioButton3.Checked = true; // Ýkon 1 seçili
            checkBox1.Checked = false;   // Müzik seçili deðil
            checkBox2.Checked = false;   // Kitap seçili deðil
            checkBox3.Checked = false;   // Sinema seçili deðil
        }

        // Öðrenci listesini temizleme metodu
        private void ClearList()
        {
            listView1.Items.Clear();
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

        // Menü - Çýkýþ
        private void çýkýþToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Menü - Hakkýnda
        private void hakkýndaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
        }

        // ToolStrip butonlarý
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
            frm.ShowDialog(); // Hakkýnda formunu göster
        }

        // ToolStrip ItemClicked olayýný iþle
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Bu metot toolStrip'teki herhangi bir öðeye týklandýðýnda çalýþýr
            // Þu anda özel bir iþlem yapmasýna gerek yok
        }

        // MenuStrip ItemClicked olayýný iþle
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Bu metot menuStrip'teki herhangi bir öðeye týklandýðýnda çalýþýr
            // Þu anda özel bir iþlem yapmasýna gerek yok
        }
    }
}