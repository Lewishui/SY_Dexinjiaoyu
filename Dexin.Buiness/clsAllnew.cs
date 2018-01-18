using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Order.Common;
using Order.DB;
using Word = Microsoft.Office.Interop.Word;
namespace Dexin.Buiness
{
    public class clsAllnew
    {
        private string dataSource = "SY_Dexinjiaoyu.sqlite";
        string newsth;
        public BackgroundWorker bgWorker1;

        public clsAllnew()
        {
            newsth = AppDomain.CurrentDomain.BaseDirectory + "" + dataSource;//System\\



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
                    item.Message = Convert.ToString(reader.GetString(19));

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

            string ZFCEPath = AppDomain.CurrentDomain.BaseDirectory + "Resources\\photo\\林光琼.jpg";
            string yinzhangweizhi = AppDomain.CurrentDomain.BaseDirectory + "Resources\\seal\\01.gif";

            Word.Application app = null;
            Word.Document doc = null;
            //将要导出的新word文件名
            string newFile = DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc";
            //  string physicNewFile = Server.MapPath(DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc");
            string physicNewFile = AppDomain.CurrentDomain.BaseDirectory + "Results\\" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc";
            physicNewFile = savefolder+"\\" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc";;

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
                datas.Add("{sex}", temp.xingbie);
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
                    else if (bk.Name == "image2")
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
                    else if (bk.Name == "seal")
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
                      
                    }
                }  
                //对替换好的word模板另存为一个新的word文档
                doc.SaveAs(physicNewFile,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing,
                oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

                //准备导出word
                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "utf-8";
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmssss") + ".doc");
                //Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                //Response.ContentType = "application/ms-word";
                //Response.End();
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
    }
}
