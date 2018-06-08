using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace AddressBookOop
{
    public class AddressBookController
    {
        FrmAddressBook form1 = new FrmAddressBook();
        FrmTambahData form2 = new FrmTambahData(true, null);
        Address addrBook = new Address();
        Address _addrBook = null;
        bool _result = false;

        //property
        public List<Address> ListData { get; set; }

        public AddressBookController() //constructor
        {
            ListData = new List<Address>();

            try
            {
                if (File.Exists(Properties.Settings.Default.NamaFile))
                {
                    string[] fileContent = File.ReadAllLines(Properties.Settings.Default.NamaFile);

                    foreach (string item in fileContent)
                    {
                        string[] arrItem = item.Split(';');
                        ListData.Add(new Address
                        {
                            Nama = arrItem[0].Trim(),
                            Alamat = arrItem[1].Trim(),
                            Kota = arrItem[2].Trim(),
                            NoHp = arrItem[3].Trim(),
                            TanggalLahir = Convert.ToDateTime(arrItem[4]),
                            Email = arrItem[5].Trim(),
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        //method

        public void LoadData(DataGridView dgvData)
        {
            try
            {
                dgvData.Rows.Clear();
                if (File.Exists("addressbook.csv"))
                {
                    string[] arrLine = File.ReadAllLines("addressbook.csv");
                    if (arrLine.Length > 0)
                    {
                        foreach (string item in arrLine)
                        {
                            if (item.Trim() != "")
                            {
                                string[] arrItem = item.Split(';');
                                dgvData.Rows.Add(new string[] { arrItem[0], arrItem[1], arrItem[2], arrItem[3], arrItem[4], arrItem[5] });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form1.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                form1.lblBanyakRecordData.Text = $"{dgvData.Rows.Count.ToString("n0")} Record data.";
            }
        }

        public void TambahData(bool _addMode, string[] txt)
        {
            try
            {
                // simpan data ke file 
                if (_addMode) // add new item 
                {
                    using (FileStream fs = new FileStream("addressbook.csv", FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            writer.WriteLine($"{txt[0].Trim()};{txt[1].Trim()};{txt[2].Trim()};{txt[3].Trim()};{form2.dtpTglLahir.Value.ToShortDateString()};{txt[5].Trim()}");
                        }
                    }
                }
                else // edit data
                {
                    string[] fileContent = File.ReadAllLines("addressbook.csv");
                    using (FileStream fs = new FileStream("temporary.csv", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            foreach (string line in fileContent)
                            {
                                string[] arrline = line.Split(';');
                                if (arrline[0] == _addrBook.Nama && arrline[1] == _addrBook.Alamat && arrline[2] == _addrBook.Kota && arrline[3] == _addrBook.NoHp && Convert.ToDateTime(arrline[4]).Date == _addrBook.TanggalLahir.Date && arrline[5] == _addrBook.Email)
                                {
                                    writer.WriteLine($"{txt[0].Trim()};{txt[1].Trim()};{txt[2].Trim()};{txt[3].Trim()};{form2.dtpTglLahir.Value.ToShortDateString()};{txt[5].Trim()}");
                                }
                                else
                                {
                                    writer.WriteLine(line);
                                }
                            }
                        }
                    }
                    File.Delete(Properties.Settings.Default.NamaFile);
                    File.Move("temporary.csv", Properties.Settings.Default.NamaFile);
                }
                _result = true;
                form2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form2.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void HapusData(DataGridView dgvData, int[] selectedRows, int selectedRowsCount)
        {
            if (selectedRowsCount >= 0 && MessageBox.Show("Hapus Baris Data Terpilih ? ", form1.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataGridViewRow row = dgvData.SelectedRows[0];
                addrBook.Nama = row.Cells[0].Value.ToString();
                addrBook.Alamat = row.Cells[1].Value.ToString();
                addrBook.Kota = row.Cells[2].Value.ToString();
                addrBook.NoHp = row.Cells[3].Value.ToString();
                addrBook.TanggalLahir = Convert.ToDateTime(row.Cells[4].Value).Date;
                addrBook.Email = row.Cells[5].Value.ToString();
                try
                {
                    string[] fileContent = File.ReadAllLines("addressbook.csv");
                    using (FileStream fs = new FileStream("temporary.csv", FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(fs))
                        {
                            foreach (string line in fileContent)
                            {
                                string[] arrline = line.Split(';');
                                if (arrline[0] == addrBook.Nama && arrline[1] == addrBook.Alamat && arrline[2] == addrBook.Kota && arrline[3] == addrBook.NoHp && Convert.ToDateTime(arrline[4]).Date == addrBook.TanggalLahir.Date && arrline[5] == addrBook.Email)
                                { // do nothing 
                                }
                                else
                                {
                                    writer.WriteLine(line);
                                }
                            }
                        }
                    }
                    File.Delete("addressbook.csv");
                    File.Move("temporary.csv", "addressbook.csv");
                    LoadData(dgvData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, form1.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void EditData(DataGridView dgvData)
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                DataGridViewRow row = form1.dgvData.SelectedRows[0];
                addrBook.Nama = row.Cells[0].Value.ToString();
                addrBook.Alamat = row.Cells[1].Value.ToString();
                addrBook.Kota = row.Cells[2].Value.ToString();
                addrBook.NoHp = row.Cells[3].Value.ToString();
                addrBook.TanggalLahir = Convert.ToDateTime(row.Cells[4].Value).Date;
                addrBook.Email = row.Cells[5].Value.ToString();
                FrmTambahData form = new FrmTambahData(false, addrBook);
                if (form.Run(form))
                {
                    LoadData(dgvData);
                }
            }
        }

    }
}
