using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace SY_Dexinjiaoyu
{
    public partial class frmReportViewer : Form
    {
        public frmReportViewer()
        {
            InitializeComponent();
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
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

    }
}
