using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace QuanLyTrangSuc
{
    public partial class ThongKe : Form
    {
        KetNoi kn = new KetNoi();
        public ThongKe()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home frm = new Home();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SanPham frm = new SanPham();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
            TaiKhoan frm = new TaiKhoan();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon frm = new HoaDon();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhaCungCap frm = new NhaCungCap();
            frm.ShowDialog();
            this.Close();
        }

      

        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhap frm = new PhieuNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            this.Hide();
            DangNhap frm = new DangNhap();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKe frm = new ThongKe();
            frm.ShowDialog();
            this.Close();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ThongKeSanPham frm = new ThongKeSanPham();
            frm.ShowDialog();
            this.Close();
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            get_year();
            cbx_nam.SelectedValue = 0;
        }
        void get_year()
        {
            try
            {
                string query = "SELECT DISTINCT YEAR(NgayXuat) AS NamXuat FROM HoaDon";
                DataSet ds = kn.selectData(query);
                cbx_nam.DataSource = ds.Tables[0];
                cbx_nam.DisplayMember = "Nam";
                cbx_nam.ValueMember = "NamXuat";
                cbx_nam.Text = "2023";

            }
            catch (Exception ex)
            {

            }
        }

        private void cbx_nam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbx_nam.Text != "")
                {
                    chart_thongkedoanhthu.Series.Clear();

                    string query = "SELECT MONTH(ngayxuat) AS Thang, SUM(tongtien) AS TongTien " +
                                   $"FROM hoadon where YEAR(ngayxuat) = {cbx_nam.Text} " +
                                   "GROUP BY MONTH(ngayxuat)";
                    DataSet ds = kn.selectData(query);

                    Series newColumnSeries = new Series("TongTienThang_Column");
                    newColumnSeries.ChartType = SeriesChartType.Column;
                    chart_thongkedoanhthu.Series.Add(newColumnSeries);
                    newColumnSeries["PointWidth"] = "0.5";

                    Series newLineSeries = new Series("TongTienThang_Line");
                    newLineSeries.ChartType = SeriesChartType.Line;
                    chart_thongkedoanhthu.Series.Add(newLineSeries);
                    newLineSeries.BorderWidth = 2;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataPoint pointColumn = new DataPoint();
                        pointColumn.SetValueXY(row["Thang"], row["TongTien"]);
                        pointColumn.LabelForeColor = Color.Red;
                        pointColumn.Color = Color.FromArgb(73, 134, 255);
                        newColumnSeries.Points.Add(pointColumn);

                        DataPoint pointLine = new DataPoint();
                        pointLine.SetValueXY(row["Thang"], row["TongTien"]);
                        pointLine.Label = row["TongTien"].ToString();
                        pointLine.LabelForeColor = Color.Red;
                        pointLine.Color = Color.FromArgb(255, 0, 0);
                        newLineSeries.Points.Add(pointLine);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void btn_theothang_Click(object sender, EventArgs e)
        {

            try
            {
                label_thongke.Text = "Thống kê theo năm";
                btn_theonam.Visible = true;
                btn_theothang.Visible = false;
                cbx_nam.Enabled = false;
                cbx_nam.Text = "2023";
                chart_thongkedoanhthu.Series.Clear();

                string query = "SELECT YEAR(ngayxuat) AS NAM, SUM(tongtien) AS TongTien FROM hoadon GROUP BY YEAR(ngayxuat)";
                DataSet ds = kn.selectData(query);

                Series newColumnSeries = new Series("TongTienNam_colum");
                newColumnSeries.ChartType = SeriesChartType.Column;
                chart_thongkedoanhthu.Series.Add(newColumnSeries);
                newColumnSeries["PointWidth"] = "0.6";

                Series newLineSeries = new Series("TongTienNam_line");
                newLineSeries.ChartType = SeriesChartType.Line;
                chart_thongkedoanhthu.Series.Add(newLineSeries);
                newLineSeries.BorderWidth = 2;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataPoint pointColumn = new DataPoint();
                    pointColumn.SetValueXY(row["NAM"], row["TongTien"]);
                    pointColumn.LabelForeColor = Color.Red;
                    pointColumn.Color = Color.FromArgb(73, 134, 255);
                    newColumnSeries.Points.Add(pointColumn);

                    DataPoint pointLine = new DataPoint();
                    pointLine.SetValueXY(row["NAM"], row["TongTien"]);
                    pointLine.Label = row["TongTien"].ToString();
                    pointLine.LabelForeColor = Color.Red;
                    pointLine.Color = Color.FromArgb(255, 0, 0);
                    newLineSeries.Points.Add(pointLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void btn_theonam_Click(object sender, EventArgs e)
        {
            try
            {
                label_thongke.Text = "Thống kê theo tháng";
                btn_theonam.Visible = false;
                btn_theothang.Visible = true;
                cbx_nam.Enabled = true;
                if (cbx_nam.Text != "")
                {
                    chart_thongkedoanhthu.Series.Clear();

                    string query = "SELECT MONTH(ngayxuat) AS Thang, SUM(tongtien) AS TongTien " +
                                   $"FROM hoadon where YEAR(ngayxuat) = {cbx_nam.Text} " +
                                   "GROUP BY MONTH(ngayxuat)";
                    DataSet ds = kn.selectData(query);

                    Series newColumnSeries = new Series("TongTienThang_Column");
                    newColumnSeries.ChartType = SeriesChartType.Column;
                    chart_thongkedoanhthu.Series.Add(newColumnSeries);
                    newColumnSeries["PointWidth"] = "0.6";

                    Series newLineSeries = new Series("TongTienThang_Line");
                    newLineSeries.ChartType = SeriesChartType.Line;
                    chart_thongkedoanhthu.Series.Add(newLineSeries);
                    newLineSeries.BorderWidth = 2;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataPoint pointColumn = new DataPoint();
                        pointColumn.SetValueXY(row["Thang"], row["TongTien"]);
                        pointColumn.LabelForeColor = Color.Red;
                        pointColumn.Color = Color.FromArgb(73, 134, 255);
                        newColumnSeries.Points.Add(pointColumn);

                        DataPoint pointLine = new DataPoint();
                        pointLine.SetValueXY(row["Thang"], row["TongTien"]);
                        pointLine.Label = row["TongTien"].ToString();
                        pointLine.LabelForeColor = Color.Red;
                        pointLine.Color = Color.FromArgb(255, 0, 0);
                        newLineSeries.Points.Add(pointLine);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void chart_thongkedoanhthu_Click(object sender, EventArgs e)
        {

        }
    }
}
