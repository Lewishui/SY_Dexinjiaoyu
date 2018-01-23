using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Order.Common;
using Order.DB;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using Microsoft.Win32;


namespace Dexin.Buiness
{
    public class clsAllnew
    {
        System.Reflection.Missing missing = System.Reflection.Missing.Value;
        private string dataSource = "SY_Dexinjiaoyu.sqlite";
        string newsth;
        public BackgroundWorker bgWorker1;
        private Object Nothing = Missing.Value;
        private string savepath;
        clsDATAinfo punblic_item;


        #region print
        private List<Stream> m_streams;
        private int m_currentPageIndex;
        List<clsDATAinfo> FilterTIPResults;
        string orderprint;
        #endregion


        public clsAllnew()
        {
            newsth = AppDomain.CurrentDomain.BaseDirectory + "" + dataSource;//System\\
            getUserPint();


        }
        public List<clsDATAinfo> Read_Excel(string Alist)
        {

            List<clsDATAinfo> MAPPINGResult = new List<clsDATAinfo>();
            try
            {

                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Application excelApp;
                {
                    string path = Alist;
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook analyWK = excelApp.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing,
                        "htc", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    Microsoft.Office.Interop.Excel.Worksheet WS = (Microsoft.Office.Interop.Excel.Worksheet)analyWK.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range rng;
                    rng = WS.Range[WS.Cells[2, 1], WS.Cells[WS.UsedRange.Rows.Count, 26]];
                    int rowCount = WS.UsedRange.Rows.Count - 1;
                    object[,] o = new object[2, 1];
                    o = (object[,])rng.Value2;
                    int wscount = analyWK.Worksheets.Count;
                    clsCommHelp.CloseExcel(excelApp, analyWK);
                    for (int i = 1; i <= rowCount; i++)
                    {
                        clsDATAinfo temp = new clsDATAinfo();

                        #region 基础信息

                        temp.zhenghao = "";
                        if (o[i, 1] != null)
                            temp.zhenghao = o[i, 1].ToString().Trim();

                        temp.xingming = "";
                        if (o[i, 2] != null)
                            temp.xingming = o[i, 2].ToString().Trim();

                        if (temp.xingming == null || temp.xingming == "")
                            break;

                        temp.xingbie = "";
                        if (o[i, 3] != null)
                            temp.xingbie = o[i, 3].ToString().Trim();


                        temp.zuoyeleibie = "";
                        if (o[i, 4] != null)
                            temp.zuoyeleibie = o[i, 4].ToString().Trim();

                        temp.zhuncaoxiangmu = "";
                        if (o[i, 5] != null)
                            temp.zhuncaoxiangmu = o[i, 5].ToString().Trim();

                        temp.chulingriqi = "";
                        if (o[i, 6] != null)
                            temp.chulingriqi = o[i, 6].ToString().Trim();
                        temp.youxiangqixian = "";
                        if (o[i, 7] != null)
                            temp.youxiangqixian = o[i, 7].ToString().Trim();

                        temp.fushenriqi = "";
                        if (o[i, 8] != null)
                            temp.fushenriqi = o[i, 8].ToString().Trim();

                        temp.Input_Date = DateTime.Now;

                        #endregion
                        MAPPINGResult.Add(temp);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: 01032 表格维护有误，请重新检查格式及命名！");
                return null;

                throw;
            }
            return MAPPINGResult;

        }

        public string sql_yuju(DoWorkEventArgs e, clsDATAinfo item, string conditions)
        {
            if (item.zhenghao != null)
            {
                conditions += " zhenghao ='" + item.zhenghao + "'";
            }
            if (item.xingming != null)
            {
                conditions += " ,xingming ='" + item.xingming + "'";
            }
            if (item.xingbie != null)
            {
                conditions += " ,xingbie ='" + item.xingbie + "'";
            }
            if (item.zuoyeleibie != null)
            {
                conditions += " ,zuoyeleibie ='" + item.zuoyeleibie + "'";
            }
            if (item.zhuncaoxiangmu != null)
            {
                conditions += " ,zhuncaoxiangmu ='" + item.zhuncaoxiangmu + "'";
            }
            if (item.chulingriqi != null)
            {
                conditions += " ,chulingriqi ='" + item.chulingriqi + "'";
            }
            if (item.youxiangqixian != null)
            {
                conditions += " ,youxiangqixian ='" + item.youxiangqixian + "'";
            }
            if (item.fushenriqi != null)
            {
                conditions += " ,fushenriqi ='" + item.fushenriqi + "'";
            }

            if (item.Message != null)
            {
                conditions += " ,Message ='" + item.Message + "'";
            }
            if (item.Input_Date != null)
            {
                conditions += " ,Input_Date ='" + item.Input_Date.ToString("yyyy/MM/dd") + "'";
            }
            if (item.xinzeng == "true")
                conditions = "insert into PeopleData(zhenghao,xingming,xingbie,zuoyeleibie,zhuncaoxiangmu,chulingriqi,youxiangqixian,fushenriqi,Input_Date,Message) values ('" + item.zhenghao + "','" + item.xingming + "','" + item.xingbie + "','" + item.zuoyeleibie + "','" + item.zhuncaoxiangmu + "','" + item.chulingriqi + "','" + item.youxiangqixian + "','" + item.fushenriqi + "','" + item.Input_Date.ToString("yyyy/MM/dd") + "','" + item.Message + "')";
            else
                conditions = "update PeopleData set  " + conditions + " where Item_ID = " + item.Item_ID + " ";

            return conditions;
        }
        public int update_Server(string findtext, clsDATAinfo item)
        {
            if (findtext.Contains("insert into"))
            {
                string sql2 = "delete from PeopleData where  zhenghao='" + item.zhenghao + "'" + "And xingming='" + item.xingming + "'";

                int isrun1 = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.CONNECTION_STRING_BASE, sql2, CommandType.Text, null);

            }
            int isrun = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.CONNECTION_STRING_BASE, findtext, CommandType.Text, null);

            return isrun;
        }
        public int deleteitem(string name)
        {
            string sql2 = "delete from PeopleData where  Item_ID='" + name + "'";

            int isrun = SQLiteHelper.ExecuteNonQuery(SQLiteHelper.CONNECTION_STRING_BASE, sql2, CommandType.Text, null);

            return isrun;

        }
        public List<clsDATAinfo> findOrder(string findtext)
        {

            SQLiteConnection dbConn = new SQLiteConnection("Data Source=" + dataSource);

            dbConn.Open();
            SQLiteCommand dbCmd = dbConn.CreateCommand();
            dbCmd.CommandText = findtext;

            DbDataReader reader = SQLiteHelper.ExecuteReader("Data Source=" + newsth, dbCmd);
            List<clsDATAinfo> ClaimReport_Server = new List<clsDATAinfo>();

            while (reader.Read())
            {
                clsDATAinfo item = new clsDATAinfo();

                item.Item_ID = reader.GetInt32(0);
                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.zhenghao = reader.GetString(1);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.xingming = Convert.ToString(reader.GetString(2));
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.xingbie = reader.GetString(3);
                if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")

                    item.zuoyeleibie = reader.GetString(4);
                if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")

                    item.zhuncaoxiangmu = reader.GetString(5);
                if (reader.GetValue(6) != null && Convert.ToString(reader.GetValue(6)) != "")

                    item.chulingriqi = reader.GetString(6);
                if (reader.GetValue(7) != null && Convert.ToString(reader.GetValue(7)) != "")

                    item.youxiangqixian = reader.GetString(7);
                if (reader.GetValue(8) != null && Convert.ToString(reader.GetValue(8)) != "")

                    item.fushenriqi = reader.GetString(8);

                if (reader.GetValue(9) != null && Convert.ToString(reader.GetValue(9)) != "")
                    item.Input_Date = Convert.ToDateTime(reader.GetString(9));


                if (reader.GetValue(10) != null && Convert.ToString(reader.GetValue(10)) != "")
                    item.Message = Convert.ToString(reader.GetString(10));

                ClaimReport_Server.Add(item);


            }
            return ClaimReport_Server;
        }
        #region 替换word


