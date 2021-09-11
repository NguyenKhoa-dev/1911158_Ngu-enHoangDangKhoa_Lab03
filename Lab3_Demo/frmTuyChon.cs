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
    public partial class frmTuyChon : Form
    {
        private int _kieu { get; set; }
        public List<SinhVien> dssv;

        public frmTuyChon(int kieu = 0)
        {
            _kieu = kieu;
            InitializeComponent();
        }

        private void frmTuyChon_Load(object sender, EventArgs e)
        {
            LoadFormTheoKieu();
            rdMaSV.Checked = true;
        }

        private void LoadFormTheoKieu()
        {
            if (_kieu != 0)
                pnlTimKiem.Enabled = false;
            else
                btnSapXep.Enabled = false;                   
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtNhap.Text))
                MessageBox.Show("Hãy nhập thông tin!", "Lỗi nhập thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (rdMaSV.Checked)
                {
                    dssv = dssv.FindAll(sv => sv.MaSo == txtNhap.Text);
                    DemSoSV(dssv);
                }
                if (rdHoTen.Checked)
                {
                    dssv = dssv.FindAll(sv => sv.HoTen == txtNhap.Text);
                    DemSoSV(dssv);
                }
                if (rdNgaySinh.Checked)
                {
                    dssv = dssv.FindAll(sv => sv.NgaySinh == DateTime.Parse(txtNhap.Text));
                    DemSoSV(dssv);
                }
                DialogResult = DialogResult.OK;
            }                 
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            if (rdMaSV.Checked)
                dssv = dssv.OrderBy(sv => sv.MaSo).ToList();
            if (rdHoTen.Checked)
                dssv = dssv.OrderBy(sv => sv.HoTen).ToList();
            if (rdNgaySinh.Checked)
                dssv = dssv.OrderBy(sv => sv.NgaySinh).ToList();
            DialogResult = DialogResult.OK;
        }

        private void rdMaSV_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMaSV.Checked)
            {
                txtNhap.Mask = "SV.0000000";
            }
            else
                txtNhap.Mask = "";
        }

        private void DemSoSV(List<SinhVien> ds)
        {
            int count = ds.Count();
            string msg = "Số sinh viên tìm thấy: " + count;
            MessageBox.Show(msg);
        }

        private void rdNgaySinh_CheckedChanged(object sender, EventArgs e)
        {
            if (rdNgaySinh.Checked)
            {
                txtNhap.Mask = "00/00/0000";
                txtNhap.TextMaskFormat = MaskFormat.IncludeLiterals;
            }               
            else
            {
                txtNhap.Mask = "";
                txtNhap.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            }                
        }
    }
}
