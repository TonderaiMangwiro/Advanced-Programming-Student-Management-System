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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Form2 yüklendiğinde ikonları ayarla
            try
            {
                pictureBox1.Image = Image.FromFile("../../Icons/logo.jpg"); // About formundaki adam ikonu
                pictureBox2.Image = Image.FromFile("../../Icons/esoguLogo.png"); // Eskişehir Osmangazi Üniversitesi logosu
            }
            catch (Exception ex)
            {
                MessageBox.Show("İkon dosyaları yüklenirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}