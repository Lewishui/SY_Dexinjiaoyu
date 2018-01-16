using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Order.DB;

namespace Order.Common
{
    public class clsCoverTime
    {


        public static string Catchwenbenshijian(clsR2accrualsapinfo item, string result, string time1)
        {
            try
            {
                if (result != null && result != "" && !item.wenben.ToUpper().Contains("Q1") && !item.wenben.ToUpper().Contains("Q2") && !item.wenben.ToUpper().Contains("Q3") && !item.wenben.ToUpper().Contains("Q4") && !item.wenben.ToUpper().Contains("季度"))
                {
                    DateTime ji1;

                    int index2 = item.wenben.IndexOf(result.Substring(0, 1));
                    int indexyue = item.wenben.IndexOf("月");
                    int indexnian = item.wenben.IndexOf("年");
                    int indexhao = item.wenben.IndexOf("号");
                    int indexri = item.wenben.IndexOf("日");
                    int index20 = item.wenben.IndexOf("20");
                    //判断是否是时间段，包含几个 月或年来判断字符串个数
                    int MMcount = item.wenben.Split('月').Length - 1;
                    int Yearcount = item.wenben.Split('年').Length - 1;

                    if (indexyue > index2 && index2 != -1 && indexyue != -1)
                    {
                        string riqijiequ = "";

                        if (MMcount > 1 || Yearcount > 1)
                        {
                            riqijiequ = item.wenben;
                        }
                        else
                            riqijiequ = item.wenben.Substring(index2, indexyue - index2);
                        string time = "";
                        try
                        {

                            if (riqijiequ.Contains("年"))
                                if (indexyue - indexnian == 2)
                                {
                                    riqijiequ = riqijiequ.Replace("年", "0");
                                    if (riqijiequ.Length == 4)
                                        riqijiequ = "20" + riqijiequ;

                                }
                                else
                                    riqijiequ = riqijiequ.Replace("年", "");

                            if (riqijiequ.Length == 6)
                                riqijiequ = riqijiequ + "01";
                            //else if (riqijiequ.Length == 2)//这么加法危险性太高 
                            //    riqijiequ = DateTime.Now.ToString("yyyy") + riqijiequ + "01";

                            #region 日期格式为17.1-17.3
                            if (riqijiequ.Contains("-"))
                            {
                                time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                            }
                            #endregion
                            else
                                time = clsCommHelp.objToDateTime(riqijiequ);
                            //仅是包含 月没有写年的 1.SAP导出科目明细表，如果(B列)记账日期------当年当月KA凭证的文本没有写年份，默认为当年。
                            //BC上海龙之梦5月租金63440元

                            if (time == "" && Convert.ToDateTime(item.jizhangriqi).Year.ToString() == DateTime.Now.Year.ToString())
                            {
                                if (indexnian < 0 && indexyue > 0 && riqijiequ.Length > 0 && riqijiequ.Length < 3)
                                {
                                    if (riqijiequ.Length == 1)
                                        time = DateTime.Now.Year.ToString() + "/0" + riqijiequ + "/01";
                                    else
                                        time = DateTime.Now.Year.ToString() + "/" + riqijiequ;
                                }
                                //杭州西溪印象城NC6到7月固定租金2017
                                else if (indexyue > 0 && riqijiequ.Length > 0)//indexnian < 0 &&
                                {
                                    riqijiequ = item.wenben;

                                    string bresult = "";
                                    string aresult = "";
                                    if (riqijiequ.Contains("到") || riqijiequ.Contains("至"))
                                    {
                                        if (riqijiequ.Contains("到"))
                                        {
                                            if (item.wenben.Contains(" "))
                                            {
                                                item.wenben = item.wenben.Replace(" ", "");
                                                index20 = item.wenben.IndexOf("20");
                                            }
                                            string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "到");
                                            int indexnianweizhi = riqijiequ.IndexOf("20");
                                            int indexdaoweizhi = riqijiequ.IndexOf("到");
                                            if (result != null && result != "" && result.Contains("20") && indexnianweizhi > indexdaoweizhi)
                                            {


                                                if (temp3[0].Length > 2)
                                                {
                                                    aresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[0].Substring(0, 2) + "/" + temp3[0].Substring(2, temp3[0].Length - 2);


                                                    //杭州西溪印象城NC6到7月固定租金2017

                                                    string s = item.wenben;
                                                    int indexdaoyue = s.IndexOf("到");


                                                    char[] ca = s.ToCharArray();
                                                    Array.Reverse(ca);
                                                    s = new string(ca);
                                                    int indexend = 0;


                                                    var cname = s.ToCharArray();
                                                    //检查字母和汉字所在的位置
                                                    indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);
                                                    int zhengshujiequweizhi = item.wenben.Length - indexend;

                                                    int blanki = System.Text.RegularExpressions.Regex.Matches(item.wenben.Substring(0, indexend), " ").Count;
                                                    if (blanki != 0)
                                                    {
                                                        indexdaoyue = blanki + indexdaoyue;
                                                        zhengshujiequweizhi = zhengshujiequweizhi + blanki;

                                                    }
                                                    if (index20 >= 0)
                                                    {
                                                        s = item.wenben;
                                                        cname = s.ToCharArray();
                                                        int index20end = 0;
                                                        string ri = "";
                                                        index20end = Checkchinese_shuzi(index20, index20end, cname);
                                                        if (index20end == 0)
                                                            index20end = item.wenben.Length;
                                                        if (indexdaoyue - zhengshujiequweizhi >= 1 && indexdaoyue - zhengshujiequweizhi <= 2)
                                                            ri = "/01";
                                                        aresult = item.wenben.Substring(index20, index20end - index20) + "/" + item.wenben.Substring(zhengshujiequweizhi, indexdaoyue - zhengshujiequweizhi) + ri;

                                                    }
                                                    //  riqijiequ = item.wenben.Substring(item.wenben.Length - indexend, yue_i - zhengshujiequweizhi + blanki).Replace(".", "/");
                                                }
                                                else
                                                    aresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[0] + "/01";

                                                if (temp3[1].Length > 2)
                                                {
                                                    //  bresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[1].Substring(0, 2) + "/" + temp3[1].Substring(2, temp3[0].Length - 2);

                                                    //杭州西溪印象城NC6到7月固定租金2017
                                                    string s = item.wenben;

                                                    int indexend = 0;

                                                    int indexdaoyue = s.IndexOf("到") + 1;
                                                    var cname = s.ToCharArray();
                                                    //检查字母和汉字所在的位置
                                                    indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);
                                                    int zhengshujiequweizhi = indexend;

                                                    int blanki = System.Text.RegularExpressions.Regex.Matches(item.wenben.Substring(0, indexend), " ").Count;
                                                    if (index20 >= 0)
                                                    {
                                                        s = item.wenben;
                                                        cname = s.ToCharArray();
                                                        int index20end = 0;
                                                        string ri = "";
                                                        index20end = Checkchinese_shuzi(index20, index20end, cname);
                                                        if (index20end == 0)
                                                            index20end = item.wenben.Length;
                                                        if (zhengshujiequweizhi - indexdaoyue >= 1 && zhengshujiequweizhi - indexdaoyue <= 2)
                                                            ri = "/28";
                                                        bresult = item.wenben.Substring(index20, index20end - index20) + "/" + item.wenben.Substring(indexdaoyue, zhengshujiequweizhi - indexdaoyue) + ri;

                                                    }


                                                }
                                                else
                                                    bresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[1] + "/01";

                                            }
                                            else if (result != null && result != "" && !result.Contains("20"))
                                            {

                                                string nian = "";
                                                string yue = "";
                                                string ri = "";
                                                string timeend = "";

                                                int indexniani = temp3[0].IndexOf("年");
                                                int indexyuei = temp3[0].IndexOf("月");
                                                int indexhaoi = temp3[0].IndexOf("号");
                                                int indexhaori = temp3[0].IndexOf("日");

                                                if (indexyuei > 0 && indexniani < 0)
                                                {
                                                    //    time1 = year_split(time, timeend);
                                                    return NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                                                }
                                                return NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);

                                            }
                                            //扬州三盛PD201704月到6月物业费
                                            if (result != null && result != "" && result.Contains("20") && indexnianweizhi < indexdaoweizhi)
                                            {
                                                aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
                                                if (temp3[0].Substring(temp3[0].Length - 1, 1) == "月")
                                                {
                                                    aresult = aresult.Substring(0, 4) + "/" + aresult.Replace(aresult.Substring(0, 4), "") + "/01";
                                                }
                                                else if (temp3[0].Substring(temp3[0].Length - 1, 1) != "月")
                                                {
                                                    aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                                                    if (aresult.Substring(0, 1) == "/")
                                                        aresult = aresult.Substring(1, aresult.Length - 1);
                                                    int pie_count = System.Text.RegularExpressions.Regex.Matches(aresult, "/").Count;
                                                    aresult = aresultNewMethod(aresult);
                                                }
                                                int yue2weizhi = temp3[1].IndexOf("月");
                                                if (yue2weizhi < 3)
                                                {
                                                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + temp3[1].Substring(0, yue2weizhi) + "/28";

                                                }
                                            }
                                            bresult = bresultNewMethod(bresult);
                                            aresult = aresultNewMethod(aresult);
                                        }
                                        else if (riqijiequ.Contains("至"))
                                        {
                                            zhimethod(result, riqijiequ, ref bresult, ref aresult);
                                        }
                                    }
                                    if (bresult != "" && aresult != "")
                                    {

                                        time = year_split(aresult, bresult);
                                    }
                                }
                            }
                            if (time == "" && indexyue > 0 && indexnian < 0)
                                time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                            else if (time == "" && indexyue > 0 && indexnian > 0)
                            {
                                time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                            }
                        }
                        catch
                        {

                            try
                            {
                                if (riqijiequ.Length == 6)
                                    riqijiequ = riqijiequ + "01";
                                time = clsCommHelp.objToDateTime1(riqijiequ);
                            }
                            catch
                            {
                            }
                        }

                        time1 = time;
                        if (time1 == "")
                        {
                            riqijiequ = "";
                            time1 = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                        }
                    }
                    else
                    {
                        string time = "";
                        try
                        {
                            if (result.Length == 6)
                                result = result + "01";
                            else if (result.Length == 4)
                                result = "20" + result + "01";
                            else if (result.Length == 5)
                            {
                                DateTime dt = Convert.ToDateTime(result);
                                time = clsCommHelp.objToDateTime(result);
                            }
                            time = clsCommHelp.objToDateTime(result);
                            if (time == "")
                                time = clsCommHelp.objToDateTime1(result);
                            //格式              NC虹口龙之梦17.01.28-02.28电费
                            if (time == "" && item.wenben != null && item.wenben.Contains("-"))
                            {

                                string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "-");
                                string aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
                                string bresult = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
                                if (aresult.Length == 6)
                                {
                                    string riqijiequ = "";
                                    time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                                }

                            }
                        }
                        catch
                        {

                            try
                            {
                                time = clsCommHelp.objToDateTime1(result);
                            }
                            catch
                            {

                            }
                        }

