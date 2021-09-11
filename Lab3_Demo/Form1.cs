using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Demo
{
    public partial class frmSinhVien : Form
    {
        QuanLySinhVien qlsv;
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DocTuFile();
            LoadListView();
        }

        private void Them(SinhVien sv)
        {
            ListViewItem lvItem = new ListViewItem(sv.MaSo);
            lvItem.SubItems.Add(sv.HoTen);
            lvItem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvItem.SubItems.Add(sv.DiaChi);
            lvItem.SubItems.Add(sv.Lop);
            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lvItem.SubItems.Add(gt);
            string cn = "";
            foreach (string s in sv.ChuyenNganh)
                cn += s + ",";
            cn = cn.Substring(0, cn.Length - 1);
            lvItem.SubItems.Add(cn);
            lvItem.SubItems.Add(sv.Hinh);
            this.lvSinhVien.Items.Add(lvItem);
        }

        private void LoadListView()
        {
            lvSinhVien.Items.Clear();
            foreach (var sv in qlsv.DanhSach)
            {
                Them(sv);
            }
        }

        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            sv.MaSo = mtxtMaSo.Text;
            sv.HoTen = txtHoTen.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.DiaChi = txtDiaChi.Text;
            sv.Lop = cbbLop.Text;
            sv.Hinh = txtHinh.Text;
            if (rdNu.Checked)
                gt = false;
            sv.GioiTinh = gt;
            for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
                if (clbChuyenNganh.GetItemChecked(i))
                    cn.Add(clbChuyenNganh.Items[i].ToString());
            sv.ChuyenNganh = cn;
            return sv;
        }

        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = this.lvSinhVien.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvitem = this.lvSinhVien.SelectedItems[0];
                SinhVien sv = GetSinhVienListView(lvitem);
                ThietLapThongTin(sv);
            }
        }

        private SinhVien GetSinhVienListView(ListViewItem lvItem)
        {
            SinhVien sv = new SinhVien();
            sv.MaSo = lvItem.SubItems[0].Text;
            sv.HoTen = lvItem.SubItems[1].Text;
            sv.NgaySinh = DateTime.Parse(lvItem.SubItems[2].Text);
            sv.DiaChi = lvItem.SubItems[3].Text;
            sv.Lop = lvItem.SubItems[4].Text;
            sv.GioiTinh = false;
            if (lvItem.SubItems[5].Text == "Nam")
                sv.GioiTinh = true;
            List<string> cn = new List<string>();
            string[] s = lvItem.SubItems[6].Text.Split(',');
            foreach (var item in s)
            {
                cn.Add(item.Trim());
            }
            sv.ChuyenNganh = cn;
            sv.Hinh = lvItem.SubItems[7].Text;
            return sv;
        }

        private void ThietLapThongTin(SinhVien sv)
        {
            mtxtMaSo.Text = sv.MaSo;
            txtHoTen.Text = sv.HoTen;
            dtpNgaySinh.Value = sv.NgaySinh;
            txtDiaChi.Text = sv.DiaChi;
            cbbLop.Text = sv.Lop;
            txtHinh.Text = sv.Hinh;
            pbHinh.ImageLocation = sv.Hinh;
            if (sv.GioiTinh)
                rdNam.Checked = true;
            else
                rdNu.Checked = true;

            for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
                clbChuyenNganh.SetItemChecked(i, false);

            foreach (string s in sv.ChuyenNganh)
            {
                for (int i = 0; i < clbChuyenNganh.Items.Count; i++)
                    if (s.CompareTo(clbChuyenNganh.Items[i]) == 0)
                        clbChuyenNganh.SetItemChecked(i, true);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MaSo, delegate (object obj1, object obj2)
            {
                return (obj2 as SinhVien).MaSo.CompareTo(obj1.ToString());
            });
            if (kq != null)
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Lỗi thêm dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.qlsv.Them(sv);
                this.LoadListView();
            }
        }


    }
}
