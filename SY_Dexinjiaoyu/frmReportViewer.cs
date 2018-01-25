using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Order.DB;

namespace SY_Dexinjiaoyu
{
    public partial class frmReportViewer : Form
    {
        List<clsDATAinfo> Result;
        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;
        public frmReportViewer(List<clsDATAinfo> Orderinfolist_Server)
        {
            InitializeComponent();
            InitialSystemInfo();
            Result = new List<clsDATAinfo>();

            Result = Orderinfolist_Server;

            InitializeReportEvent();

        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            //this.reportViewer1.RefreshReport();


            try
            {
               
                reportViewer1.LocalReport.ReportPath = Application.StartupPath + "\\Report1.rdlc";
               
                 //指定数据集,数据集名称后为表,不是DataSet类型的数据集
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", Result));

                #region 印章
                reportViewer1.LocalReport.EnableExternalImages = true;//启用外部加载图片。默认false  
                ReportParameter params1;
                string yinzhang_path = AppDomain.CurrentDomain.BaseDirectory + "Resources\\seal\\" + "定州市安全生产监督管理局副本.gif";//02.gif

               params1 = new ReportParameter("yinzhang", "file:///" + yinzhang_path);//注意图片路径格式  
               reportViewer1.LocalReport.SetParameters(new ReportParameter[] { params1 });//加载参数  
         
                #endregion
                  //显示报表
                this.reportViewer1.RefreshReport();
                  //240×340   一般文件袋
                //2835  X  1713



            }
            catch (Exception ex)
            {
                MessageBox.Show("异常" + ex);
                return;

                throw;
            }
         

        }
        //自动导出excel/pdf/word
        private void ResponseFile(int oType, string fileName)
        {
            //string outType;
            //if (oType == 0)
            //{
            //    outType = "Excel";
            //}
            //else if (oType == 1)
            //{
            //    outType = "Word";
            //}
            //else
            //{
            //    outType = "Word";
            //}
            //try
            //{
            //    Warning[] warnings;
            //    string[] streamids;

            //    string mimeType;
            //    string encoding;
            //    string extension;


            //    byte[] bytes = ReportViewer1.LocalReport.Render(
            //            outType, null, out mimeType, out encoding, out extension,
            //            out streamids, out warnings);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = mimeType;
            //    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "." + extension);
            //    Response.BinaryWrite(bytes);

            //    Response.Flush();

            //}

            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        public void btnExportExcel_Click( )
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = reportViewer1.LocalReport.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            FileStream fs = new FileStream(@"c:\output.xls",
               FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

        }
        private void InitialSystemInfo()
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");

            #endregion
        }
        private void InitializeReportEvent()
        {
            //this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
            this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);

            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;


            //this.reportViewer1.Width = 10;
            //this.reportViewer1.Height = 10;
            PageSettings pageset = new PageSettings();
            pageset.Landscape = true;
            //var pageSettings = this.reportViewer1.GetPageSettings();
            pageset.PaperSize = new PaperSize()
            {
                //Width = 210,
                //Height = 297
                //
                //Width = 100,
                //Height = 100

                Width = 3800,
                Height = 3800
                //Width = 340,
                //Height = 240
            };
            pageset.Margins = new Margins() { Left = 10, Top = 10, Bottom = 10, Right = 10 };
            reportViewer1.SetPageSettings(pageset);

        }

    }
}