                        if (time == "")
                        {
                            string bresult = "";
                            string aresult = "";
                            if (item.wenben.Contains("至"))
                            {
                                item.wenben = item.wenben.Replace("至", "-");
                                string riqijiequ = "";
                                time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                                //  zhimethod(result, item.wenben, ref bresult, ref aresult);
                                item.wenben = item.wenben.Replace("-", "至");

                            }
                            else
                            {

                                string riqijiequ = "";
                                time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                            }
                        }
                        time1 = time;
                    }
                }
                else
                {
                    if (item.wenben != null && item.wenben != "")
                    {
                        string aresult = "";
                        string bresult = "";
                        if (item.wenben.Contains("第一季度") || item.wenben.Contains("第二季度") || item.wenben.Contains("第三季度") || item.wenben.Contains("第四季度") || item.wenben.Contains("3季度") || item.wenben.Contains("4季度") || item.wenben.Contains("1季度") || item.wenben.Contains("2季度"))
                        {
                            if (item.wenben.Contains("第一季度") || item.wenben.Contains("1季度"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/01/01";
                                bresult = DateTime.Now.Year.ToString() + "/03/31";
                            }
                            else if (item.wenben.Contains("第二季度") || item.wenben.Contains("2季度"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/04/01";
                                bresult = DateTime.Now.Year.ToString() + "/06/30";
                            }
                            else if (item.wenben.Contains("第三季度") || item.wenben.Contains("3季度"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/07/01";
                                bresult = DateTime.Now.Year.ToString() + "/09/30";
                            }
                            else if (item.wenben.Contains("第一季度") || item.wenben.Contains("4季度"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/10/01";
                                bresult = DateTime.Now.Year.ToString() + "/12/30";
                            }

                        }
                        else if (item.wenben.ToUpper().Contains("Q1") || item.wenben.ToUpper().Contains("Q2") || item.wenben.ToUpper().Contains("Q3") || item.wenben.ToUpper().Contains("Q4"))
                        {
                            if (item.wenben.Contains("Q1"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/01/01";
                                bresult = DateTime.Now.Year.ToString() + "/03/31";
                            }
                            else if (item.wenben.Contains("Q2"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/04/01";
                                bresult = DateTime.Now.Year.ToString() + "/06/30";
                            }
                            else if (item.wenben.Contains("Q3"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/07/01";
                                bresult = DateTime.Now.Year.ToString() + "/09/30";
                            }
                            else if (item.wenben.Contains("Q4"))
                            {
                                aresult = DateTime.Now.Year.ToString() + "/10/01";
                                bresult = DateTime.Now.Year.ToString() + "/12/30";
                            }
                        }
                        if (bresult != "" && aresult != "")
                            time1 = year_split(aresult, bresult);
                    }
                }

                return time1;
            }
            catch (Exception EX)
            {

                throw;
            }
        }

        private static void zhimethod(string result, string riqijiequ, ref string bresult, ref string aresult)
        {
            string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "至");
            if (result != null && result != "" && result.Contains("20"))
            {
                if (temp3[0].Length > 2)
                    aresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[0].Substring(0, 2) + "/" + temp3[0].Substring(2, temp3[0].Length - 2);
                else
                    aresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[0] + "/01";

                if (temp3[1].Length > 2 && temp3[0].Length - 2 < temp3[1].Length)
                    bresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[1].Substring(0, 2) + "/" + temp3[1].Substring(2, temp3[0].Length - 2);
                else
                    bresult = result.Replace(temp3[0] + temp3[1], "") + "/" + temp3[1] + "/01";

                bool ischina1 = newHasChineseTest(aresult);
                bool ischina2 = newHasChineseTest(bresult);
                //上海奕欧来2017年1月1日至3月31日租金预付申请
                if (ischina1 == true && aresult.Length > 10)
                {
                    int nian = temp3[0].IndexOf("年");
                    int yue = temp3[0].IndexOf("年");
                    int ri = temp3[0].IndexOf("年");
                    int hao = temp3[0].IndexOf("年");
                    int index20 = temp3[0].IndexOf("20");

                    if (nian > 0 && yue > 0)
                    {
                        if (hao > 0 || ri > 0)
                        {
                            string a1 = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                            if (a1.Length >= 8 && a1.Length <= 10)
                                aresult = aresultNewMethod(a1);
                        }

                    }
                }
                if (ischina2 == true && bresult.Length > 10)
                {
                    int nian = temp3[1].IndexOf("年");
                    int yue = temp3[1].IndexOf("月");
                    int ri = temp3[1].IndexOf("日");
                    int hao = temp3[1].IndexOf("号");
                    int index20 = temp3[1].IndexOf("20");

                    if (nian < 0 && yue > 0)
                    {
                        if (hao > 0 || ri > 0)
                        {
                            string a1 = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                            if (a1.Length >= 8 && a1.Length <= 10)
                                bresult = aresultNewMethod(a1);
                            if (a1.Length <= 5)
                            {
                                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + a1;
                                bresult = aresultNewMethod(bresult);
                            }

                        }

                    }
                }
            }
        }

        private static string SPLNewMethod(clsR2accrualsapinfo item, ref string time1, ref int index2, ref int indexyue, ref int indexnian, ref string riqijiequ, ref string time)
        {
            try
            {
                time = "";
                time1 = "";
                string newresult = System.Text.RegularExpressions.Regex.Replace(item.wenben, @"[^0-9]+", "");
                string[] tempsplit_All = System.Text.RegularExpressions.Regex.Split(item.wenben, "-");

                int nian_i = tempsplit_All[0].IndexOf("年");
                int yue_i = tempsplit_All[0].IndexOf("月");
                int hao_i = tempsplit_All[0].IndexOf("号");
                int ri_i = tempsplit_All[0].IndexOf("日");
                int indexhao = item.wenben.IndexOf("号");
                int indexri = item.wenben.IndexOf("日");
                int index20 = item.wenben.IndexOf("20");
                int MMcount = item.wenben.Split('月').Length - 1;
                int yearcount = item.wenben.Split('年').Length - 1;
                int haocount = item.wenben.Split('号').Length - 1;
                int ricount = item.wenben.Split('日').Length - 1;
                int yue_valoume = System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count;


                if (riqijiequ != null && riqijiequ.Contains("-") && newresult.Length >= 6)
                {

                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                    if (temp3[0].Length == 4 && temp3[0].Contains("."))
                    {
                        temp3[0] = "20" + temp3[0].Replace(".", "0") + "01";
                        temp3[0] = clsCommHelp.objToDateTime(temp3[0]);
                    }
                    else if (temp3[0].Length == 6 && temp3[0].Contains("."))
                    {
                        time = clsCommHelp.objToDateTime(temp3[0]);

                    }
                    if (temp3[1].Length == 4 && temp3[1].Contains("."))
                    {
                        try
                        {
                            temp3[1] = "20" + temp3[1].Replace(".", "0") + "01";
                            temp3[1] = clsCommHelp.objToDateTime(temp3[1]);

                        }
                        catch
                        {


                        }
                    }
                    else if (temp3[1].Length == 1 || temp3[1].Length == 2)
                    {
                        if (temp3[1].Length == 1 && time != "")
                            temp3[1] = Convert.ToDateTime(time).Year.ToString() + "0" + temp3[1] + "01";
                        else if (time != "")
                            temp3[1] = Convert.ToDateTime(time).Year.ToString() + temp3[1] + "01";

                    }

                    if (time == "")
                    {
                        string datari = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
                        if (datari.Length == 6 && datari.Substring(0, 2) == "20" && ri_i < 0 && indexhao < 0)
                        {
                            datari = datari + "01";
                            time = clsCommHelp.objToDateTime(datari);
                        }
                    }
                    string timeend = "";
                    if (temp3[1].Replace(".", "").Length > 4)
                        try
                        {
                            string datari = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
                            if (datari.Length == 6 && datari.Substring(0, 2) == "20")
                                datari = datari + "01";
                            //NC天津银河2016.11-2017.5月管理费36212.54元清帐
                            else if (datari.Length == 5 && datari.Substring(0, 2) == "20")
                                datari = datari.Substring(0, 4) + "/0" + datari.Substring(4, 1) + "/01";
                            timeend = clsCommHelp.objToDateTime(datari);
                        }
                        catch
                        {
                        }
                    else
                    {
                        int indexniani = temp3[1].IndexOf("月");
                        string yue = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                        if (indexniani < 0)
                        {
                            if (!yue.Contains("/") && yue.Length == 4)
                                yue = yue.Substring(0, 2) + "/" + yue.Substring(2, 2);
                            if (!yue.Contains("/") && yue.Length == 2)
                                yue = yue.Substring(0, 2) + "/" + "01";
                            if (time != "")
                                timeend = Convert.ToDateTime(time).Year.ToString() + "/" + yue;
                        }
                        else if (indexniani == 1)
                        {
                            if (yue.Contains("/") && yue.Length == 2)
                                yue = System.Text.RegularExpressions.Regex.Replace(yue, @"[^0-9]+", "");
                            if (time != "")
                                timeend = Convert.ToDateTime(time).Year.ToString() + "/" + yue + "/" + "01"; ;

                        }
                    }
                    if (timeend != "" && time != "")
                    {
                        time1 = year_split(time, timeend);
                    }
                    else
                    {
                        string nian = "";
                        string yue = "";
                        string ri = "";
                        // demo   EC港汇广场16年8月1号-8月31号租金20777.09元
                        int indexniani = tempsplit_All[0].IndexOf("年");
                        int indexyuei = tempsplit_All[0].IndexOf("月");
                        int indexhaoi = tempsplit_All[0].IndexOf("号");
                        int indexhaori = tempsplit_All[0].IndexOf("日");
                        int yue_count = 0;
                        if (indexhaoi < 0 && indexyuei > 0)
                        {
                            if (tempsplit_All[0].Length >= indexyuei + 3)
                            {
                                string risp = tempsplit_All[0].Substring(indexyuei, 3);//EC虹口2016年9月28
                                //如果出现 TT天津新业2016年1月2月物业费-清账
                                yue_count = System.Text.RegularExpressions.Regex.Matches(risp, "月").Count;
                                if (haocount == 0 && ricount == 0 && yue_count == 2)
                                {
                                    ri = "01";
                                }
                                else
                                {
                                    //
                                    ri = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                                    if (ri.Length == 1)
                                        ri = "0" + ri;
                                }
                            }
                        }
                        else if (indexyuei > 0)
                        {
                            //读取日
                            string hao = tempsplit_All[0].Substring(indexyuei, indexhaoi - indexyuei);
                            ri = System.Text.RegularExpressions.Regex.Replace(hao, @"[^0-9]+", "");
                            if (ri.Length == 1)
                                ri = "0" + ri;

                        }
                        //读取月
                        if (indexyuei > 0 && indexniani > 0)
                        {
                            string y = tempsplit_All[0].Substring(indexniani, indexyuei - indexniani);
                            yue = System.Text.RegularExpressions.Regex.Replace(y, @"[^0-9]+", "");
                            if (yue.Length == 1)
                                yue = "0" + yue;
                        }

                        #region 没有年 的情况
                        //BC上海龙之梦5月1日-5月17租金63440元
                        else if (indexyuei > 0 && indexniani < 0)
                        {
                            //    time1 = year_split(time, timeend);
                            return NO_yearTime1(item, ref time1, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        }
                        //BC上海龙之梦3-6月租金63440元
                        else if (indexyuei < 0 && indexniani < 0 && tempsplit_All.Length == 2)
                        {

                            if (tempsplit_All.Length == 2 || tempsplit_All.Length == 3)
                            {
                                return NO_yearTime_end(ref time1, riqijiequ, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                            }
                        }
                        #endregion

                        //读取年
                        if (indexniani > 0)
                        {
                            string n = tempsplit_All[0].Substring(index2, indexniani - index2);
                            nian = System.Text.RegularExpressions.Regex.Replace(n, @"[^0-9]+", "");
                            if (nian.Length == 1)
                            {
                                time1 = "日期无法识别";
                                return time1;
                            }
                            else if (nian.Length == 2)
                                nian = "20" + nian;
                            else if (nian.Length == 3)
                                nian = "201" + nian;
                            else if (nian.Length > 4)
                            {
                                time1 = "日期无法识别";
                                return time1;
                            }
                        }
                        time = "";
                        if (nian != "")
                            time += nian + "年";
                        if (yue != "")
                            time += yue + "月";
                        if (ri != "")
                            time += ri + "日";

                        ///后半段 8月31号租金20777.09元
                        ///
                        indexniani = tempsplit_All[1].IndexOf("年");
                        indexyuei = tempsplit_All[1].IndexOf("月");
                        indexhaoi = tempsplit_All[1].IndexOf("号");
                        if (indexhaoi < 0 && indexyuei > 0)
                        {
                            string risp = tempsplit_All[1].Substring(indexyuei, 3);//EC虹口2016年9月28
                            ri = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                            if (ri.Length == 1)
                                ri = "0" + ri;
                        }
                        else if (indexyuei > 0)
                        {
                            //读取日
                            string hao = tempsplit_All[1].Substring(indexyuei, indexhaoi - indexyuei);
                            ri = System.Text.RegularExpressions.Regex.Replace(hao, @"[^0-9]+", "");
                            if (ri.Length == 1)
                                ri = "0" + ri;

                        }
                        //读取月
                        yue = "";
                        if (indexyuei > 0 && indexniani > 0)
                        {
                            string y = tempsplit_All[1].Substring(indexniani, indexyuei - indexniani);
                            yue = System.Text.RegularExpressions.Regex.Replace(y, @"[^0-9]+", "");
                            if (yue.Length == 1)
                                yue = "0" + yue;
                        }
                        else if (indexyuei > 0 && indexniani < 0)
                        {
                            string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[1], "月");
                            yue = System.Text.RegularExpressions.Regex.Replace(yues[0], @"[^0-9]+", "");
                            if (yue.Length == 1)
                                yue = "0" + yue;
                        }

                        //读取年
                        //PC协信16年9月-17年3月电费
                        if (tempsplit_All[1].Contains("年"))
                        {
                            nian = "";
                            index2 = tempsplit_All[1].IndexOf(System.Text.RegularExpressions.Regex.Replace(tempsplit_All[1], @"[^0-9]+", "").Substring(0, 1));
                        }
                        //
                        if (indexniani > 0)
                        {
                            if (indexniani > index2)
                            {
                                string n = tempsplit_All[0].Substring(index2, indexniani - index2);
                                if (tempsplit_All[1].Contains("年"))
                                {
                                    n = tempsplit_All[1].Substring(index2, indexniani - index2);
                                }
                                nian = System.Text.RegularExpressions.Regex.Replace(n, @"[^0-9]+", "");
                            }
                            if (nian.Length == 1)
                            {
                                time1 = "日期无法识别";
                                return time1;
                            }
                            else if (nian.Length == 2)
                                nian = "20" + nian;
                            else if (nian.Length == 3)
                                nian = "201" + nian;
                            else if (nian.Length > 4)
                            {
                                time1 = "日期无法识别";
                                return time1;
                            }
                        }
                        else if (indexyuei < 3 && indexyuei > 0 && indexniani < 0 && time != "")
                        {

                            nian = Convert.ToDateTime(time).Year.ToString();

                        }
                        timeend = "";
                        if (nian != "")
                            timeend += nian + "年";
                        if (yue != "")
                            timeend += yue + "月";
                        else if (yue_count == 2)
                        {
                            indexyuei = tempsplit_All[0].IndexOf("月");
                            string risp = tempsplit_All[0].Substring(indexyuei, 3);//EC虹口2016年9月28
                            yue = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                            timeend += yue + "月";
                            ri = "27";
                        }
                        if (ri != "")
                            timeend += ri + "日";
                        //格式 是//201611.10
                        if (time == "" && temp3.Length > 0 && temp3[0].Contains("."))
                        {
                            string t1 = temp3[0].Replace(".", "");//2016.10
                            if (t1.Length == 6)
                            {
                                t1 = t1 + "01";
                                time = clsCommHelp.objToDateTime(t1.Replace(".", ""));
                            }
                            else
                                time = clsCommHelp.objToDateTime(temp3[0].Replace(".", ""));
                        }
                        //后半段是11.30
                        if (time != "" && temp3.Length > 1 && !temp3[1].Contains("201") && System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "").Length > 0 && !timeend.Contains("月"))
                        {
                            yue = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                            if (indexniani < 0)
                            {
                                if (!yue.Contains("/") && yue.Length == 4)
                                    yue = yue.Substring(0, 2) + "/" + yue.Substring(2, 2);
                                if (!yue.Contains("/") && yue.Length == 2)
                                    yue = yue.Substring(0, 2) + "/" + "01";

                                timeend = Convert.ToDateTime(time).Year.ToString() + "/" + yue;
                            }
                        }

                        if (time.Length == 8 && time.Contains("年") && time.Contains("月") && !time.Contains("日"))
                            time = time + "01日";
                        if (timeend.Length == 8 && timeend.Contains("年") && timeend.Contains("月") && !timeend.Contains("日"))
                            timeend = timeend + "28日";
                        if (time != null && time != "" && timeend != null && timeend != "")
                            time1 = year_split(time, timeend);

                    }
                }
                #region 没有年 的情况 2
                //NC江桥4-6月店铺物业管理费  ///EE 武汉经开万达9-11月物业  是3位数
                else if (riqijiequ != null && riqijiequ.Contains("-") && newresult.Length >= 2 && newresult.Length <= 4)
                {
                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    string timeend = "";
                    // demo   EC港汇广场16年8月1号-8月31号租金20777.09元
                    int indexniani = tempsplit_All[0].IndexOf("年");
                    int indexyuei = tempsplit_All[0].IndexOf("月");
                    int indexhaoi = tempsplit_All[0].IndexOf("号");
                    int indexhaori = tempsplit_All[0].IndexOf("日");
                    if (indexyuei > 0 && indexniani < 0)
                    {
                        //    time1 = year_split(time, timeend);
                        return NO_yearTime1(item, ref time1, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                    }
                    //BC上海龙之梦3-6月租金63440元
                    else if (indexyuei < 0 && indexniani < 0 && tempsplit_All.Length == 2)
                    {



                        if (tempsplit_All.Length == 2 || tempsplit_All.Length == 3)
                        {
                            return NO_yearTime_end(ref time1, riqijiequ, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        }
                    }
                }
                //PO 上海江桥万达6.1-8.17POS 租金
                else if (riqijiequ != null && riqijiequ == "" && item.wenben.Contains("-") && tempsplit_All.Length == 2 && tempsplit_All[0].Contains(".") && tempsplit_All[1].Contains(".") && tempsplit_All[0].IndexOf("月") < 0 && tempsplit_All[0].IndexOf("年") < 0 && tempsplit_All[0].IndexOf("日") < 0 && tempsplit_All[1].IndexOf("月") < 0 && tempsplit_All[1].IndexOf("年") < 0 && tempsplit_All[1].IndexOf("日") < 0 && newresult.Length == 5)
                {
                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    string timeend = "";

                    int indexniani = tempsplit_All[0].IndexOf("年");
                    int indexyuei = tempsplit_All[0].IndexOf("月");
                    int indexhaoi = tempsplit_All[0].IndexOf("号");
                    int indexhaori = tempsplit_All[0].IndexOf("日");
                    return NO_yearTime_end(ref time1, riqijiequ, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);

                }
                //PO 上海莘庄仲盛4.6-5.6电费
                else if (riqijiequ != null && riqijiequ == "" && item.wenben.Contains("-") && tempsplit_All.Length == 2 && tempsplit_All[0].Contains(".") && tempsplit_All[1].Contains(".") && tempsplit_All[0].IndexOf("月") < 0 && tempsplit_All[0].IndexOf("年") < 0 && tempsplit_All[0].IndexOf("日") < 0 && tempsplit_All[1].IndexOf("月") < 0 && tempsplit_All[1].IndexOf("年") < 0 && tempsplit_All[1].IndexOf("日") < 0 && newresult.Length == 4)
                {
                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    string timeend = "";
                    int indexniani = tempsplit_All[0].IndexOf("年");
                    int indexyuei = tempsplit_All[0].IndexOf("月");
                    int indexhaoi = tempsplit_All[0].IndexOf("号");
                    int indexhaori = tempsplit_All[0].IndexOf("日");

                    if (indexyuei > 0 && indexniani < 0)
                    {
                        //    time1 = year_split(time, timeend);
                        return NO_yearTime1(item, ref time1, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                    }

                    return NO_yearTime_end(ref time1, riqijiequ, ref time, tempsplit_All, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);

                }

                #endregion
                else if (newresult != "" && newresult.Length == 5 && !item.wenben.Contains("-") && !item.wenben.ToUpper().Contains("Q1") && !item.wenben.ToUpper().Contains("Q2") && !item.wenben.ToUpper().Contains("Q3") && !item.wenben.ToUpper().Contains("Q4"))
                {
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    //读取年
                    if (newresult.Length > 0)
                    {
                        if (index20 >= 0 && newresult.Substring(0, 2) == "20")
                        {
                            nian = newresult.Substring(0, 4);
                        }
                        yue = newresult.Substring(4, 1);

                        if (yue.Length == 1)
                        {
                            yue = "0" + yue;

                        }
                        ri = "01";
                        if (nian != "")
                            time1 = nian + "/" + yue + "/" + ri;
                        else if (index20 < 0)
                        {
                            //NC 武汉1818凯德5月份提成租金
                            time1 = DateTime.Now.Year.ToString() + "/" + yue + "/" + "01";
                        }
                    }

                }

               // NC虹口龙之梦17.01.28-02.28电费
                else if (time == "" && item.wenben != null && item.wenben.Contains("-"))
                {
                    Split_henggang(item, ref time1, indexyue, indexnian, ref riqijiequ, time, newresult, indexhao, indexri, index20);
                }
                //EE莘庄仲盛2017年5.6~6.6电费
                else if (time == "" && item.wenben != null && item.wenben.Contains("~"))
                {
                    item.wenben = item.wenben.Replace("~", "-");
                    Split_henggang(item, ref time1, indexyue, indexnian, ref riqijiequ, time, newresult, indexhao, indexri, index20);
                }

                //NC苏州绿宝2017.4租金47825.65
                else if (time == "" && item.wenben != null && !item.wenben.Contains("-") && newresult != "" && newresult.Length >= 8 && !item.wenben.Contains("年") && newresult.Substring(0, 2) == "20")
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //判断从第一个数据后数到——字母或汉字的位置

                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();
                        bool zimu = false;

                        bool ischina = newHasChineseTest(jdd);
                        if (Regex.Matches(jdd.ToString(), "[a-zA-Z]").Count > 0)
                            zimu = true;

                        if (ischina == true || zimu == true)
                        {
                            if (ic > index2)
                            {
                                indexend = ic;
                                break;
                            }
                        }
                    }
                    if (indexend != 0)
                    {
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");
                        riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                        if (riqijiequ.Length <= 8)
                        {
                            string nian = "";
                            string yue = "";
                            string ri = "";
                            //RC北京金源燕莎MALL    2017.5.1日物业费2089
                            if (riqijiequ.Contains("/") && System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count == 2 && riqijiequ.Substring(riqijiequ.Length - 1, 1) != "/" && Regex.Matches(riqijiequ.Substring(riqijiequ.Length - 1, 1).ToString(), "[a-zA-Z]").Count <= 0 && newHasChineseTest(riqijiequ.Substring(riqijiequ.Length - 1, 1)) == false)
                            {

                            }
                            else if (riqijiequ.Contains("/"))
                                riqijiequ = riqijiequ + "/" + "01";
                            else
                            {
                                //朝北TZ201606租金39611.7
                                if (index20 > 0 && riqijiequ.Substring(0, 2) == "20")
                                {
                                    nian = riqijiequ.Substring(0, 4);
                                }
                                if (riqijiequ.Length == 6)

                                    yue = riqijiequ.Substring(4, 2);

                                if (yue.Length == 1)
                                {
                                    yue = "0" + yue;

                                }
                                ri = "01";
                                if (nian != "")
                                    riqijiequ = nian + "/" + yue + "/" + ri;

                            }

                            time1 = riqijiequ.Replace("//", "/");
                        }
                    }
                    else
                    {
                        //rc天津大悦城租金POS2016.04+2016.06
                        if (time == "" && item.wenben != null && item.wenben.Contains(".") && item.wenben.Contains("+") && newresult != "")
                        {
                            string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben.ToString().Replace("+", "-"), "-");

                            item.wenben = item.wenben.ToString().Replace("+", "-");

                            // time = SPLNewMethod(item, ref time1, ref index2, ref indexyue, ref indexnian, ref riqijiequ, ref time);
                            if (time == "" && item.wenben != null && item.wenben.Contains("-"))
                            {
                                Split_jiahao(item, ref time1, indexyue, indexnian, ref riqijiequ, time, newresult, indexhao, indexri, index20);
                            }
                            item.wenben = item.wenben.ToString().Replace("-", "+");
                        }

                    }
                }
                //WA仲盛17/4月租金。
                else if (time == "" && item.wenben != null && item.wenben.Contains("/") && newresult != "" && newresult.Length == 3 && yue_i > 0)
                {
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count;
                    string result = "";
                    string result2 = "";
                    string[] temp2 = tiqushijian(item, item, ref result, ref result2);
                    index2 = item.wenben.IndexOf(result.Substring(0, 1));
                    if (pie_count == 2 && index2 > yue_i)
                    {
                        yue_i = item.wenben.LastIndexOf("月");

                    }
                    riqijiequ = item.wenben.Substring(index2, yue_i - index2);
                    if (riqijiequ.Substring(0, 2) != "20")
                        time1 = "20" + riqijiequ + "/01";

                }
                //WA长泰17/1提成租金。
                else if (time == "" && item.wenben != null && item.wenben.Contains("/") && newresult != "" && newresult.Length == 3 && yue_i < 0)
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index2)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");
                        if (riqijiequ.Length < 8)
                        {
                            riqijiequ = riqijiequ + "/" + "01";
                            time1 = riqijiequ.Replace("//", "/");
                            if (riqijiequ.Substring(0, 2) != "20")
                                time1 = "20" + riqijiequ;
                        }
                    }
                }

                //WA五角场17年4月/5月固定租金。
                else if (time == "" && item.wenben != null && item.wenben.Contains("/") && newresult != "" && newresult.Length == 4 && yue_i > 0 && nian_i > 0 & hao_i < 0 && ri_i < 0)
                {
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    string timeend = "";
                    int yue_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count;
                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "月");
                    int indexniani = temp3[0].IndexOf("年");
                    string n = tempsplit_All[0].Substring(index2, indexniani - index2);
                    nian = System.Text.RegularExpressions.Regex.Replace(n, @"[^0-9]+", "");
                    if (nian.Length == 1)
                    {
                        time1 = "日期无法识别";
                        return time1;
                    }
                    else if (nian.Length == 2)
                        nian = "20" + nian;
                    else if (nian.Length == 3)
                        nian = "201" + nian;
                    else if (nian.Length > 4)
                    {
                        time1 = "日期无法识别";
                        return time1;
                    }
                    //月
                    //读取月
                    if (indexniani > 0)
                    {
                        string y = temp3[0].Substring(indexniani, temp3[0].Length - indexniani);
                        yue = System.Text.RegularExpressions.Regex.Replace(y, @"[^0-9]+", "");
                        if (yue.Length == 1)
                            yue = "0" + yue;
                    }
                    time = "";
                    if (nian != "")
                        time += nian + "/";
                    if (yue != "")
                        time += yue + "/";
                    if (ri == "")
                        time += "01";
                    yue = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
                    if (yue != "")
                    {

                        timeend += yue.ToString() + "/";
                    }
                    timeend = Convert.ToDateTime(time).Year.ToString() + "/" + timeend;

                    if (ri == "")
                        timeend += "30";

                    if (time != null && time != "" && timeend != null && timeend != "")
                        time1 = year_split(time, timeend);
                }
                //补提201701PD 上海环贸广场固定租金2
                else if (time == "" && item.wenben != null && !item.wenben.Contains("-") && newresult != "" && newresult.Length <= 7 && newresult.Length >= 6 && yearcount < 2 && MMcount < 2 && indexyue < 0)
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index2)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        //截取日期
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");

                        if (riqijiequ.Length > 8)
                        {
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "");
                            riqijiequ = riqijiequ + "" + "01";
                            if (riqijiequ != null && riqijiequ.Length == 8 && riqijiequ.Substring(0, 2) == "20")
                                riqijiequ = DateTime.Parse(riqijiequ.ToString().Substring(0, 4) + "-" +
                                                          riqijiequ.ToString().Substring(4, 2) + "-" +
                                                          riqijiequ.ToString().Substring(6, 2)).ToString("yyyy/MM/dd");

                            time1 = riqijiequ.Replace("//", "/");

                        }
                        else
                        {

                            //[清帐]SA大连柏威年店6月物业费清帐 156440元
                            if (riqijiequ.Length == 1)
                                riqijiequ = "0" + riqijiequ;
                            //泸州万象汇BC 17.7pos租金
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                            int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                            if (riqijiequ.Length > 2 && riqijiequ.Substring(2, 1) == "/" && item.wenben.Contains(".") && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length <= 4 && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length >= 3)
                            {
                                time1 = "20" + riqijiequ + "/01";
                                time1 = time1.Replace("//", "/");
                                time1 = aresultNewMethod(time1);
                            }
                            else
                                time1 = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/" + "01";

                            return time1;

                        }

                    }
                    else if (index20 > 0 && indexnian < 0 && indexyue < 0 && indexri < 0 && indexhao < 0)
                    {
                        // 环球SM POS 2014.08 CB3X
                        indexend = 0;
                        //
                        cname = item.wenben.ToCharArray();
                        //检查字母和汉字所在的位置
                        indexend = Checkchinese_shuzi(index20, indexend, cname);

                        riqijiequ = item.wenben.Substring(index20, indexend - index20).Replace(".", "/");
                        if (riqijiequ.Contains("/") && riqijiequ.Length == 8)
                            riqijiequ = riqijiequ.Trim() + "/01";

                        time1 = riqijiequ.Replace("//", "/");
                        time1 = aresultNewMethod(time1);
                    }
                }
                //NC苏州欧尚超市2017年07份租金费用53200
                else if (time == "" && item.wenben != null && !item.wenben.Contains("/") && newresult != "" && yue_i < 0 && nian_i > 0 & hao_i < 0 && ri_i < 0 && newresult.Length > 4 && newresult.Substring(0, 2).Contains("20"))
                {
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > nian_i)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        riqijiequ = item.wenben.Substring(index20, indexend - index20).Replace(".", "/");


                        //2016年长春活力城物业费wa2016.05
                        bool isrun = false;

                        int erling_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "20").Count;
                        if (erling_count == 2 && newresult.Length > 8 && riqijiequ.Length == 5)
                        {

                            indexend = 0;
                            //
                            cname = item.wenben.ToCharArray();

                            //检查字母和汉字所在的位置
                            indexend = Checkchinese_shuzi(nian_i, indexend, cname);
                            if (indexend - nian_i == 1)
                            {
                                index20 = item.wenben.LastIndexOf("20");
                                indexend = 0;
                                for (int ic = 0; ic < cname.Length; ic++)
                                {
                                    string jdd = cname[ic].ToString();

                                    bool ischina = newHasChineseTest(jdd);
                                    if (ischina == true && ic > index20)
                                    {
                                        indexend = ic;
                                        break;
                                    }
                                }
                                if (indexend == 0)
                                {

                                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(item.wenben.Substring(index20, item.wenben.Length - index20), @"[^0-9]+", "/");
                                    int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                                    if (riqijiequ.Length >= 6 && riqijiequ.Length <= 7 && riqijiequ.Substring(0, 2) == "20" && pie_count == 1)
                                    {
                                        riqijiequ = riqijiequ + "/01";
                                        isrun = true;

                                    }
                                }


                            }
                        }
                        //

                        if (indexend - nian_i < 4 && isrun == false)
                            riqijiequ = riqijiequ + "月01日";

                        time1 = clsCommHelp.objToDateTime1(riqijiequ);
                    }

                }
                //*F2549AR17000052*杭州西城NC2017.03水电费
                else if (time == "" && item.wenben != null && !item.wenben.Contains("/") && newresult != "" && newresult.Length > 4 && !newresult.Substring(0, 2).Contains("20") && yearcount < 2 && MMcount < 2 && indexyue < 0)
                {
                    int indexend = 0;
                    //杭 位置
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index20)// 
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    //截取后段日期
                    if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0)
                        riqijiequ = item.wenben.Substring(index20, indexend - index20);
                    else if (indexnian > 0 && indexyue > 0 && indexhao < 0 && indexri < 0)
                    {
                        indexend = 0;
                        //
                        cname = item.wenben.ToCharArray();
                        for (int ic = 0; ic < cname.Length; ic++)
                        {
                            string jdd = cname[ic].ToString();

                            bool ischina = newHasChineseTest(jdd);
                            if (ischina == true && ic > nian_i)
                            {
                                indexend = ic;
                                break;
                            }
                        }
                        riqijiequ = item.wenben.Substring(index20, indexend - index20).Replace(".", "/");
                        if (indexend - nian_i < 4)
                            riqijiequ = riqijiequ + "月01日";
                        riqijiequ = clsCommHelp.objToDateTime1(riqijiequ);
                        return riqijiequ;

                    }
                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                    if (pie_count == 1 && riqijiequ.Length == 7)
                        time1 = riqijiequ + "/01";
                    //F2549AR17000077NC昆山九方2017年6月POS机使用费
                    else if (pie_count == 2 && riqijiequ.Length >= 7 && indexnian > 0 && indexyue > 0)
                    {
                        riqijiequ = item.wenben.Substring(index20, indexend - index20);
                        string nian = riqijiequ;
                        riqijiequ = item.wenben.Substring(indexnian, indexyue - indexnian);
                        string yue = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                        riqijiequ = item.wenben.Substring(indexyue, 2);
                        string ri = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                        if (ri == "/")
                            ri = "01";
                        time1 = nian + "/" + yue + "/" + ri;
                        time1 = time1.Replace("//", "/");
                    }

                }
                //*F2549AR17000115*宁波环球银泰城WA6月POS管理费  ///F255 BC 大悦城17.07月提成租金

                else if (time == "" && item.wenben != null && !item.wenben.Contains("/") && newresult != "" && newresult.Length > 4 && !newresult.Substring(0, 2).Contains("20") && yearcount < 2 && MMcount < 2 && indexyue > 0 && indexyue - item.wenben.IndexOf(newresult.Substring(0, 1)) > 7 && yue_valoume == 1)
                {
                    int indexend = 0;
                    //杭 位置
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > indexyue)// 
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    string s = item.wenben;
                    char[] ca = s.ToCharArray();
                    Array.Reverse(ca);
                    s = new string(ca);
                    indexend = 0;

                    int indexdaoyue = s.IndexOf("月");
                    cname = s.ToCharArray();
                    //检查字母和汉字所在的位置
                    indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);
                    indexend = item.wenben.Length - indexend;

                    riqijiequ = item.wenben.Substring(indexend, indexyue - indexend).Replace(".", "/");
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;

                    if (riqijiequ.Length == 1)
                    {
                        time1 = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/01";
                        if (pie_count == 2 && item.wenben.Contains("和"))
                        {
                            yue_i = item.wenben.LastIndexOf("月");
                            riqijiequ = item.wenben.Substring(yue_i - 2, 2).Replace(".", "/");
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "");
                            if (riqijiequ.Length >= 2 && riqijiequ.Substring(0, 2) != "20")
                                time1 = "20" + riqijiequ + "/01";
                            else if (riqijiequ.Length == 1)
                                time1 = time1 + "_" + DateTime.Now.Year.ToString() + "/" + riqijiequ + "/01";
                        }
                    }
                    else if (riqijiequ.Length >= 1 && pie_count == 1 && riqijiequ.Substring(0, 1) != "/" && riqijiequ.Substring(0, 2) == "20")
                    {
                        riqijiequ = riqijiequ + "/01";
                        time1 = aresultNewMethod(riqijiequ);

                    }
                    else if (riqijiequ.Length >= 1 && pie_count == 1 && riqijiequ.Substring(0, 1) != "/" && riqijiequ.Substring(0, 2) != "20")
                    {
                        if (riqijiequ.Substring(0, 2) != "20" && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length <= 5 && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length >= 3)
                            time1 = "20" + riqijiequ + "/01";
                        time1 = aresultNewMethod(time1);
                    }

                    return time1;

                }
                //16.10月物品租赁发票

                else if (time == "" && item.wenben != null && item.wenben.Contains(".") && newresult != "" && !item.wenben.Contains("年") && yue_i > 0 && newresult.Length >= 3)
                {
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count;
                    string result = "";
                    string result2 = "";
                    string[] temp2 = tiqushijian(item, item, ref result, ref result2);
                    index2 = item.wenben.IndexOf(result.Substring(0, 1));
                    if (pie_count == 2 && index2 > yue_i)
                    {
                        yue_i = item.wenben.LastIndexOf("月");

                    }
                    string s = item.wenben;
                    char[] ca = s.ToCharArray();
                    Array.Reverse(ca);
                    s = new string(ca);
                    int indexend = 0;

                    int indexdaoyue = s.IndexOf("月");
                    var cname = s.ToCharArray();
                    //检查字母和汉字所在的位置
                    indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);
                    if (indexend == 0)
                        indexend = item.wenben.Length;

                    int zhengshujiequweizhi = item.wenben.Length - indexend;

                    int blanki = System.Text.RegularExpressions.Regex.Matches(item.wenben.Substring(0, indexend), " ").Count;
                    if (yue_i - zhengshujiequweizhi + blanki > 0)
                        riqijiequ = item.wenben.Substring(item.wenben.Length - indexend, yue_i - zhengshujiequweizhi + blanki).Replace(".", "/");
                    else
                    {

                    }
                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                    if (riqijiequ.Length >= 2 && riqijiequ.Substring(0, 2) != "20")
                    {
                        if (riqijiequ.Substring(0, 1) == "/")
                            riqijiequ = riqijiequ.Substring(1, riqijiequ.Length - 1);
                        if (riqijiequ.Substring(0, 2) != "20" && System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count < 3)
                        {
                            time1 = "20" + riqijiequ + "/01";
                            time1 = aresultNewMethod(time1);
                            //EE 武汉凯德1818 62017.7月物业费核销
                            pie_count = System.Text.RegularExpressions.Regex.Matches(time1, "/").Count;
                            int erling_count = System.Text.RegularExpressions.Regex.Matches(time1, "20").Count;
                            if (time1.Length > 10 && blanki > 0 && index20 > 0 && pie_count == 3 && erling_count == 2)
                            {
                                int indexnew20 = time1.LastIndexOf("20");
                                riqijiequ = time1.Substring(indexnew20, time1.Length - indexnew20);
                                riqijiequ = aresultNewMethod(riqijiequ);
                            }
                        }
                        //大悦城PR2017.2月3月4月物业费
                        else if (riqijiequ.Substring(0, 2) == "20" && System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count >= 3)
                        {

                            string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "月");
                            string date1 = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                            if (date1 != "" && date1.Substring(0, 1) == "/" && System.Text.RegularExpressions.Regex.Matches(date1, "/").Count == 2)
                                date1 = date1 + "/01";
                            string otherdateAdd = "";
                            date1 = aresultNewMethod(date1);
                            for (int d = 1; d < temp3.Length - 1; d++)
                            {
                                if (System.Text.RegularExpressions.Regex.Replace(temp3[d], @"[^0-9]+", "") == "" || System.Text.RegularExpressions.Regex.Replace(temp3[d], @"[^0-9]+", "").Length > 2)
                                    continue;

                                otherdateAdd = otherdateAdd + "_" + Convert.ToDateTime(date1).Year.ToString() + "/" + System.Text.RegularExpressions.Regex.Replace(temp3[d], @"[^0-9]+", "") + "/28";
                            }
                            time1 = date1 + otherdateAdd;


                        }
                    }
                    else if (riqijiequ.Length == 1)
                    {
                        time1 = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/01";
                        if (pie_count == 2 && item.wenben.Contains("和"))
                        {
                            yue_i = item.wenben.LastIndexOf("月");
                            riqijiequ = item.wenben.Substring(yue_i - 2, 2).Replace(".", "/");
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "");
                            if (riqijiequ.Length >= 2 && riqijiequ.Substring(0, 2) != "20")
                                time1 = "20" + riqijiequ + "/01";
                            else if (riqijiequ.Length == 1)
                                time1 = time1 + "_" + DateTime.Now.Year.ToString() + "/" + riqijiequ + "/01";
                        }
                    }

                }
                //TT天津国贸16年12月POS //计提SF 天津大悦城购物中心17年08月物业费

                else if (time == "" && item.wenben != null && newresult != "" && item.wenben.Contains("年") && yue_i > 0 && yearcount == 1 && MMcount == 1)
                {
                    string nian = "";
                    string yue = "";
                    string ri = "";
                    string timeend = "";


                    string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "月");
                    int indexniani = temp3[0].IndexOf("年");
                    if (indexniani > index2)
                    {
                        string s = tempsplit_All[0];
                        char[] ca = s.ToCharArray();
                        Array.Reverse(ca);
                        s = new string(ca);
                        int indexend = 0;
                        //
                        int indexdaoyue = s.IndexOf("年");
                        var cname = s.ToCharArray();
                        //检查字母和汉字所在的位置
                        indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);

                        if (indexend == 0)
                            indexend = tempsplit_All[0].Length;
                        int zhengshujiequweizhi = tempsplit_All[0].Length - indexend;

                        int blanki = System.Text.RegularExpressions.Regex.Matches(item.wenben.Substring(0, indexend), " ").Count;
                        int blankindex = item.wenben.IndexOf(" ");
                        if (blanki != 0)
                        {
                            indexdaoyue = blanki + indexdaoyue;
                            if (blankindex < indexdaoyue)
                                zhengshujiequweizhi = zhengshujiequweizhi - blanki;
                            else
                                zhengshujiequweizhi = zhengshujiequweizhi + blanki;

                        }
                        string n = "";
                        //F2549AR17000077NC昆山九方2017年6月POS机使用费
                        if (index2 != zhengshujiequweizhi)
                        {
                            n = tempsplit_All[0].Substring(zhengshujiequweizhi, indexniani - zhengshujiequweizhi);
                        }
                        else n = tempsplit_All[0].Substring(index2, indexniani - index2);
                        nian = System.Text.RegularExpressions.Regex.Replace(n, @"[^0-9]+", "");

                    }
                    if (nian.Length == 1)
                    {
                        time1 = "日期无法识别";
                        return time1;
                    }
                    else if (nian.Length == 2)
                        nian = "20" + nian;
                    else if (nian.Length == 3)
                        nian = "201" + nian;
                    else if (nian.Length > 4)
                    {
                        time1 = "日期无法识别";
                        //  return time1;
                    }
                    if (nian != null && nian.Length > 2 && nian.Substring(0, 2) != "20" && index20 >= 0)
                    {
                        string s = item.wenben;
                        char[] ca = s.ToCharArray();
                        Array.Reverse(ca);
                        s = new string(ca);
                        int indexend = 0;
                        //
                        int indexdaoyue = s.IndexOf("年");
                        var cname = s.ToCharArray();
                        //检查字母和汉字所在的位置
                        indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);

                        int blanki = System.Text.RegularExpressions.Regex.Matches(item.wenben.Substring(0, indexend), " ").Count;
                        nian = item.wenben.Substring(item.wenben.Length - indexend, nian_i - indexend + blanki).Replace(".", "/");

                    }
                    //月
                    //读取月
                    if (indexniani > 0)
                    {
                        string y = temp3[0].Substring(indexniani, temp3[0].Length - indexniani);
                        yue = System.Text.RegularExpressions.Regex.Replace(y, @"[^0-9]+", "");
                        if (yue.Length == 1)
                            yue = "0" + yue;
                    }
                    time1 = "";
                    if (nian != "")
                        time1 += nian + "/";
                    else
                    {
                        //[清帐]SA大连柏威年店4月租金抽成清帐
                        nian = "";
                        yue = "";
                        ri = "";
                        timeend = "";
                        item.wenben = item.wenben.Replace("年", "+");

                        indexniani = temp3[0].IndexOf("年");
                        int indexyuei = temp3[0].IndexOf("月");
                        int indexhaoi = temp3[0].IndexOf("号");
                        int indexhaori = temp3[0].IndexOf("日");

                        if (indexyuei > 0 && indexniani < 0)
                        {
                            //    time1 = year_split(time, timeend);
                            time1 = NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        }
                        time1 = NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        item.wenben = item.wenben.Replace("+", "年");
                        if (time1.Length == 6)
                            time1 = time1 + "/01";
                        return time1;

                    }
                    if (yue != "" && yue != "/")
                        time1 += yue + "/";
                    if (ri == "")
                        time1 += "01";

                }
                //计提SA 大连柏威年 17年06月物业费
                else if (time == "" && item.wenben != null && newresult != "" && item.wenben.Contains("年") && yue_i > 0 && yearcount == 2 && MMcount == 1 && ri_i < 0 && hao_i < 0)
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index2)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");
                        if (riqijiequ.Length < 8)
                        {
                            riqijiequ = riqijiequ + "/" + item.wenben.Substring(indexend + 1, yue_i - indexend).Replace("月", "/");

                            time1 = riqijiequ.Replace("//", "/");
                            if (riqijiequ.Substring(0, 2) != "20")
                                time1 = "20" + riqijiequ;
                            int pie_count = System.Text.RegularExpressions.Regex.Matches(time1, "/").Count;
                            if (time1.Length >= 7 && time1.Length <= 8 && pie_count == 2)
                                time1 = time1 + "01";
                        }
                    }
                }
                else if (time == "" && item.wenben != null && !item.wenben.Contains("-") && newresult != "" && newresult.Length <= 7 && newresult.Length >= 6 && yearcount == 2 || MMcount == 2)
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index2)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        //截取日期
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");
                        if (riqijiequ.Length > 8)
                        {
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "");
                            riqijiequ = riqijiequ + "" + "01";
                            if (riqijiequ != null && riqijiequ.Length == 8 && riqijiequ.Substring(0, 2) == "20")
                                riqijiequ = DateTime.Parse(riqijiequ.ToString().Substring(0, 4) + "-" +
                                                          riqijiequ.ToString().Substring(4, 2) + "-" +
                                                          riqijiequ.ToString().Substring(6, 2)).ToString("yyyy/MM/dd");

                            time1 = riqijiequ.Replace("//", "/");

                        }
                        else if (yearcount == 2)
                        {

                            //[清帐]SA大连柏威年店6月物业费清帐 156440元
                            if (riqijiequ.Length == 1)
                                riqijiequ = "0" + riqijiequ;

                            #region //EK万柳17年6月EK万柳17年6月物业
                            bool isrun = false;

                            int yue_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "月").Count;
                            if (yue_count == 2)
                            {
                                string s = item.wenben;
                                char[] ca = s.ToCharArray();
                                Array.Reverse(ca);
                                s = new string(ca);
                                indexend = 0;

                                int indexdaoyue = s.IndexOf("月");
                                int indexdaonian = s.IndexOf("年");
                                cname = s.ToCharArray();
                                //检查字母和汉字所在的位置
                                indexend = Checkchinese_shuzi(indexdaoyue, indexend, cname);
                                if (indexend == 0)
                                    indexend = item.wenben.Length;
                                //后边的日期提取
                                int zhengshujiequweizhi = item.wenben.Length - indexend;
                                indexend = Checkchinese_shuzi(indexdaonian, indexend, cname);
                                int houbian_nianweizhi = item.wenben.Length - indexend;
                                string hou_riqi = item.wenben.Substring(houbian_nianweizhi, zhengshujiequweizhi - houbian_nianweizhi + 1);
                                //正数的日期提取
                                indexyue = item.wenben.IndexOf("月");
                                //
                                indexyue = item.wenben.Length - indexyue;
                                indexend = Checkchinese_shuzi(indexyue, indexend, cname);
                                zhengshujiequweizhi = item.wenben.Length - indexend;
                                indexdaonian = item.wenben.IndexOf("年");
                                //
                                indexdaonian = item.wenben.Length - indexdaonian;
                                indexend = Checkchinese_shuzi(indexdaonian, indexend, cname);
                                houbian_nianweizhi = item.wenben.Length - indexend;


                                string qian_riqi = item.wenben.Substring(houbian_nianweizhi, zhengshujiequweizhi - houbian_nianweizhi + 1);

                                if (qian_riqi != "" && qian_riqi == hou_riqi)
                                    if (qian_riqi.Length == 4 && qian_riqi.Contains("年") && !qian_riqi.Contains("月") && qian_riqi.Substring(0, 2) != "20")
                                    {
                                        time1 = "20" + qian_riqi.Replace("年", "/").Replace("月", "/") + "/01";
                                        isrun = true;

                                    }
                            }
                            /// 
                            #endregion
                            if (isrun == false)
                                time1 = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/" + "01";
                            return time1;

                        }
                        else if (MMcount == 2 && indexnian < 0 && index20 < 0)
                        {
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(item.wenben.Substring(indexyue - 2, 2), @"[^0-9]+", "");
                            int indexyue2 = item.wenben.LastIndexOf("月");
                            riqijiequ = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/" + "01";
                            time1 = riqijiequ + "_" + DateTime.Now.Year.ToString() + "/" + System.Text.RegularExpressions.Regex.Replace(item.wenben.Substring(indexyue2 - 2, 2), @"[^0-9]+", "") + "/" + "01";
                        }

                    }
                }
                //CO 上海合生汇3管理费6360
                else if (time1 == "" && item.wenben != null && !item.wenben.Contains("-") && newresult != "" && newresult.Length <= 7 && newresult.Length >= 1 && yearcount < 2 && MMcount < 2)
                {
                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                    int indexend = 0;
                    //
                    var cname = item.wenben.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > index2)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend != 0)
                    {
                        //截取日期
                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");

                        if (riqijiequ.Length > 8)
                        {
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "");
                            riqijiequ = riqijiequ + "" + "01";
                            if (riqijiequ != null && riqijiequ.Length == 8 && riqijiequ.Substring(0, 2) == "20")
                                riqijiequ = DateTime.Parse(riqijiequ.ToString().Substring(0, 4) + "-" +
                                                          riqijiequ.ToString().Substring(4, 2) + "-" +
                                                          riqijiequ.ToString().Substring(6, 2)).ToString("yyyy/MM/dd");

                            time1 = riqijiequ.Replace("//", "/");

                        }
                        else
                        {

                            //[清帐]SA大连柏威年店6月物业费清帐 156440元
                            if (riqijiequ.Length == 1)
                                riqijiequ = "0" + riqijiequ;
                            //泸州万象汇BC 17.7pos租金
                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                            int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                            if (riqijiequ.Length > 2 && riqijiequ.Substring(2, 1) == "/" && item.wenben.Contains(".") && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length <= 4 && System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "").Length >= 3)
                            {
                                time1 = "20" + riqijiequ + "/01";
                                time1 = time1.Replace("//", "/");
                                time1 = aresultNewMethod(time1);
                            }
                            else
                                if (Convert.ToDouble(System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "")) <= 12)
                                    time1 = DateTime.Now.Year.ToString() + "/" + riqijiequ + "/" + "01";
                                ////NC 天津嘉里汇 17年2提成租金
                                else if (yearcount == 1)
                                {
                                    string nianyue = System.Text.RegularExpressions.Regex.Replace(item.wenben, @"[^0-9]+", "/").Replace(".", "/");
                                    pie_count = System.Text.RegularExpressions.Regex.Matches(nianyue, "/").Count;
                                    int pieindex = nianyue.IndexOf("/");
                                    if (pie_count == 3 && nianyue.Substring(0, 1) == "/")
                                    {
                                        nianyue = nianyue.Substring(1, nianyue.Length - 1);
                                        time1 = "20" + nianyue + "/01";
                                        time1 = aresultNewMethod(time1);
                                        return time1;
                                    }
                                    //16年5-8月抽成租金津嘉里汇NC

                                    else if (pie_count == 1 && nianyue.Substring(0, 1) != "/" && indexnian > 0 && pieindex < nianyue.Length - 1)
                                    {
                                        //  nianyue = nianyue.Substring(0, nianyue.Length - 1);
                                        time1 = "20" + nianyue + "/01";
                                    }

                                }
                                else if (yearcount == 0 && pie_count == 1)//WA莲花17/10月物业费
                                {
                                    index2 = item.wenben.IndexOf(newresult.Substring(0, 1));
                                    indexend = 0;
                                    //
                                    cname = item.wenben.ToCharArray();
                                    for (int ic = 0; ic < cname.Length; ic++)
                                    {
                                        string jdd = cname[ic].ToString();

                                        bool ischina = newHasChineseTest(jdd);
                                        if (ischina == true && ic > index2)
                                        {
                                            indexend = ic;
                                            break;
                                        }
                                    }
                                    if (indexend != 0)
                                    {
                                        riqijiequ = item.wenben.Substring(index2, indexend - index2).Replace(".", "/");
                                        if (riqijiequ.Length < 8)
                                        {
                                            riqijiequ = riqijiequ + "/" + "01";
                                            time1 = riqijiequ.Replace("//", "/");
                                            if (riqijiequ.Substring(0, 2) != "20")
                                                time1 = "20" + riqijiequ;
                                        }
                                        else if (riqijiequ.Length == 8 && Regex.Matches(riqijiequ.ToString(), "[a-zA-Z]").Count > 0)
                                        {
                                            riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                                            pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;

                                            if (pie_count == 2)
                                                riqijiequ = riqijiequ + "/" + "01";
                                            else if (pie_count == 1 && riqijiequ.Substring(0, 2) == "20")
                                                riqijiequ = riqijiequ.Substring(0, 4) + "/" + riqijiequ.Substring(4, riqijiequ.IndexOf("/") - 4) + "/" + "01";
                                            time1 = riqijiequ.Replace("//", "/");
                                            if (riqijiequ.Substring(0, 2) != "20")
                                                time1 = "20" + riqijiequ;

                                            time1 = aresultNewMethod(time1);
                                        }
                                    }

                                }
                            return time1;

                        }

                    }
                }
                return time1;

            }
            catch
            {
                return "";



            }
        }

        private static string year_split(string time, string timeend)
        {
            int en = Convert.ToDateTime(time).Year * 12 - Convert.ToDateTime(time).Month;
            int st = Convert.ToDateTime(timeend).Year * 12 + Convert.ToDateTime(timeend).Month;


            int totalMonth = Convert.ToDateTime(timeend).Year * 12 + Convert.ToDateTime(timeend).Month - Convert.ToDateTime(time).Year * 12 - Convert.ToDateTime(time).Month;

            string timere = "";
            for (int im = 0; im <= totalMonth; im++)
            {
                //temp3[0] = "2017/01/01";
                DateTime t = Convert.ToDateTime(time).AddMonths(im);
                if (totalMonth > 0 && totalMonth != 1)
                    timere += t.ToString("MM/dd/yyyy") + "_";
                else if (totalMonth > 0 && totalMonth == 1)
                {
                    if (Convert.ToDateTime(time).Day == Convert.ToDateTime(timeend).Day && Convert.ToDateTime(time).Day > 15)
                    {
                        t = Convert.ToDateTime(time).AddMonths(1);
                        timere += t.ToString("MM/dd/yyyy");
                        return timere;

                    }
                    else if (Convert.ToDateTime(time).Day != Convert.ToDateTime(timeend).Day && Convert.ToDateTime(time).Day < 15 && Convert.ToDateTime(timeend).Day < 15)
                    {
                        t = Convert.ToDateTime(time).AddMonths(0);
                        timere += t.ToString("MM/dd/yyyy");
                        return timere;

                    }
                    else if (Convert.ToDateTime(time).Day != Convert.ToDateTime(timeend).Day && Convert.ToDateTime(time).Day < 15 && Convert.ToDateTime(timeend).Day > 15)
                    {
                        t = Convert.ToDateTime(time).AddMonths(im);
                        timere += t.ToString("MM/dd/yyyy") + "_";
                        //return timere;

                    }
                    else if (Convert.ToDateTime(time).Day != Convert.ToDateTime(timeend).Day)
                    {
                        t = Convert.ToDateTime(time).AddMonths(1);
                        timere += t.ToString("MM/dd/yyyy");
                        return timere;

                    }
                    else if (Convert.ToDateTime(time).Day == Convert.ToDateTime(timeend).Day && Convert.ToDateTime(time).Day < 7)
                    {
                        t = Convert.ToDateTime(time).AddMonths(0);
                        timere += t.ToString("MM/dd/yyyy");
                        return timere;

                    }
                }
                else if (totalMonth == 0)
                    timere += t.ToString("MM/dd/yyyy");

            }
            return timere;

        }

        public static string[] tiqushijian(clsR2accrualsapinfo item, clsR2accrualsapinfo temp, ref string result, ref string result2)
        {
            string[] temp1 = null;

            result = System.Text.RegularExpressions.Regex.Replace(item.wenben, @"[^0-9]+", "");
            if (item.wenben != null && item.wenben.Contains("-") && result.Length >= 6)
            {
                string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "-");
                string aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
                string bresult = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
                string atime1 = "";
                string btime1 = "";
                btime1 = Catchwenbenshijian(item, aresult, btime1);
                temp1 = System.Text.RegularExpressions.Regex.Split(btime1, "_");

            }
            if (temp != null)
                result2 = System.Text.RegularExpressions.Regex.Replace(temp.wenben, @"[^0-9]+", "");
            return temp1;
        }
        private static void Split_jiahao(clsR2accrualsapinfo item, ref string time1, int indexyue, int indexnian, ref string riqijiequ, string time, string newresult, int indexhao, int indexri, int index20)
        {
            string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "-");
            string aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
            string bresult = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
            if (aresult.Length == 6 && aresult.Substring(0, 2) != "20")
            {
                aresult = "20" + aresult;
                aresult = aresult.Substring(0, 4) + "/" + aresult.Substring(4, 2) + "/" + aresult.Substring(6, 2);

            }
            //镇江万达NC2017.5.1-7.31房屋租金发票登记
            else if (aresult.Length == 6 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                aresult = aresult.Substring(0, 4) + yueri;

            }
            //杭州万象城PC 2017.2.21-3.20电费
            else if (aresult.Length == 7 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                yueri = System.Text.RegularExpressions.Regex.Replace(yueri, @"[^0-9]+", "/");

                aresult = aresult.Substring(0, 4) + yueri;
                aresult = aresultNewMethod(aresult);
            }
            //补提PD 上海IFC国金中心（新路二）2017.03.24-3.31推广
            else if (aresult.Length == 8 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                if (yueri.Substring(0, 1) != "/")
                    yueri = "/" + yueri;
                aresult = aresult.Substring(0, 4) + yueri;
                aresult = aresultNewMethod(aresult);
            }
            //WA仲盛17/1/6-2/6电费。
            else if (aresult.Length <= 4 && aresult.Length >= 3 && aresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                }
                else if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        tx = "20" + tx;
                        if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                            aresult = tx + "/01";
                    }
                }
            }
            //WA昆城广场17/2/16-17/3/10电费。
            else if (aresult.Length == 5 && aresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                }
            }
            //绍兴银泰城NC 2017.4-6租金部分
            else if (aresult.Length == 5 && aresult.Substring(0, 2) == "20")
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], ".").Count;
                if (temp3[0].Contains("."))
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Contains("/") && tx.Length == 6 && pie_count == 1)
                            aresult = tx + "/01";

                    }
                }
            }
            //F2549AR17000099-PD协信2016.01-4物业管理费
            else if (time == "" && item.wenben != null && !item.wenben.Contains("/") && newresult != "" && newresult.Length > 4 && !newresult.Substring(0, 2).Contains("20"))
            {

                int indexend = 0;
                //杭 位置
                var cname = item.wenben.ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true && ic > index20)// 
                    {
                        indexend = ic;
                        break;
                    }
                }
                //
                //截取后段日期
                if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0)
                {
                    riqijiequ = item.wenben.Substring(index20, indexend - index20);
                    string[] newtemp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                    string[] tempdian = System.Text.RegularExpressions.Regex.Split(newtemp3[0], riqijiequ.Substring(0, 4));
                    string yueri = tempdian[1].Replace(".", "/");

                    if (yueri.Substring(0, 1) != "/")
                        yueri = "/" + yueri;
                    aresult = riqijiequ.Substring(0, 4) + yueri;
                    aresult = aresultNewMethod(aresult);
                    aresult = System.Text.RegularExpressions.Regex.Replace(aresult, @"[^0-9]+", "/");
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(aresult, "/").Count;
                    if (pie_count == 1 && aresult.Length == 7)
                        time1 = aresult + "/01";

                    //
                    indexend = 0;
                    //杭 位置
                    cname = newtemp3[1].ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true)// 
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    if (indexend == 0)
                        bresult = System.Text.RegularExpressions.Regex.Replace(newtemp3[1], @"[^0-9]+", "/");
                    else
                    {
                        bresult = newtemp3[1].Substring(0, indexend);
                    }
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult;
                    pie_count = System.Text.RegularExpressions.Regex.Matches(bresult, "/").Count;
                    if (pie_count == 1 && bresult.Length <= 7)
                        bresult = bresult + "/01";
                    bresult = bresultNewMethod(bresult);
                    if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                    {
                        aresult = bresultNewMethod(aresult);
                        if (aresult != null && aresult.Length == 8)
                            aresult = aresult + "01";


                        bresult = aresultNewMethod(bresult);
                        if (bresult != null && bresult.Length == 8)
                            bresult = bresult + "01";
                        time1 = aresult + "_" + bresult;



                        return;
                    }
                }
            }
            //朝北 PR 5.4-17日特卖租金
            else if (aresult.Length == 2 && aresult.Substring(0, 2) != "20" && temp3[0].Contains("."))
            {

                string nian = "";
                string yue = "";
                string ri = "";
                string timeend = "";

                int indexniani = temp3[0].IndexOf("年");
                int indexyuei = temp3[0].IndexOf("月");
                int indexhaoi = temp3[0].IndexOf("号");
                int indexhaori = temp3[0].IndexOf("日");

                if (indexyuei > 0 && indexniani < 0)
                {
                    //    time1 = year_split(time, timeend);
                    time1 = NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                }

                time1 = NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                aresult = time;

                //17日特卖租金
                indexniani = temp3[1].IndexOf("年");
                indexyuei = temp3[1].IndexOf("月");
                indexhaoi = temp3[1].IndexOf("号");
                indexhaori = temp3[1].IndexOf("日");
                if (indexyuei < 0 && bresult.Length == 2)
                    if (indexhaori > 0 || indexhaoi > 0)
                    {
                        aresult = aresultNewMethod(aresult);
                        bresult = aresult;
                        bresult = bresultNewMethod(bresult);
                    }
                if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                {
                    // time1 = for_yue(aresult, bresult);
                    aresult = bresultNewMethod(aresult);
                    if (aresult != null && aresult.Length == 8)
                        aresult = aresult + "01";


                    bresult = aresultNewMethod(bresult);
                    if (bresult != null && bresult.Length == 8)
                        bresult = bresult + "01";
                    time1 = aresult + "_" + bresult;
                    return;
                }

            }

            ////////////////////////////////

            if (bresult.Length == 4)
            {

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult.Substring(0, 2) + "/" + bresult.Substring(2, 2);

            }
            else if (bresult.Length == 1)
            {

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult.Substring(0, 1) + "/30";

            }

             //镇江万达NC2017.5.1-7.31房屋租金发票登记
            else if (bresult.Length == 3 && item.wenben.Contains(".") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length - 1);
                bresult = bresult.Replace("//", "/");
            }
            //   PO 上海莘庄仲盛2016.12.6-2017.2.6号电费
            else if (bresult.Length == 6 && item.wenben.Contains(".") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) == "20")
            {
                //判断2017.2.6 后的第一个汉字位数
                int indexend = 0;

                var cname = temp3[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend == 0)
                {
                    string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[1], bresult.Substring(0, 4));
                    string yueri01 = tempdian[1].Replace(".", "/");
                    //  aresult = "20" + aresult;
                    bresult = bresult.Substring(0, 4) + yueri01;
                }
                else

                    bresult = temp3[1].Substring(0, indexend);

                string yueri = bresult.Replace(".", "/");
                //  aresult = "20" + aresult;
                bresult = bresultNewMethod(bresult);

            }
            //WA仲盛17/1/6-2/6电费。
            else if (bresult.Length == 2 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);

                }
            }
            else if (bresult.Length == 5 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        bresult = "20" + tx;
                    }
                    bresult = Convert.ToDateTime(bresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);

                }
                else if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        bresult = "20" + tx;
                    }
                    else if (tx.Length == 8)
                    {

                        tx = tx.Substring(0, tx.Length);
                        bresult = "20" + tx;
                        if (bresult.Length == 10 && bresult.Substring(9, 1) == "/")
                            bresult = bresult.Substring(0, 9);
                    }
                }
            }
            //杭州星光NC2017.6.15-2017.9.14物管费
            else if (bresult.Length >= 7 && bresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[1], bresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;

                //EE哈尔滨江北万达首期租金(2017.06.30-2017.09.30)102194.52
                int indexkuohao = yueri.IndexOf(")");
                if (indexkuohao > 0)
                    yueri = yueri.Substring(0, indexkuohao);

                bresult = bresult.Substring(0, 4) + yueri;
                bresult = System.Text.RegularExpressions.Regex.Replace(bresult, @"[^0-9]+", "/");
                bresult = bresultNewMethod(bresult);
            }
            //NC苏州绿宝2017年6.16-7.15电费1765.56
            else if (bresult.Length >= 7 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {
                int indexend = 0;
                //
                var cname = temp3[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)//&& ic > nian_i
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend > 0)
                {
                    bresult = temp3[1].Substring(0, indexend);
                    string yueri = bresult.Replace(".", "/");
                    if (yueri.Substring(0, 1) != "/")
                        yueri = "/" + yueri;

                    bresult = Convert.ToDateTime(bresult).Year.ToString() + yueri;

                    bresult = bresultNewMethod(bresult);
                }
            }
            //莘庄pd 2017.5.6-6.6电费
            else if (bresult.Length == 2 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {
                //int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], ".").Count;
                //if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);
                    if (bresult != null && bresult.Length == 8)
                        bresult = bresult + "01";
                }
            }
            else if (bresult.Length == 3 && item.wenben.Contains("/") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length - 1);
                bresult = bresult.Replace("//", "/");


            }
            else if (bresult.Length == 6 && !item.wenben.Contains("/") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                if (yueri.Length == 9)
                    bresult = "20" + yueri.Substring(0, yueri.Length - 1);
                else
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length - 1);
                bresult = bresult.Replace("//", "/");


            }

            if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
            {
                aresult = bresultNewMethod(aresult);
                if (aresult != null && aresult.Length == 8)
                    aresult = aresult + "/01";
                if (aresult != null && aresult.Length == 7)
                    aresult = aresult + "/01";

                bresult = aresultNewMethod(bresult);
                if (bresult != null && bresult.Length == 8)
                    bresult = bresult + "/01";
                if (bresult != null && bresult.Length == 7)
                    bresult = bresult + "/01";
                time1 = aresult + "_" + bresult;
            }
        }


        private static void Split_henggang(clsR2accrualsapinfo item, ref string time1, int indexyue, int indexnian, ref string riqijiequ, string time, string newresult, int indexhao, int indexri, int index20)
        {
            if (item.wenben != null && item.wenben.Contains("--"))
                item.wenben = item.wenben.Replace("--", "-");
            bool newadd = false;
            string[] temp3 = System.Text.RegularExpressions.Regex.Split(item.wenben, "-");
            string aresult = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "");
            string bresult = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "");
            int pie_count_temp30 = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
            int erling_count_temp30 = System.Text.RegularExpressions.Regex.Matches(temp3[0], "20").Count;
            if (aresult.Length == 6 && aresult.Substring(0, 2) != "20")
            {
                aresult = "20" + aresult;
                aresult = aresult.Substring(0, 4) + "/" + aresult.Substring(4, 2) + "/" + aresult.Substring(6, 2);

            }
            //镇江万达NC2017.5.1-7.31房屋租金发票登记
            else if (aresult.Length == 6 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                yueri = System.Text.RegularExpressions.Regex.Replace(yueri, @"[^0-9]+", "/");
                aresult = aresult.Substring(0, 4) + yueri;

            }
            //杭州万象城PC 2017.2.21-3.20电费
            else if (aresult.Length == 7 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                yueri = System.Text.RegularExpressions.Regex.Replace(yueri, @"[^0-9]+", "/");

                aresult = aresult.Substring(0, 4) + yueri;
                aresult = aresultNewMethod(aresult);
            }
            //补提PD 上海IFC国金中心（新路二）2017.03.24-3.31推广
            else if (aresult.Length == 8 && aresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                if (yueri.Substring(0, 1) != "/")
                    yueri = "/" + yueri;
                aresult = aresult.Substring(0, 4) + yueri;
                aresult = aresultNewMethod(aresult);
            }
            //WA仲盛17/1/6-2/6电费。
            else if (aresult.Length <= 4 && aresult.Length >= 3 && aresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                }
                else if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        int pie_weizhi = tx.IndexOf("/");
                        if (pie_weizhi < 2)
                        {
                            tx = DateTime.Now.Year.ToString() + "/" + tx;

                            tx = aresultNewMethod(aresult);
                        }
                        else
                        {
                            tx = tx.Substring(1, tx.Length - 1);
                            tx = "20" + tx;
                        }
                        if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                            aresult = tx + "/01";
                        else
                            aresult = tx;

                    }
                }
            }
            //WA昆城广场17/2/16-17/3/10电费。
            else if (aresult.Length == 5 && aresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                }
            }
            //绍兴银泰城NC 2017.4-6租金部分
            else if (aresult.Length == 5 && aresult.Substring(0, 2) == "20" && temp3[0].Contains("."))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], ".").Count;
                if (temp3[0].Contains("."))
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Contains("/") && tx.Length >= 6 && tx.Length <= 7 && pie_count >= 1 && pie_count <= 2)
                            aresult = tx + "/01";
                        aresult = aresultNewMethod(aresult);
                    }
                }
            }
            //F2549AR17000099-PD协信2016.01-4物业管理费  ||F2549AR17000094-NC协信2016.12水电费
            else if (time == "" && item.wenben != null && !item.wenben.Contains("/") && newresult != "" && newresult.Length > 4 && !newresult.Substring(0, 2).Contains("20") && index20 > 0 && indexhao < 0)
            {
                //   item.wenben = "F2549AR17000099-PD协信2016.01-4物业管理费";

                int indexend = 0;
                //杭 位置
                var cname = item.wenben.ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true && ic > index20)// 
                    {
                        indexend = ic;
                        break;
                    }
                }
                //杭州万象城3.21-4.20电费
                if (index20 >= 0)
                {
                    int index20_count = System.Text.RegularExpressions.Regex.Matches(item.wenben, "20").Count;
                    if (index20_count == 1 && newresult.Substring(newresult.Length - 2, 2) == "20")
                    {
                        string nian = "";
                        string yue = "";
                        string ri = "";
                        string timeend = "";

                        int indexniani = temp3[0].IndexOf("年");
                        int indexyuei = temp3[0].IndexOf("月");
                        int indexhaoi = temp3[0].IndexOf("号");
                        int indexhaori = temp3[0].IndexOf("日");

                        if (indexyuei > 0 && indexniani < 0)
                        {
                            //    time1 = year_split(time, timeend);
                            time1 = NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        }

                        time1 = NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                        aresult = time;
                        indexniani = temp3[1].IndexOf("年");
                        indexyuei = temp3[1].IndexOf("月");
                        indexhaoi = temp3[1].IndexOf("号");
                        indexhaori = temp3[1].IndexOf("日");
                        if (indexyuei < 0 && bresult.Length == 2)
                        {
                            if (indexhaori > 0 || indexhaoi > 0)
                            {
                                aresult = aresultNewMethod(aresult);
                                bresult = aresult;
                                bresult = bresultNewMethod(bresult);
                            }
                        }
                        else if (indexyuei < 0 && bresult.Length >= 2 && bresult.Length <= 4)
                        {
                            string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");

                            bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length);
                            bresult = bresult.Replace("//", "/");
                            bresult = bresultNewMethod(bresult);
                        }
                        if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                        {
                            // time1 = for_yue(aresult, bresult);
                            time1 = year_split(aresult, bresult);
                            return;
                        }
                    }
                    //WA 长春活力城12.12-12.31 租金2015
                    else if (index20_count == 1 && newresult.Substring(newresult.Length - 2, 2) != "20" && index20 > 0)
                    {
                        index20 = newresult.IndexOf("20");
                        int index_count_20 = System.Text.RegularExpressions.Regex.Matches(newresult, "20").Count;
                        string nian = "";
                        string yue = "";
                        string ri = "";
                        string timeend = "";

                        int nian_i = temp3[0].IndexOf("年");
                        if (index20 + 4 <= newresult.Length)
                        {
                            nian = newresult.Substring(index20, 4);
                            string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                            int piecont = System.Text.RegularExpressions.Regex.Matches(yueri, "/").Count;
                            if (nian.Length == 4 && yueri.Substring(0, 1) == "/" && piecont == 2 && yueri.Length < 6)
                            {
                                aresult = nian + yueri;
                                aresult = aresultNewMethod(aresult);
                            }
                            //F2549AR17000105-PD协信2016.02-04推广费
                            else if (nian.Length == 4 && yueri.Substring(0, 1) == "/" && piecont == 2 && yueri.Length > 6)
                            {
                                index20 = item.wenben.IndexOf("20");

                                indexend = 0;
                                //杭 位置
                                cname = item.wenben.ToCharArray();
                                for (int ic = 0; ic < cname.Length; ic++)
                                {
                                    string jdd = cname[ic].ToString();

                                    bool ischina = newHasChineseTest(jdd);
                                    if (ischina == true && ic > index20)// 
                                    {
                                        indexend = ic;
                                        break;
                                    }
                                }
                                //截取后段日期
                                if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0)
                                    riqijiequ = item.wenben.Substring(index20, indexend - index20);
                                else if (indexnian > 0 && indexyue > 0 && indexhao < 0 && indexri < 0)
                                {
                                    indexend = 0;
                                    //
                                    cname = item.wenben.ToCharArray();
                                    for (int ic = 0; ic < cname.Length; ic++)
                                    {
                                        string jdd = cname[ic].ToString();

                                        bool ischina = newHasChineseTest(jdd);
                                        if (ischina == true && ic > nian_i)
                                        {
                                            indexend = ic;
                                            break;
                                        }
                                    }
                                    riqijiequ = item.wenben.Substring(index20, indexend - index20).Replace(".", "/");
                                    if (indexend - nian_i < 4)
                                        riqijiequ = riqijiequ + "月01日";
                                    riqijiequ = clsCommHelp.objToDateTime1(riqijiequ);


                                }
                                if (!riqijiequ.Contains("-"))
                                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                                else
                                {
                                    if (temp3.Length == 3)
                                        aresult = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                                    if (aresult.Length >= 6 && aresult.Length <= 8 && aresult.Substring(0, 1) == "/")
                                    {
                                        aresult = aresult.Substring(1, aresult.Length - 1) + "/01";
                                        aresult = bresultNewMethod(aresult);
                                    }
                                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(temp3[2], @"[^0-9]+", "/");
                                    int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                                    if (pie_count == 1 && riqijiequ.Length <= 3)
                                    {
                                        bresult = riqijiequ + "/01";
                                        bresult = bresultNewMethod(bresult);
                                    }
                                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult;
                                    bresult = bresultNewMethod(bresult);
                                }
                                if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                                {
                                    // time1 = for_yue(aresult, bresult);
                                    time1 = year_split(aresult, bresult);
                                    return;
                                }

                            }
                            //*F2549AR17000123PM090117*WA合肥万象城2017.08-POS机使用费
                            else if (nian.Length == 4 && yueri.Substring(0, 1) == "/" && piecont >= 2 && index_count_20 == 1)
                            {
                                index20 = item.wenben.IndexOf("20");

                                indexend = 0;
                                //机 位置
                                cname = item.wenben.ToCharArray();
                                for (int ic = 0; ic < cname.Length; ic++)
                                {
                                    string jdd = cname[ic].ToString();

                                    bool ischina = newHasChineseTest(jdd);
                                    if (ischina == true && ic > index20)// 
                                    {
                                        indexend = ic;
                                        break;
                                    }
                                }

                                if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0)
                                {
                                    riqijiequ = item.wenben.Substring(index20, indexend - index20);
                                    riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                                }
                                else if (indexnian > 0 && indexyue > 0 && indexhao < 0 && indexri < 0)
                                {
                                    indexend = 0;
                                    //
                                    cname = item.wenben.ToCharArray();
                                    for (int ic = 0; ic < cname.Length; ic++)
                                    {
                                        string jdd = cname[ic].ToString();

                                        bool ischina = newHasChineseTest(jdd);
                                        if (ischina == true && ic > nian_i)
                                        {
                                            indexend = ic;
                                            break;
                                        }
                                    }
                                    riqijiequ = item.wenben.Substring(index20, indexend - index20).Replace(".", "/");
                                    if (indexend - nian_i < 4)
                                        riqijiequ = riqijiequ + "月01日";
                                    riqijiequ = clsCommHelp.objToDateTime1(riqijiequ);


                                }
                                riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                                int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                                if (pie_count == 2 && riqijiequ.Length == 8)
                                    aresult = riqijiequ + "/01";

                                aresult = bresultNewMethod(aresult);
                            }
                            yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");

                            if (yueri == "/")
                                yueri = aresult;

                            bresult = bresultNewMethod(yueri);
                        }

                        if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                        {
                            // time1 = for_yue(aresult, bresult);
                            time1 = year_split(aresult, bresult);
                            return;
                        }
                    }
                }
                //
                //截取后段日期
                if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0 && index20 > 0)
                {
                    if (indexend > index20)
                    {
                        riqijiequ = item.wenben.Substring(index20, indexend - index20);
                        string[] newtemp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");

                        string[] tempdian = System.Text.RegularExpressions.Regex.Split(newtemp3[0], riqijiequ.Substring(0, 4));

                        string yueri = tempdian[1].Replace(".", "/");

                        if (yueri.Substring(0, 1) != "/")
                            yueri = "/" + yueri;
                        aresult = riqijiequ.Substring(0, 4) + yueri;


                        aresult = aresultNewMethod(aresult);
                        aresult = System.Text.RegularExpressions.Regex.Replace(aresult, @"[^0-9]+", "/");
                        if (aresult.Length > 10)
                        {

                        }
                        int pie_count = System.Text.RegularExpressions.Regex.Matches(aresult, "/").Count;
                        if (pie_count == 1 && aresult.Length == 7)
                            time1 = aresult + "/01";

                        //
                        indexend = 0;
                        //杭 位置
                        if (newtemp3.Length > 1)
                        {
                            cname = newtemp3[1].ToCharArray();
                            for (int ic = 0; ic < cname.Length; ic++)
                            {
                                string jdd = cname[ic].ToString();

                                bool ischina = newHasChineseTest(jdd);
                                if (ischina == true)// 
                                {
                                    indexend = ic;
                                    break;
                                }
                            }
                            if (indexend == 0)
                                bresult = System.Text.RegularExpressions.Regex.Replace(newtemp3[1], @"[^0-9]+", "/");
                            else
                            {
                                bresult = newtemp3[1].Substring(0, indexend);
                            }
                            bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult;
                            pie_count = System.Text.RegularExpressions.Regex.Matches(bresult, "/").Count;
                            if (pie_count == 1 && bresult.Length <= 7)
                                bresult = bresult + "/01";
                        }
                        else if (newtemp3.Length == 1)
                            bresult = aresult;
                        bresult = bresultNewMethod(bresult);

                        aresult = aresultNewMethod(aresult);
                        if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                        {
                            // time1 = for_yue(aresult, bresult);
                            time1 = year_split(aresult, bresult);
                            return;
                        }
                    }
                    else
                    {


                    }

                }
            }
            //朝北 PR 5.4-17日特卖租金
            else if (aresult.Length == 2 && aresult.Substring(0, 2) != "20" && temp3[0].Contains("."))
            {

                string nian = "";
                string yue = "";
                string ri = "";
                string timeend = "";

                int indexniani = temp3[0].IndexOf("年");
                int indexyuei = temp3[0].IndexOf("月");
                int indexhaoi = temp3[0].IndexOf("号");
                int indexhaori = temp3[0].IndexOf("日");

                if (indexyuei > 0 && indexniani < 0)
                {
                    //    time1 = year_split(time, timeend);
                    time1 = NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                }

                time1 = NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                aresult = time;

                //17日特卖租金
                indexniani = temp3[1].IndexOf("年");
                indexyuei = temp3[1].IndexOf("月");
                indexhaoi = temp3[1].IndexOf("号");
                indexhaori = temp3[1].IndexOf("日");
                if (indexyuei < 0 && bresult.Length == 2)
                    if (indexhaori > 0 || indexhaoi > 0)
                    {
                        aresult = aresultNewMethod(aresult);
                        bresult = aresult;
                        bresult = bresultNewMethod(bresult);
                    }
                if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                {
                    // time1 = for_yue(aresult, bresult);
                    time1 = year_split(aresult, bresult);
                    return;
                }

            }
            else if (aresult.Length <= 5 && aresult.Length >= 3 && aresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {
                temp3[0] = temp3[0].Replace(".", "/");
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[0], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                }
                else if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        int pieweizhi = tx.IndexOf("/");
                        if (pieweizhi != 1)
                            tx = "20" + tx;
                        else if (pieweizhi == 1 && tx.Length == 4)
                            aresult = DateTime.Now.Year.ToString() + "/" + tx;
                        if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                            aresult = tx + "/01";
                        else if (tx.Contains("/") && tx.Length >= 8 && tx.Length <= 10 && System.Text.RegularExpressions.Regex.Matches(tx, "/").Count == 2)
                            aresult = tx;

                    }
                }
            }
            //朝北TT20161019-1101特卖抽成租金
            else if (aresult.Length == 8 && aresult.Substring(0, 2) == "20" && !aresult.Contains(".") && !aresult.Contains("/"))
            {
                aresult = clsCommHelp.objToDateTime(aresult);
            }
            //2016年4-6、7、8 物业费补税金
            else if (aresult.Length == 5 && aresult.Substring(0, 2) == "20" && indexnian > 0)
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = System.Text.RegularExpressions.Regex.Replace(tempdian[1], @"[^0-9]+", "/").Replace(".", "/");
                //  aresult = "20" + aresult;
                aresult = aresult.Substring(0, 4) + yueri;
                int pie_count = System.Text.RegularExpressions.Regex.Matches(aresult, "/").Count;
                if (pie_count == 1 && aresult.Length == 6)
                    aresult = aresult + "/01";

            }
            //NC合生汇17年5-6月POS租金
            else if (aresult.Length == 3 && aresult.Substring(0, 2) != "20" && indexnian > 0)
            {
                string nianyue = System.Text.RegularExpressions.Regex.Replace(temp3[0], @"[^0-9]+", "/").Replace(".", "/");
                int pie_count = System.Text.RegularExpressions.Regex.Matches(nianyue, "/").Count;
                int pieindex = nianyue.IndexOf("/");
                if (pie_count == 2 && nianyue.Substring(0, 1) == "/")
                {
                    nianyue = nianyue.Substring(1, nianyue.Length - 1);
                    aresult = "20" + nianyue + "/01";
                }
                //16年5-8月抽成租金津嘉里汇NC

                else if (pie_count == 1 && nianyue.Substring(0, 1) != "/" && indexnian > 0 && pieindex < nianyue.Length - 1)
                {
                    //  nianyue = nianyue.Substring(0, nianyue.Length - 1);
                    aresult = "20" + nianyue + "/01";
                }

            }
            //F2549AR17000089-NC协信2015.12推广费/手续费
            else if (System.Text.RegularExpressions.Regex.Replace(aresult, @"[^0-9]+", "").Length >= 9 && aresult.Substring(0, 2) != "20" && System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "").Length >= 3)
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], ".").Count;
                if (temp3[1].Contains("."))
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;
                        if (pie_count == 2 && tx.LastIndexOf("/") + 1 == tx.Length && tx.Length <= 8)
                        {
                            bresult = tx + "/01";
                        }
                        else if (tx.Contains("/") && tx.Length == 6 && pie_count == 1)
                            bresult = tx + "/01";

                    }
                    else if (tx.Substring(0, 1) != "/" && bresult.Substring(0, 2) == "20")
                    {

                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Contains("/") && tx.Length == 6 && pie_count == 1)
                            bresult = tx + "/01";
                        if (tx.Length == 7 && pie_count == 2 && tx.Substring(tx.Length - 1, 1) == "/")
                            bresult = tx + "01";

                    }

                }
                bresult = bresultNewMethod(bresult);
                aresult = bresult;

                if (newadd == true)
                {
                    time1 = time1 + "_" + year_split(aresult, bresult);

                }
                else
                    time1 = year_split(aresult, bresult);

                return;

            }
            //NC上海南西卖场租金(2017/6/25-9/24)
            else if (aresult.Length >= 7 && aresult.Length <= 8 && aresult.Substring(0, 2) == "20" && indexnian < 0 && pie_count_temp30 == 2)
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;
                if (yueri.Substring(0, 1) != "/")
                    yueri = "/" + yueri;
                aresult = aresult.Substring(0, 4) + yueri;
                aresult = aresultNewMethod(aresult);

            }
            //RC 北京金源燕莎2017.4租金  2077-2078 新卖场装修期
            else if (aresult.Length >= 7 && aresult.Length <= 9 && aresult.Substring(0, 2) == "20" && indexnian < 0 && erling_count_temp30 == 2)
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[0], aresult.Substring(0, 4));
                //截取第一个20 的内容
                int indexend = temp3[0].IndexOf("20");
                string riqijiequ2 = "";

                var cname = temp3[0].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true && ic > indexend)// 
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend > 0)
                {
                    riqijiequ = item.wenben.Substring(temp3[0].IndexOf("20"), indexend - temp3[0].IndexOf("20")).Replace(".", "/");


                }
                //截取第二个 20的内容
                indexend = temp3[0].LastIndexOf("20");
                int newindexend = 0;
                cname = temp3[0].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true && ic > indexend)// 
                    {
                        newindexend = ic;
                        break;
                    }
                }
                if (newindexend > 0)
                {
                    riqijiequ2 = item.wenben.Substring(temp3[0].LastIndexOf("20"), indexend - temp3[0].LastIndexOf("20")).Replace(".", "/");
                }
                else
                {
                    riqijiequ2 = item.wenben.Substring(temp3[0].LastIndexOf("20"), temp3[0].Length - temp3[0].LastIndexOf("20")).Replace(".", "/");
                }
                if (riqijiequ.Length > riqijiequ2.Length)
                {
                    if (riqijiequ.Length >= 6 && riqijiequ.Length <= 7)
                        riqijiequ = riqijiequ + "/01";

                    aresult = aresultNewMethod(riqijiequ);
                    //
                    bresult = aresult;
                    bresult = bresultNewMethod(bresult);

                    if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
                    {
                        // time1 = for_yue(aresult, bresult);
                        time1 = year_split(aresult, bresult);
                        return;
                    }
                }
            }
            //EE 北京金源燕莎5月23日-6月4日特卖租金
            else if (riqijiequ != null && riqijiequ != "" && index20 < 0 && item.wenben.Contains("-") && temp3.Length == 2 && temp3[0].IndexOf("月") > 0 && temp3[0].IndexOf("年") < 0 && temp3[0].IndexOf("日") > 0 && temp3[1].IndexOf("月") > 0 && temp3[1].IndexOf("年") < 0 && temp3[1].IndexOf("日") > 0 && newresult.Length >= 4)
            {
                temp3 = System.Text.RegularExpressions.Regex.Split(riqijiequ, "-");
                string nian = "";
                string yue = "";
                string ri = "";
                string timeend = "";
                int indexniani = temp3[0].IndexOf("年");
                int indexyuei = temp3[0].IndexOf("月");
                int indexhaoi = temp3[0].IndexOf("号");
                int indexhaori = temp3[0].IndexOf("日");

                if (indexyuei > 0 && indexniani < 0)
                {
                    //    time1 = year_split(time, timeend);
                    NO_yearTime1(item, ref time1, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);
                }

                NO_yearTime_end(ref time1, riqijiequ, ref time, temp3, ref timeend, ref yue, ref ri, ref indexniani, ref indexyuei, ref indexhaoi, ref indexhaori);

            }

            ////////////////////////////////

            if (bresult.Length == 4)
            {

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult.Substring(0, 2) + "/" + bresult.Substring(2, 2);
                //FO宁波环球银泰城17.8-17.10保底租金
                if (Convert.ToDouble(System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "")) > 12)
                {
                    temp3[1] = temp3[1].Replace(".", "/");
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                    if (pie_count == 2)
                    {
                        string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                        if (tx.Substring(0, 1) == "/")
                        {
                            tx = tx.Substring(1, tx.Length - 1);
                            bresult = "20" + tx;
                        }
                    }
                    else if (pie_count == 1)
                    {
                        string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Substring(0, 1) == "/")
                        {
                            tx = tx.Substring(1, tx.Length - 1);
                            int pieweizhi = tx.IndexOf("/");
                            if (pieweizhi != 1)
                                tx = "20" + tx;
                            else if (pieweizhi == 1 && tx.Length == 4)
                                bresult = DateTime.Now.Year.ToString() + "/" + tx;
                            if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                                bresult = tx + "/01";
                        }
                        else if (pie_count == 2)
                        {
                            tx = tx.Substring(0, tx.Length - 1);
                            int pieweizhi = tx.IndexOf("/");
                            if (pieweizhi != 1)
                                tx = "20" + tx;
                            else if (pieweizhi == 1 && tx.Length == 4)
                                bresult = DateTime.Now.Year.ToString() + "/" + tx;
                            if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                                bresult = tx + "/01";
                            else if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 7 && pie_count == 2)
                                bresult = tx + "/01";
                        }
                    }
                }
            }
            else if (bresult.Length == 1 || bresult.Length == 2)
            {
                //WA仲盛17/6.7-7.6电费。
                if (Convert.ToDouble(bresult.Substring(0, bresult.Length)) > 12)
                {
                    if (Convert.ToDouble(bresult.Substring(0, 1)) > 2)
                    {
                        if (bresult.Length == 2 && aresult.Length > 6)
                        {
                            bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult.Substring(0, bresult.Length - 1) + "/" + bresult.Substring(1, 1);

                        }
                    }
                }
                else
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + bresult.Substring(0, bresult.Length) + "/30";

            }

             //镇江万达NC2017.5.1-7.31房屋租金发票登记
            else if (bresult.Length == 3 && item.wenben.Contains(".") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");

                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length);
                bresult = bresult.Replace("//", "/");
                bresult = bresultNewMethod(bresult);
            }
            //   PO 上海莘庄仲盛2016.12.6-2017.2.6号电费
            else if (bresult.Length == 6 && item.wenben.Contains(".") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) == "20")
            {
                //判断2017.2.6 后的第一个汉字位数
                int indexend = 0;

                var cname = temp3[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend == 0)
                {
                    string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[1], bresult.Substring(0, 4));
                    string yueri01 = tempdian[1].Replace(".", "/");
                    //  aresult = "20" + aresult;
                    bresult = bresult.Substring(0, 4) + yueri01;
                }
                else

                    bresult = temp3[1].Substring(0, indexend);

                string yueri = bresult.Replace(".", "/");
                //  aresult = "20" + aresult;
                bresult = bresultNewMethod(bresult);

            }
            //WA仲盛17/1/6-2/6电费。
            else if (bresult.Length == 2 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);

                }
            }
            else if (bresult.Length == 5 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("/"))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        bresult = "20" + tx;
                    }
                    bresult = Convert.ToDateTime(bresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);

                }
                else if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        bresult = "20" + tx;
                    }
                    else if (tx.Length == 8)
                    {

                        tx = tx.Substring(0, tx.Length);
                        bresult = "20" + tx;
                        if (bresult.Length == 10 && bresult.Substring(9, 1) == "/")
                            bresult = bresult.Substring(0, 9);
                    }
                }
            }
            //杭州星光NC2017.6.15-2017.9.14物管费
            else if (bresult.Length >= 7 && bresult.Substring(0, 2) == "20" && item.wenben.Contains("."))
            {
                string[] tempdian = System.Text.RegularExpressions.Regex.Split(temp3[1], bresult.Substring(0, 4));
                string yueri = tempdian[1].Replace(".", "/");
                //  aresult = "20" + aresult;

                //EE哈尔滨江北万达首期租金(2017.06.30-2017.09.30)102194.52
                int indexkuohao = yueri.IndexOf(")");
                if (indexkuohao > 0)
                    yueri = yueri.Substring(0, indexkuohao);
                int indexend = 0;
                //
                var cname = yueri.ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)//&& ic > nian_i
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend == 0)
                    yueri = yueri;
                else
                    yueri = yueri.Substring(0, indexend);

                bresult = bresult.Substring(0, 4) + yueri;
                bresult = System.Text.RegularExpressions.Regex.Replace(bresult, @"[^0-9]+", "/");
                bresult = bresultNewMethod(bresult);
            }
            //NC苏州绿宝2017年6.16-7.15电费1765.56
            else if (bresult.Length >= 7 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {
                int indexend = 0;
                //
                var cname = temp3[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)//&& ic > nian_i
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend > 0)
                {
                    bresult = temp3[1].Substring(0, indexend);


                    string yueri = bresult.Replace(".", "/");
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(yueri, "/").Count;
                    if (yueri.Substring(0, 1) != "/" && pie_count < 2)
                    {
                        yueri = "/" + yueri;

                        bresult = Convert.ToDateTime(bresult).Year.ToString() + yueri;
                    }
                    else if (yueri.Substring(0, 1) != "/" && pie_count == 2 && yueri.Substring(0, 2) != "20")
                    {
                        bresult = "20" + yueri;
                        if (bresult.Length == 10 && bresult.Substring(9, 1) == "/")
                            bresult = bresult.Substring(0, 9);
                    }


                    bresult = bresultNewMethod(bresult);
                }
            }
            //莘庄pd 2017.5.6-6.6电费
            else if (bresult.Length == 2 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {

                //if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        aresult = "20" + tx;
                    }
                    //朝北EE 抬头2016.9.17-29日特卖租金
                    bool isrun = false;

                    if (indexri > 0)
                    {
                        int indexri_new = item.wenben.IndexOf(System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", ""));
                        if (indexri - indexri_new <= 2)
                        {
                            bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + Convert.ToDateTime(aresult).Month.ToString() + "/" + tx;
                            isrun = true;
                            //
                        }
                    }
                    if (isrun == false)
                        bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + tx;
                    bresult = bresultNewMethod(bresult);
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(bresult, "/").Count;
                    int lastpie = 0;
                    if (pie_count == 2)
                        lastpie = bresult.LastIndexOf("/");

                    if (bresult != null && bresult.Length == 8 && pie_count == 2 && lastpie != 0 && lastpie < bresult.Length)
                        bresult = bresult + "";
                    else if (bresult != null && bresult.Length == 8)
                        bresult = bresult + "01";
                }
            }
            else if (bresult.Length == 3 && item.wenben.Contains("/") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                int pie_count = System.Text.RegularExpressions.Regex.Matches(yueri, "/").Count;


                //WA大宁国际16/5-17/4补交租金。
                if (bresult.Length <= 4 && bresult.Length >= 3 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("/") && pie_count == 2)
                {

                    if (pie_count == 2)
                    {
                        if (yueri.Substring(0, 1) == "/")
                        {
                            yueri = yueri.Substring(1, yueri.Length - 1);
                            bresult = "20" + yueri;
                        }
                        else
                        {
                            //WA昆山17/5/1-5/31提成租金。

                            int pieindex = yueri.IndexOf("/");
                            if (pieindex < 2)
                            {
                                bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length - 1);
                                bresult = bresult.Replace("//", "/");
                            }
                            else
                                bresult = "20" + yueri;
                            if (bresult.Length <= 7)
                                bresult = bresult + "/01";
                            bresult = bresult.Replace("//", "/");
                            bresult = bresultNewMethod(bresult);
                        }
                    }

                }
                else
                {
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length - 1);
                    bresult = bresult.Replace("//", "/");
                }


            }

            else if (bresult.Length == 6 && !item.wenben.Contains("/") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20")
            {
                int indexend = 0;
                //
                var cname = temp3[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)//&& ic > nian_i
                    {
                        indexend = ic;
                        break;
                    }
                }

                string yueri = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                if (yueri.Length == 9)
                    bresult = "20" + yueri.Substring(0, yueri.Length - 1);
                else
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length);
                bresult = bresult.Replace("//", "/");


            }

               // 2016年4-6、7、8 物业费补税金
            else if (bresult != null && bresult != "" && item.wenben.Contains("、") && item.wenben.Contains("-") && aresult.Substring(0, 2) == "20" && bresult.Substring(0, 2) != "20" && bresult.Length > 1)
            {
                string[] temp = System.Text.RegularExpressions.Regex.Split(temp3[1], "、");
                string yueri = System.Text.RegularExpressions.Regex.Replace(temp[0], @"[^0-9]+", "/");
                if (yueri.Length == 9)
                    bresult = "20" + yueri.Substring(0, yueri.Length - 1);
                else
                    bresult = Convert.ToDateTime(aresult).Year.ToString() + "/" + yueri.Substring(0, yueri.Length);
                bresult = bresultNewMethod(bresult);
                int pie_count = System.Text.RegularExpressions.Regex.Matches(bresult, "/").Count;
                if (bresult != null && bresult.Length == 6 && pie_count == 1)
                    bresult = bresult + "/01";
                time1 = "";

                for (int a = 1; a < temp.Length; a++)
                {
                    time1 = time1 + "_" + Convert.ToDateTime(aresult).Year.ToString() + "/" + System.Text.RegularExpressions.Regex.Replace(temp[a], @"[^0-9]+", "/") + "/01";
                    newadd = true;
                }
                time1 = time1.Replace("//", "/");
            }

            //杭州星光大道NC15.12.1-16.1.18
            else if (bresult.Length <= 5 && bresult.Length >= 3 && bresult.Substring(0, 2) != "20" && item.wenben.Contains("."))
            {
                temp3[1] = temp3[1].Replace(".", "/");
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], "/").Count;
                if (pie_count == 2)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        bresult = "20" + tx;
                    }
                    else if (tx.Substring(0, 1) != "/" && tx.Length == 7)
                    {
                        bresult = "20" + tx;
                    }
                    else if (tx.Substring(0, 1) != "/" && tx.Length == 8 && pie_count == 2)
                    {
                        bresult = "20" + tx;
                    }
                }
                else if (pie_count == 1)
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {

                        tx = tx.Substring(1, tx.Length - 1);
                        int pieweizhi = tx.IndexOf("/");
                        if (pieweizhi != 1)
                            tx = "20" + tx;
                        else if (pieweizhi == 1 && tx.Length == 4)
                            bresult = DateTime.Now.Year.ToString() + "/" + tx;
                        if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                            bresult = tx + "/01";
                    }
                    else
                    {
                        int pieweizhi = tx.IndexOf("/");
                        if (pieweizhi != 1)
                            tx = "20" + tx;
                        else if (pieweizhi == 1 && tx.Length == 4)
                            bresult = DateTime.Now.Year.ToString() + "/" + tx;
                        if (tx.Contains("/") && tx.Length >= 5 && tx.Length <= 6 && pie_count == 1)
                            bresult = tx + "/01";
                    }
                }
            }
            //NC常州九洲新世界2017.7-2017.9物管费
            else if (bresult.Length == 5 && bresult.Substring(0, 2) == "20" && temp3[1].Contains("."))
            {
                int pie_count = System.Text.RegularExpressions.Regex.Matches(temp3[1], ".").Count;
                if (temp3[1].Contains("."))
                {
                    string tx = System.Text.RegularExpressions.Regex.Replace(temp3[1], @"[^0-9]+", "/");
                    if (tx.Substring(0, 1) == "/")
                    {
                        tx = tx.Substring(1, tx.Length - 1);
                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Contains("/") && tx.Length == 6 && pie_count == 1)
                            bresult = tx + "/01";

                    }
                    else if (tx.Substring(0, 1) != "/" && bresult.Substring(0, 2) == "20")
                    {

                        pie_count = System.Text.RegularExpressions.Regex.Matches(tx, "/").Count;

                        if (tx.Contains("/") && tx.Length == 6 && pie_count == 1)
                            bresult = tx + "/01";
                        if (tx.Length == 7 && pie_count == 2 && tx.Substring(tx.Length - 1, 1) == "/")
                            bresult = tx + "01";

                    }

                }
            }
            else if (bresult.Length == 0 && aresult.Length > 0)
            {
                bresult = aresult;

            }
            //WA昆城广场20170611-20170710电费
            else if (bresult.Length == 8 && bresult.Substring(0, 2) == "20" && !bresult.Contains(".") && !bresult.Contains("/"))
            {
                bresult = clsCommHelp.objToDateTime(bresult);
            }

            //
            if (aresult != "" && aresult.Length > 6 && bresult != "" && bresult.Length > 6)
            {
                bresult = bresultNewMethod(bresult);
                aresult = aresultNewMethod(aresult);
                // time1 = for_yue(aresult, bresult);
                if (newadd == true)
                {
                    time1 = time1 + "_" + year_split(aresult, bresult);

                }
                else
                    time1 = year_split(aresult, bresult);
            }

        }
        private static string NO_yearTime1(clsR2accrualsapinfo item, ref string time1, ref string time, string[] tempsplit_All, ref string timeend, ref string yue, ref string ri, ref int indexniani, ref int indexyuei, ref int indexhaoi, ref int indexhaori)
        {

            if (time1 == "" && Convert.ToDateTime(item.jizhangriqi).Year.ToString() == DateTime.Now.Year.ToString())
            {
                if (tempsplit_All.Length > 0)
                {
                    string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[0], "月");
                    yue = System.Text.RegularExpressions.Regex.Replace(yues[0], @"[^0-9]+", "");

                    if (yue.Length > 2)
                    {
                        if (yue.Length > 4 && yue.Substring(0, 2) == "20")
                            yue = yue.Substring(4, yue.Length - 4);
                        else if (yue.Length > 4 && yue.Substring(0, 2) != "20")
                        {
                            yue = System.Text.RegularExpressions.Regex.Replace(yue.Substring(yue.Length - 2, 2), @"[^0-9]+", "");

                        }
                    }
                    if (yue.Length == 1)
                        yue = "0" + yue;

                    if (indexhaoi > 0 && indexyuei > 0)
                    {
                        ri = tempsplit_All[0].Substring(indexyuei, indexhaoi - indexyuei);
                        ri = System.Text.RegularExpressions.Regex.Replace(ri, @"[^0-9]+", "");
                    }
                    if (indexhaori > 0 && indexyuei > 0)
                    {
                        ri = tempsplit_All[0].Substring(indexyuei, indexhaori - indexyuei);
                        ri = System.Text.RegularExpressions.Regex.Replace(ri, @"[^0-9]+", "");
                    }
                    if (indexhaori < 0 && indexhaoi < 0)
                        time = DateTime.Now.Year.ToString() + "/" + yue + "/01";
                    else if (indexhaori > 0 && indexhaoi < 0)
                        time = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                    else if (indexhaori < 0 && indexhaoi > 0)
                    {
                        time = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                    }
                }
                if (tempsplit_All.Length > 1)
                {
                    indexniani = tempsplit_All[1].IndexOf("年");
                    indexyuei = tempsplit_All[1].IndexOf("月");
                    indexhaoi = tempsplit_All[1].IndexOf("号");
                    indexhaori = tempsplit_All[1].IndexOf("日");
                    if (indexyuei > 0)
                    {
                        string risp = tempsplit_All[1].Substring(indexyuei, 3);//EC虹口2016年9月28
                        ri = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                    }
                    if (ri.Length == 1)
                        ri = "0" + ri;
                    if (indexyuei > 0 && indexniani < 0)
                    {
                        string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[1], "月");
                        yue = System.Text.RegularExpressions.Regex.Replace(yues[0], @"[^0-9]+", "");
                        if (yue.Length == 1)
                            yue = "0" + yue;

                        if (indexhaori < 0 && indexhaoi < 0)
                            timeend = DateTime.Now.Year.ToString() + "/" + yue + "/01";
                        else if (indexhaori > 0 && indexhaoi < 0)
                            timeend = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                        else if (ri != "")
                            timeend = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                    }
                }
            }

            if (time != null && time != "" && timeend != null && timeend != "")
            {
                // 此种跨年 补没有年的情况自动 减去 前者的一年 ，EC虹口12月28-1月28号电费1328.4O元
                if (Convert.ToDateTime(timeend.Replace("//", "/")).Month < Convert.ToDateTime(time.Replace("//", "/")).Month)
                {
                    DateTime t = Convert.ToDateTime(time).AddYears(-1);
                    time = t.ToString("yyyy/MM/dd");
                }

                time1 = year_split(time.Replace("//", "/"), timeend.Replace("//", "/"));
            }
            return time1;

            // time1 = "日期无法识别";
            //return time1;
        }

        private static string for_yue(string time, string timeend)
        {
            int totalMonth = Convert.ToDateTime(timeend).Year * 12 + Convert.ToDateTime(timeend).Month - Convert.ToDateTime(time).Year * 12 - Convert.ToDateTime(time).Month;
            string timebak = time;

            time = "";
            for (int im = 0; im <= totalMonth; im++)
            {
                //temp3[0] = "2017/01/01";
                //   string ri = System.Text.RegularExpressions.Regex.Replace(timebak, @"[^0-9]+", "");
                DateTime t = Convert.ToDateTime(timebak).AddMonths(im);
                time += t.ToString("MM/dd/yyyy") + "_";
            }
            return time;
        }

        private static string NO_yearTime_end(ref string time1, string riqijiequ, ref string time, string[] tempsplit_All, ref string timeend, ref string yue, ref string ri, ref int indexniani, ref int indexyuei, ref int indexhaoi, ref int indexhaori)
        {
            if (tempsplit_All.Length == 3 && !riqijiequ.Contains("-"))
            {
                time1 = "日期无法识别";
                return time1;
            }

            //BC上海龙之梦5.1
            int index201 = tempsplit_All[0].IndexOf("20");
            int index202 = tempsplit_All[1].IndexOf("20");
            int indexnian = tempsplit_All[0].IndexOf("年");
            int indexyue = tempsplit_All[0].IndexOf("月");
            int indexhao = tempsplit_All[0].IndexOf("号");
            int indexri = tempsplit_All[0].IndexOf("日");

            yue = System.Text.RegularExpressions.Regex.Replace(tempsplit_All[0], @"[^0-9]+", "/");
            int pie_count0 = System.Text.RegularExpressions.Regex.Matches(yue, "/").Count;
            if (yue.Length == 1)
                yue = "0" + yue;
            //EE 北京金源燕莎5月23日
            if (indexri > 0 || indexhao > 0)
            {
                if (pie_count0 == 3 && System.Text.RegularExpressions.Regex.Replace(yue, @"[^0-9]+", "").Length > 1 && indexyue > 0)
                {
                    string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[0], "月");
                    yue = System.Text.RegularExpressions.Regex.Replace(yues[0], @"[^0-9]+", "");

                }
            }
            if (indexhaori < 0 && indexhaoi < 0 && yue.Contains("/"))
                time = DateTime.Now.Year.ToString() + "/" + yue;
            else if (indexhaori < 0 && indexhaoi < 0 && !yue.Contains("/"))
                time = DateTime.Now.Year.ToString() + "/" + yue + "/01";
            else if (indexhaori > 0 && indexhaoi < 0)
                time = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
            else if (ri != "" && tempsplit_All[0].IndexOf("年") < 0)
                time = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
            else if (pie_count0 == 4 && indexnian > 0 && yue.Substring(0, 1) == "/")
            {
                yue = yue.Substring(1, yue.Length - 1);
                int pie = yue.IndexOf("/");
                if (yue.Substring(0, pie).Length == 2)
                    time = "20" + yue;
                time = aresultNewMethod(time);
            }
            //
            time = time.Replace("//", "/");
            indexniani = tempsplit_All[1].IndexOf("年");
            indexyuei = tempsplit_All[1].IndexOf("月");
            indexhaoi = tempsplit_All[1].IndexOf("号");
            indexhaori = tempsplit_All[1].IndexOf("日");
            if (indexyuei > 0)
            {
                if (indexyuei > 2)
                {

                    string risp = tempsplit_All[1].Substring(indexyuei, 3);//EC虹口2016年9月28
                    ri = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                }
                else if (indexyuei < 2)
                {
                    string risp = tempsplit_All[1].Substring(0, 2);//EC虹口2016年9月28
                    ri = System.Text.RegularExpressions.Regex.Replace(risp, @"[^0-9]+", "");
                    //6月4日特卖租金
                    if (indexri > 0 || indexhao > 0)
                    {
                        if (indexyuei > 0)
                        {
                            if (pie_count0 == 3 && System.Text.RegularExpressions.Regex.Replace(tempsplit_All[1], @"[^0-9]+", "").Length > 1 && indexyue > 0)
                            {
                                string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[1], "月");
                                ri = System.Text.RegularExpressions.Regex.Replace(yues[1], @"[^0-9]+", "");

                            }
                        }
                    }
                }
            }
            if (ri.Length == 1)
                ri = "0" + ri;
            if (indexyuei > 0 && indexniani < 0)
            {
                string[] yues = System.Text.RegularExpressions.Regex.Split(tempsplit_All[1], "月");
                yue = System.Text.RegularExpressions.Regex.Replace(yues[0], @"[^0-9]+", "/");
                if (yue.Length == 1)
                    yue = "0" + yue;
                int index20 = yue.IndexOf("20");
                if (indexhaori < 0 && indexhaoi < 0 && yue.Contains("/") && index20 < 0)
                    timeend = DateTime.Now.Year.ToString() + "/" + yue;
                else if (indexhaori < 0 && indexhaoi < 0 && !yue.Contains("/") && index20 < 0)
                    timeend = DateTime.Now.Year.ToString() + "/" + yue + "/01";
                else if (indexhaori > 0 && indexhaoi < 0 && index20 < 0)
                    timeend = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                else if (ri != "" && index20 < 0)
                    timeend = DateTime.Now.Year.ToString() + "/" + yue + "/" + ri;
                else if (indexhaori < 0 && indexhaoi < 0 && yue.Contains("/") && ri == "" && index20 >= 0)
                {
                    int pie_count = System.Text.RegularExpressions.Regex.Matches(yue, "/").Count;

                    if (index20 == 0 && pie_count == 1)
                    {
                        timeend = yue + "/28";
                        timeend = timeend.Replace("//", "/");
                    }
                }
            }
            //tempsplit_All[1]= 8.17POS 租金
            else if (indexyuei == -1 && indexniani < 0)
            {
                //  6.6电费4356
                int indexend = 0;

                var cname = tempsplit_All[1].ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true)//&& ic > nian_i
                    {
                        indexend = ic;
                        break;
                    }
                }
                tempsplit_All[1] = tempsplit_All[1].Substring(0, indexend);

                yue = System.Text.RegularExpressions.Regex.Replace(tempsplit_All[1], @"[^0-9]+", "/");
                if (yue.Length == 1 && yue != "/")
                    yue = "0" + yue;
                if (indexhaori < 0 && indexhaoi < 0 && yue.Contains("/") && yue != "/")
                    timeend = DateTime.Now.Year.ToString() + "/" + yue;
                else if (indexhaori < 0 && indexhaoi == 3 && yue.Contains("/") && yue != "/")
                    timeend = DateTime.Now.Year.ToString() + "/" + yue;


                if (timeend.Length == 10 && timeend.Substring(9, 1) == "/")
                    timeend = timeend.Substring(0, 9);
                if (timeend.Length == 9 && timeend.Substring(8, 1) == "/")
                    timeend = timeend.Substring(0, 8);

            }
            //CO 上海K11-2017.5月租金83664
            if (index201 < 0 && index202 >= 0)
                time = timeend;

            //EE 武汉凯德1818 2017.3-4月租金核销
            if (tempsplit_All[0].Contains("20") && time.Length > 11)
            {
                #region MyRegion
                int indexend = 0;
                //杭 位置
                string nl = tempsplit_All[0];
                int index20 = nl.IndexOf("20");
                int nian_i = tempsplit_All[0].IndexOf("年");
                var cname = nl.ToCharArray();
                for (int ic = 0; ic < cname.Length; ic++)
                {
                    string jdd = cname[ic].ToString();

                    bool ischina = newHasChineseTest(jdd);
                    if (ischina == true && ic > index20)// 
                    {
                        indexend = ic;
                        break;
                    }
                }
                if (indexend == 0)
                    indexend = nl.Length;

                //截取后段日期
                if (indexnian < 0 && indexyue < 0 && indexhao < 0 && indexri < 0)
                    riqijiequ = nl.Substring(index20, indexend - index20);
                else if (indexnian > 0 && indexyue > 0 && indexhao < 0 && indexri < 0)
                {
                    indexend = 0;
                    //
                    cname = nl.ToCharArray();
                    for (int ic = 0; ic < cname.Length; ic++)
                    {
                        string jdd = cname[ic].ToString();

                        bool ischina = newHasChineseTest(jdd);
                        if (ischina == true && ic > nian_i)
                        {
                            indexend = ic;
                            break;
                        }
                    }
                    riqijiequ = nl.Substring(index20, indexend - index20).Replace(".", "/");
                    if (indexend - nian_i < 4)
                        riqijiequ = riqijiequ + "月01日";
                    riqijiequ = clsCommHelp.objToDateTime1(riqijiequ);
                    return riqijiequ;

                }
                riqijiequ = System.Text.RegularExpressions.Regex.Replace(riqijiequ, @"[^0-9]+", "/");
                int pie_count = System.Text.RegularExpressions.Regex.Matches(riqijiequ, "/").Count;
                if (pie_count == 1 && riqijiequ.Length <= 7 && riqijiequ.Length >= 6)
                    time = riqijiequ + "/01";

                #endregion

            }


            if (time != null && time != "" && timeend != null && timeend != "" && time.Length < 11 && timeend.Length < 11)
                time1 = year_split(time, timeend);

            else if (time != null && time != "" && timeend == "")
                time1 = time;

            return time1;
        }
        //静态方法
        public static bool newHasChineseTest(string text)
        {
            //string text = "是不是汉字，ABC,keleyi.com";
            char[] c = text.ToCharArray();
            bool ischina = false;

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] >= 0x4e00 && c[i] <= 0x9fbb)
                {
                    ischina = true;

                }
                else
                {
                    //  ischina = false;
                }
            }
            return ischina;

        }
        private static int Checkchinese_shuzi(int index20, int indexend, char[] cname)
        {
            int blankindex = 0;
            for (int ic = 0; ic < cname.Length; ic++)
            {
                string jdd = cname[ic].ToString();
                if (jdd == " ")
                    blankindex++;

                bool ischina = newHasChineseTest(jdd);
                bool zimu = false;
                if (Regex.Matches(jdd.ToString(), "[a-zA-Z]").Count > 0)
                    zimu = true;
                if (ic > index20)
                {
                    if (ischina == true || zimu == true)
                    {
                        indexend = ic;
                        break;
                    }
                }
            }
            return indexend;
        }

        private static string bresultNewMethod(string bresult)
        {
            bresult = bresult.Replace("//", "/");
            if (bresult.Length == 10 && bresult.Substring(9, 1) == "/")
                bresult = bresult.Substring(0, 9);
            if (bresult.Length == 9 && bresult.Substring(8, 1) == "/")
                bresult = bresult.Substring(0, 8);
            if (bresult.Length == 11 && bresult.Substring(10, 1) == "/")
                bresult = bresult.Substring(0, 10);
            return bresult;
        }
        private static string aresultNewMethod(string aresult)
        {
            aresult = aresult.Replace("//", "/");

            if (aresult.Length == 10 && aresult.Substring(9, 1) == "/")
                aresult = aresult.Substring(0, 9);
            if (aresult.Length == 10 && aresult.Substring(0, 1) == "/")
                aresult = aresult.Substring(1, 9);
            if (aresult.Length == 9 && aresult.Substring(8, 1) == "/")
                aresult = aresult.Substring(0, 8);
            if (aresult.Length >= 9 && aresult.Substring(0, 1) == "/")
                aresult = aresult.Substring(1, 8);
            return aresult;
        }
    }
}
