using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Order.Common;
using Order.DB;

namespace Dexin.Buiness
{
    public class clsAllnew
    {


        public  List<clsDATAinfo> Read_Excel(string Alist)
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
                conditions = "insert into PeopleData(zhenghao,xingming,xingbie,zuoyeleibie,zhuncaoxiangmu,chulingriqi,youxiangqixian,fushenriqi,Input_Date,Message) values ('" + item.zhenghao + "','" + item.xingming + "','" + item.xingbie + "','" + item.zuoyeleibie + "','" + item.zhuncaoxiangmu + "','" + item.chulingriqi + "','" + item.youxiangqixian + "','" + item.fushenriqi  + "','" + item.Input_Date.ToString("yyyy/MM/dd") + "','" + item.Message + "')";
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

    }
}
