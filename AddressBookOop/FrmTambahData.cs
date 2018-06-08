using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace AddressBookOop
{
    public partial class FrmTambahData : Form
    {
        bool _result = false;
        bool _addMode = false; // true : add item, false : edit item
        string[] txt; 
        Address _addrBook = null;
         
        public bool Run(FrmTambahData form)
        {
            form.ShowDialog();
            return _result;
        }

        public FrmTambahData(bool addMode, Address addrBook = null)
        {
            InitializeComponent();
            _addMode = addMode;
            if (addrBook != null)
            {
                _addrBook = addrBook;
                this.txtNama.Text = addrBook.Nama;
                this.txtAlamat.Text = addrBook.Alamat;
                this.txtKota.Text = addrBook.Kota;
                this.txtNoHp.Text = addrBook.NoHp;
                this.dtpTglLahir.Value = addrBook.TanggalLahir.Date;
                this.txtEmail.Text = addrBook.Email;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // validasi 
            if (this.txtNama.Text.Trim() == "") // jika isian nama kosong
            {
                MessageBox.Show("Sorry, nama wajib isi ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtNama.Focus();
            }
            else if (this.txtAlamat.Text.Trim() == "") // jika alamat kosong 
            {
                MessageBox.Show("Sorry, alamat wajib isi ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtAlamat.Focus();
            }
            else if (this.txtKota.Text.Trim() == "") // jika kota kosong
            {
                MessageBox.Show("Sorry, kota wajib isi ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtKota.Focus();
            }
            else if (this.txtNoHp.Text.Trim() == "") // jika nohp kosong
            {
                MessageBox.Show("Sorry, no. hp wajib isi ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtNoHp.Focus();
            }
            else if (this.txtEmail.Text.Trim() == "") // jika email kosong
            {
                MessageBox.Show("Sorry, email wajib isi ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtEmail.Focus();
            }
            else
            {
                AddressBookController adrCon;
                adrCon = new AddressBookController();
                adrCon.TambahData(true,txt);
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNama_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{tab}");
        }

        private bool EmailIsValid(string emailAddr)
        {
            string emailPattern1 = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(emailPattern1);
            Match match = regex.Match(emailAddr);
            return match.Success;
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (this.txtEmail.Text.Trim() != "")
            {
                if (!EmailIsValid(this.txtEmail.Text))
                {
                    MessageBox.Show("Sorry, data email tidak valid ...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtEmail.Clear();
                    this.txtEmail.Focus();
                }
            }
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
    }
}
