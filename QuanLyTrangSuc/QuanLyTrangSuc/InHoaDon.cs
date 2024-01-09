using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTrangSuc
{
    public partial class InHoaDon : Form
    {
        private int id_hoadon;
        private int tongtien;
        private DateTime date;
        public InHoaDon(int id_hoadon, int tongtien, DateTime date)
        {
            InitializeComponent();
            this.id_hoadon = id_hoadon;
            this.tongtien = tongtien;
            this.date = date;
        }
        KetNoi kn = new KetNoi();


        void get_data()
        {
            label_iD.Text = "" + id_hoadon;
            label_tongtien.Text = tongtien + "VND";
            label_ngaythang.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void in_hoadon_Load(object sender, EventArgs e)
        {
            get_data();
            getdanhsach();
        }

        void getdanhsach()
        {
            try
            {
                string query = string.Format("SELECT cthd.ID_hoadon , cthd.ID_SanPham , sp.TenSanPham , " +
                                            "sp.GiaBan,sp.GiamGia,cthd.SoLuong , " +
                                            "SUM((sp.GiaBan - (sp.GiaBan *(sp.GiamGia / 100))) * cthd.SoLuong) AS TongTien " +
                                            "FROM chitiethoadon cthd, SanPham sp " +
                                            "where cthd.ID_SanPham = sp.ID_SanPham and ID_HoaDon = '{0}' " +
                                            "GROUP BY cthd.ID_HoaDon, cthd.ID_SanPham,sp.TenSanPham , sp.GiaBan,sp.GiamGia,cthd.SoLuong", id_hoadon);
                DataSet ds = kn.selectData(query);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    txt_chitietsanpham.Text += string.Format(
                        "\nSản phẩm: {0,-20} | Gia: {1,-7} | Giảm giá: {2,-7}% | Số lượng: {3,-7} | Tổng: {4,-7} VND\n",
                        row["TenSanPham"].ToString(),
                        row["GiaBan"].ToString(),
                        row["GiamGia"].ToString(),
                        row["SoLuong"].ToString(),
                        row["TongTien"].ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void InHoaDon_Load(object sender, EventArgs e)
        {
            get_data();
            getdanhsach();
        }

        private void txt_chitietsanpham_Click(object sender, EventArgs e)
        {

        }
    }
}