        /// <summary>
        /// 替换word中的文本，并导出word
        /// </summary>
        public void ReplaceToExcel(ref BackgroundWorker bgWorker, clsDATAinfo temp, string savefolder)
        {
            bgWorker1 = bgWorker;
            bgWorker1.ReportProgress(0, "准备中 ....");

            string ZFCEPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\photo\\" + temp.xingming + ".jpg";
            string yinzhangweizhi = AppDomain.CurrentDomain.BaseDirectory + "Resources\\seal\\" + temp.Message + ".gif";//02.gif

            Word.Application app = null;
            Word.Document doc = null;
            //将要导出的新word文件名
            string newFile = DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc";
            //  string physicNewFile = Server.MapPath(DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc");
            string physicNewFile = AppDomain.CurrentDomain.BaseDirectory + "Results\\" + temp.xingming + "-" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc";
            physicNewFile = savefolder + "\\" + temp.xingming + "-" + "" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc"; ;

            try
            {
                app = new Word.Application();//创建word应用程序

                //object fileName = Server.MapPath("template.doc");//模板文件
                object fileName = AppDomain.CurrentDomain.BaseDirectory + "Resources\\" + "TEL1.docx";

                //打开模板文件
                object oMissing = System.Reflection.Missing.Value;
                doc = app.Documents.Open(ref fileName,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //构造数据
                Dictionary<string, string> datas = new Dictionary<string, string>();
                datas.Add("{zhenghao}", temp.zhenghao);
                datas.Add("{xingming}", temp.xingming);
                //datas.Add("{sex}", temp.xingbie);
                datas.Add("{zuoyeleibie}", temp.zuoyeleibie);

                datas.Add("{zhuncaoxiangmu}", temp.zhuncaoxiangmu);
                datas.Add("{chulingriqi}", temp.chulingriqi);
                datas.Add("{youxiaoqixian} ", temp.youxiangqixian);
                datas.Add("{fushenriqi}", temp.fushenriqi);



                object replace = Word.WdReplace.wdReplaceAll;
                foreach (var item in datas)
                {
                    app.Selection.Find.Replacement.ClearFormatting();
                    app.Selection.Find.ClearFormatting();
                    app.Selection.Find.Text = item.Key;//需要被替换的文本
                    app.Selection.Find.Replacement.Text = item.Value;//替换文本 

                    //执行替换操作
                    app.Selection.Find.Execute(
                    ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref replace,
                    ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing);
                }
                doc.ActiveWindow.Visible = true;
                foreach (Bookmark bk in doc.Bookmarks)
                {
                    if (bk.Name == "sex")
                    {
                        bk.Range.Text = temp.xingbie;
                    }
                    //替换人的头像
                    else if (bk.Name == "image2" && File.Exists(ZFCEPath))
                    {
                        bk.Select();
                        Selection sel = app.Selection;
                        //sel.InlineShapes.AddPicture(ZFCEPath);

                        object Anchor = app.Selection.Range;

                        object LinkToFile = false;
                        object SaveWithDocument = true;
                        //设置图片位置
                        app.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                        InlineShape inlineShape = app.ActiveDocument.InlineShapes.AddPicture(ZFCEPath, ref LinkToFile, ref SaveWithDocument, ref Anchor);

                        inlineShape.Width = 94; // 图片宽度   
                        inlineShape.Height = 127; // 图片高度  
                        //  Microsoft.Office.Interop.Word.Shape cShape = inlineShape.ConvertToShape();
                        // cShape.WrapFormat.Type = WdWrapType.wdWrapNone;  
                    }
                    //r替换印章       
                    else if (bk.Name == "seal" && File.Exists(yinzhangweizhi))
                    {
                        bk.Select();
                        Selection sel = app.Selection;

                        object Anchor = app.Selection.Range;
                        object LinkToFile = false;
                        object SaveWithDocument = true;
                        //设置图片位置
                        app.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                        InlineShape inlineShape = app.ActiveDocument.InlineShapes.AddPicture(yinzhangweizhi, ref LinkToFile, ref SaveWithDocument, ref Anchor);

                        inlineShape.Width = 97; // 图片宽度   
                        inlineShape.Height = 97; // 图片高度  


                        //builder.InsertImage(stream, RelativeHorizontalPosition.Page, left, RelativeVerticalPosition.Page, 30, 75, 75, WrapType.None);

                        //WrapType.None
                        #region 使用Goto函数，跳转到指定书签
                        //object BookMarkName = "sex";
                        //object what = Word.WdGoToItem.wdGoToBookmark;
                        //doc.ActiveWindow.Selection.GoTo(ref what, ref Nothing, ref Nothing, ref BookMarkName);
                        //app.ActiveWindow.Selection.TypeText("Hello!");

                        #endregion



                    }
                    //模拟测试  用， 目的在word 内部设置 印章背景为 透明的，
                    else if (bk.Name == "test1")
                    {
                        bk.Select();
                        Selection sel = app.Selection;

                        object Anchor = app.Selection.Range;
                        object LinkToFile = false;
                        object SaveWithDocument = true;
                        //设置图片位置
                        app.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                        InlineShape inlineShape = app.ActiveDocument.InlineShapes.AddPicture(yinzhangweizhi, ref LinkToFile, ref SaveWithDocument, ref Anchor);

                        inlineShape.Width = 97; // 图片宽度   
                        inlineShape.Height = 97; // 图片高度  

                        //
                        #region 设置 背景透明  参照 word 录的宏 但不成功

                        ////doc.Application.ActiveDocument.InlineShapes[1].Reset();
                        ////doc.Application.ActiveDocument.InlineShapes[1].PictureFormat.TransparentBackground = Microsoft.Office.Core.MsoTriState.msoTrue;
                        ////doc.Application.ActiveDocument.InlineShapes[1].PictureFormat.TransparencyColor = 0;//System.Drawing.Color.FromArgb(255, 0, 0);// RGB(255, 255,253)
                        ////doc.Application.ActiveDocument.InlineShapes[1].Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;



                        ////
                        ////将图片设置为四周环绕型
                        //Microsoft.Office.Interop.Word.Shape s = doc.Application.ActiveDocument.InlineShapes[1].ConvertToShape();
                        //s.WrapFormat.Type = Microsoft.Office.Interop.Word.WdWrapType.wdWrapSquare;
                        ////   //设置图片浮于文字之上 - 查阅WdWrapType的相关WdWrapType Enumeration
                        //s.WrapFormat.Type = WdWrapType.wdWrapNone;
                        //s.PictureFormat.TransparentBackground = Microsoft.Office.Core.MsoTriState.msoCTrue;
                        ////设置背景图片透明
                        ////var blueScreen = RGB(0, 0, 255);
                        //var blueScreen1 = Color.FromArgb(0, 0, 255, 0);
                        ////  s.PictureFormat.TransparencyColor = 0;//blueScreen1;  
                        ////inlineShape.PictureFormat.TransparentBackground = Microsoft.Office.Core.MsoTriState.msoTrue; ;

                        //#region //Bit map
                        ////Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(yinzhangweizhi);

                        ////Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                        //////设置高质量插值法
                        ////g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                        //////设置高质量,低速度呈现平滑程度
                        ////g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //////清空画布并以透明背景色填充
                        ////g.Clear(Color.Transparent); 
                        //#endregion
                        ////
                        //#region 录制的宏
                        ////Selection.InlineShapes(1).Reset
                        ////Selection.InlineShapes(1).PictureFormat.TransparentBackground = msoTrue
                        ////Selection.InlineShapes(1).PictureFormat.TransparencyColor = RGB(255, 255, _
                        ////    253)
                        ////Selection.InlineShapes(1).Fill.Visible = msoFalse
                        //#endregion
                        //doc.Application.ActiveDocument.InlineShapes[1].Reset();
                        //doc.Application.ActiveDocument.InlineShapes[1].PictureFormat.TransparentBackground = Microsoft.Office.Core.MsoTriState.msoTrue;
                        //Color c1 = Color.FromArgb(255, 255, 253);
                        ////doc.Application.ActiveDocument.InlineShapes[1].PictureFormat.TransparencyColor = System.Drawing.Color.FromArgb(255, 0, 0);// RGB(255, 255,253)
                        //doc.Application.ActiveDocument.InlineShapes[1].Fill.Visible = Microsoft.Office.Core.MsoTriState.msoFalse;



                        ////   s.PictureFormat.TransparencyColor = RGB(0, 0, 0); //' RGB(212, 208, 200);
                        ////s.PictureFormat.o = System.Drawing.Color.Beige;
                        #endregion
                    }
                }
                #region 打印word
                //   // 实例化System.Windows.Forms.PrintDialog对象
                //   PrintDialog dialog = new PrintDialog();
                //   dialog.AllowPrintToFile = true;
                //   dialog.AllowCurrentPage = true;
                //   dialog.AllowSomePages = true;
                //   dialog.UseEXDialog = true;
                //   // 关联doc.PrintDialog属性和PrintDialog对象
                //   doc.PrintDialog = dialog;
                //// 

                //   // 后台打印
                //   // PrintDocument printDoc = doc.PrintDocument;       
                //   // printDoc.Print();
                //   // 显示打印对话框并打印
                //   if (dialog.ShowDialog() == DialogResult.OK)
                //   {
                //       //printDoc.Print();
                //   } 


                string defaultPrinter = app.ActivePrinter;
                Externs.SetDefaultPrinter(orderprint);


                doc.PrintOut();
                #endregion


                //对替换好的word模板另存为一个新的word文档
                doc.SaveAs(physicNewFile,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //这边为了捕获Response.End引起的异常
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (doc != null)
                {
                    doc.Close();//关闭word文档
                }
                if (app != null)
                {
                    app.Quit();//退出word应用程序
                }
                //如果文件存在则输出到客户端
                if (File.Exists(physicNewFile))
                {
                    //  Response.WriteFile(physicNewFile);
                }
            }
        }

        #endregion

        public void Run(clsDATAinfo item, string prc_folderpath)
        {
            savepath = prc_folderpath;
            punblic_item = item;


            List<clsDATAinfo> List = new List<clsDATAinfo>();
            List.Add(item);


            LocalReport report = new LocalReport();
            report.ReportPath = System.Windows.Forms.Application.StartupPath + "\\Report1.rdlc";

            report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", List));

            Export(report);
            m_currentPageIndex = 0;


            Print(orderprint, 0, 0);

            //导出 word

            //   btnExportExcel_Click(report);
            //好用 但是 图片不能叠加的现实 
            //btnExportword(report);

        }
        public void Export(LocalReport report)
        {

            // 15.18/  2.54=5.97
            //9.62/2.54=3.78
            string deviceInfo =
                                "<DeviceInfo>" +
                                "  <OutputFormat>EMF</OutputFormat>" +
                                "  <PageWidth>8.5in</PageWidth>" +
                                "  <PageHeight>11in</PageHeight>" +
                                "  <MarginTop>0.0cm</MarginTop>" +
                                "  <MarginLeft>0.0cm</MarginLeft>" +
                                "  <MarginRight>0.0cm</MarginRight>" +
                                "  <MarginBottom>0.0cm</MarginBottom>" +
                                "</DeviceInfo>";



            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream,
               out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {

            //如果需要将报表输出的数据保存为文件，请使用FileStream对象。

            Stream stream = new MemoryStream();

            m_streams.Add(stream);

            return stream;

        }

        public void Print(string defaultPrinterName, int lenpage, int withpage)
        {

            m_currentPageIndex = 0;
            if (m_streams == null || m_streams.Count == 0)
                return;
            //声明PrintDocument对象用于数据的打印

            PrintDocument printDoc = new PrintDocument();

            //指定需要使用的打印机的名称，使用空字符串""来指定默认打印机

            if (defaultPrinterName == "" || defaultPrinterName == null)
                defaultPrinterName = printDoc.PrinterSettings.PrinterName;

            printDoc.PrinterSettings.PrinterName = defaultPrinterName;

            //判断指定的打印机是否可用

            if (!printDoc.PrinterSettings.IsValid)
            {
                MessageBox.Show("Can't find printer");
                return;
            }
            //声明PrintDocument对象的PrintPage事件，具体的打印操作需要在这个事件中处理。

            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);

            //执行打印操作，Print方法将触发PrintPage事件。
            printDoc.DefaultPageSettings.Landscape = false;
            //大小
            if (lenpage != 0)
                printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", lenpage, withpage);


            printDoc.Print();

        }
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);
            StringFormat SF = new StringFormat();
            SF.LineAlignment = StringAlignment.Center;
            SF.Alignment = StringAlignment.Center;
            float left = ev.PageSettings.Margins.Left;//打印区域的左边界
            float top = ev.PageSettings.Margins.Top;//打印区域的上边界
            float width = ev.PageSettings.PaperSize.Width - left - ev.PageSettings.Margins.Right;//计算出有效打印区域的宽度
            float height = ev.PageSettings.PaperSize.Height - top - ev.PageSettings.Margins.Bottom;//计算出有效打印区域的高度

            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        //自动导出excel/pdf/word
        public void btnExportExcel_Click(LocalReport report)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = report.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            FileStream fs = new FileStream(@"c:\output.xls",
               FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

        }
        public void btnExportword(LocalReport report)
        {
            savepath = "C:\\Users\\IBM_ADMIN\\Desktop";
            savepath = savepath + "\\" + punblic_item.xingming + ".doc";

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = report.Render(
               "Word", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            FileStream fs = new FileStream(@savepath,
               FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

        }

        //获取打印机名称
        private void getUserPint()
        {
            try
            {
                RegistryKey rkLocalMachine = Registry.LocalMachine;
                RegistryKey rkSoftWare = rkLocalMachine.OpenSubKey(clsConstant.RegEdit_Key_SoftWare);
                RegistryKey rkAmdape2e = rkSoftWare.OpenSubKey(clsConstant.RegEdit_Key_AMDAPE2E);
                if (rkAmdape2e != null)
                {
                    orderprint = clsCommHelp.encryptString(clsCommHelp.NullToString(rkAmdape2e.GetValue(clsConstant.RegEdit_Key_Order)));

                    rkAmdape2e.Close();
                }
                rkSoftWare.Close();
                rkLocalMachine.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }


    }
}
