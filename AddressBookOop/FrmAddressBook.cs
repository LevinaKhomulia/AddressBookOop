using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddressBookOop
{
    public partial class FrmAddressBook : Form
    {
        int selectedRowsCount = 0;
        int[] selectedRows = new int[15];
        Address addrBook = new Address();
        AddressBookController adrCon;

        public FrmAddressBook()
        {
            InitializeComponent();
        }

        private void FrmAddressBook_Load(object sender, EventArgs e)
        {
           adrCon = new AddressBookController();
            adrCon.LoadData(dgvData);
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            adrCon = new AddressBookController();
            FrmTambahData form = new FrmTambahData(true);
            if (form.Run(form))
            {
                adrCon.LoadData(dgvData);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            adrCon = new AddressBookController();
            adrCon.EditData(dgvData);
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            adrCon = new AddressBookController();
            adrCon.HapusData(dgvData, selectedRows, selectedRowsCount);
            adrCon.LoadData(dgvData);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            adrCon = new AddressBookController();

            if (this.txtNama.Text.Trim() != "" || this.txtAlamat.Text.Trim() != "" || this.txtKota.Text.Trim() != "" || this.txtNoHp.Text.Trim() != "" || this.txtTglLahir.Text.Trim() != "" || this.txtEmail.Text.Trim() != "")
            {
                try
                {
                    this.dgvData.Rows.Clear();
                    string[] fileContent = File.ReadAllLines("addressbook.csv");
                    foreach (string line in fileContent)
                    {
                        bool benar = false;
                        string[] arrItem = line.Split(';');
                        if (!benar && this.txtNama.Text.Trim() != "" && arrItem[0].ToLower().Contains(this.txtNama.Text.ToLower())) benar = true;
                        if (!benar && this.txtAlamat.Text.Trim() != "" && arrItem[1].ToLower().Contains(this.txtAlamat.Text.ToLower())) benar = true;
                        if (!benar && this.txtKota.Text.Trim() != "" && arrItem[2].ToLower().Contains(this.txtKota.Text.ToLower())) benar = true;
                        if (!benar && this.txtNoHp.Text.Trim() != "" && arrItem[3].ToLower().Contains(this.txtNoHp.Text.ToLower())) benar = true;
                        if (!benar && this.txtEmail.Text.Trim() != "" && arrItem[5].ToLower().Contains(this.txtEmail.Text.ToLower())) benar = true;
                        if (!benar && this.txtTglLahir.Text.Trim() != "")
                        {
                            DateTime tglDari, tglSampai;
                            if (this.txtTglLahir.Text.Trim().Contains("-"))
                            {
                                string[] arrTanggal = this.txtTglLahir.Text.Split('-');
                                if (!DateTime.TryParse(arrTanggal[0], out tglDari))
                                {
                                    throw new Exception("Sorry, kriteria tanggal lahir tidak valid ...");
                                }
                                if (!DateTime.TryParse(arrTanggal[1], out tglSampai))
                                {
                                    throw new Exception("Sorry, kriteria tanggal lahir tidak valid ...");
                                }
                            }
                            else
                            {
                                if (!DateTime.TryParse(this.txtTglLahir.Text, out tglDari))
                                {
                                    throw new Exception("Sorry, kriteria tanggal lahir tidak valid ...");
                                }
                                tglSampai = tglDari;
                            }
                            DateTime tglLahir = Convert.ToDateTime(arrItem[4]);
                            if (tglLahir.Date >= tglDari.Date && tglLahir.Date <= tglSampai.Date) benar = true;
                        }
                        if (benar)
                        {
                            this.dgvData.Rows.Add(new string[] { arrItem[0], arrItem[1], arrItem[2], arrItem[3], arrItem[4], arrItem[5] });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                adrCon.LoadData(dgvData);
            }
        }

        private void FrmAddressBook_Resize(object sender, EventArgs e)
        {
            this.dgvData.Columns[0].Width = 15 * this.dgvData.Width / 100;
            this.dgvData.Columns[1].Width = 25 * this.dgvData.Width / 100;
            this.dgvData.Columns[2].Width = 12 * this.dgvData.Width / 100;
            this.dgvData.Columns[3].Width = 12 * this.dgvData.Width / 100;
            this.dgvData.Columns[4].Width = 10 * this.dgvData.Width / 100;
            this.dgvData.Columns[5].Width = 20 * this.dgvData.Width / 100;
        }

        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            this.btnEdit_Click(null, null);
        }

        private void txtNama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) this.btnFilter_Click(null, null);

        }

        private void txtNoHp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.' || e.KeyChar == ' ' || e.KeyChar == '-' || e.KeyChar == '(' || e.KeyChar == ')')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtTglLahir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '/' || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
