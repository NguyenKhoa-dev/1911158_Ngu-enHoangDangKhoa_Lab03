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
            LoadToolStrip();

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
            lvSinhVien.Items.Add(lvItem);
        }

        private void LoadListView()
        {
            lvSinhVien.Items.Clear();
            foreach (var sv in qlsv.DanhSach)
            {
                Them(sv);
            }
            LoadToolStrip();
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
            ThemSinhVien();
        }

        private void ThemSinhVien()
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
                qlsv.Them(sv);
                LoadListView();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            XoaSinhVien();
        }

        private void XoaSinhVien()
        {
            int count, i;
            ListViewItem lvItem;
            count = lvSinhVien.Items.Count - 1;
            for (i = count; i >= 0; i--)
            {
                lvItem = lvSinhVien.Items[i];
                if (lvItem.Checked)
                    qlsv.Xoa(lvItem.SubItems[0].Text, SoSanhTheoMa);
            }
            LoadListView();
            btnMacDinh.PerformClick();
        }

        private int SoSanhTheoMa(object obj1, object obj2)
        {
            SinhVien sv = obj2 as SinhVien;
            return sv.MaSo.CompareTo(obj1);
        }

        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            mtxtMaSo.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            txtDiaChi.Text = "";
            cbbLop.Text = cbbLop.Items[0].ToString();
            txtHinh.Text = "";
            pbHinh.ImageLocation = "";
            rdNam.Checked = true;
            for (int i = 0; i < clbChuyenNganh.Items.Count - 1; i++)
                clbChuyenNganh.SetItemChecked(i, false);
            if (MessageBox.Show("Muốn tạo lại danh sách sinh viên không", "Chú Ý", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                qlsv.DanhSach.Clear();
                qlsv.DocTuFile();
                LoadListView();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SuaSinhVien();
        }

        private void SuaSinhVien()
        {
            SinhVien sv = GetSinhVien();
            bool kq;
            kq = qlsv.Sua(sv, sv.MaSo, SoSanhTheoMa);
            if (kq)
                LoadListView();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileIImage();
        }

        private void OpenFileIImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open File Image";
            dlg.Multiselect = true;
            dlg.Filter = "Image Files|"
            + "*.bmp;*.jpg;*.png|"
            + "All files (*.*)|*.*";
            dlg.FileName = "Hãy Chọn File";

            dlg.InitialDirectory = Environment.CurrentDirectory;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                txtHinh.Text = fileName;
                pbHinh.Load(fileName);
            }
        }

        private void LoadToolStrip()
        {
            int count = qlsv.DanhSach.Count;
            tstrDemSinhVien.Text = "Tổng sinh viên: " + count;
        }

        private void mởFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileIImage();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThemSinhVien();
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XoaSinhVien();
        }

        private void sửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuaSinhVien();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                lvSinhVien.Font = dlg.Font;
        }

        private void màuChữToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                lvSinhVien.ForeColor = dlg.Color;
        }

        private void sắpXếpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon dlg = new frmTuyChon(1);
            dlg.dssv = qlsv.DanhSach;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                qlsv.DanhSach = dlg.dssv;
                LoadListView();
            }
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTuyChon dlg = new frmTuyChon();
            dlg.dssv = qlsv.DanhSach;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.dssv.Count == 0)
                    return;
                else
                {
                    qlsv.DanhSach = dlg.dssv;
                    LoadListView();
                }                
            }
        }
    }
}
