using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Order.DB
{
    public class clsuserinfo
    {
        public string Order_id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string Btype { get; set; }
        public string denglushijian { get; set; }
        public string Createdate { get; set; }
        public string AdminIS { get; set; }
        public string jigoudaima { get; set; }
    }

    //证号	姓名	性别	作业类别	准操项目	初领日期	有效期限	复审日期

    public class clsDATAinfo
    {
        public int Item_ID { get; set; }
        public string zhenghao { get; set; }
        public string xingming { get; set; }
        public string xingbie { get; set; }
        public string zuoyeleibie { get; set; }
        public string zhuncaoxiangmu { get; set; }
        public string chulingriqi { get; set; }
        public string youxiangqixian { get; set; }
        public string fushenriqi { get; set; }     
        public DateTime Input_Date { get; set; }    
        public string Message { get; set; }

        //新增的标记
        public string xinzeng { get; set; }
     

      
    }
    //产品型号	订货数量	订货日期	使用单位

    public class clsLog_info
    {
        public int Log_id { get; set; }
        public string product_no { get; set; }
        public string indent { get; set; }
        public string indent_date { get; set; }
        public string end_user { get; set; }
        public string vendor { get; set; }
        public string daohuoshijian { get; set; }
   
        public DateTime Input_Date { get; set; }
        //新增的标记
        public string xinzeng { get; set; }
   

    }

    public class clsR2accrualsapinfo
    {
        public string Message { get; set; }
        public int Id { get; set; }
        public string Input_Date { get; set; }
        public string yiqingxiangmu { get; set; }
        public string jizhangriqi { get; set; }
        public string pingzhengriqi { get; set; }
        public string nashuidanwei { get; set; }
        public string lirunzhongxin { get; set; }
        public string benbijine { get; set; }
        public string wenben { get; set; }
        public string pingzhengleixing { get; set; }
        public string jizhangdaima { get; set; }
        public string pingzhenghaoma { get; set; }
        public string benbi { get; set; }
        public string qingzhangpingzheng { get; set; }
        public string glkemu { get; set; }
        public string nianduyuefen { get; set; }
        public string yonghumingcheng { get; set; }
        public string congxiao { get; set; }
        //
        public string leiming { get; set; }

        public string xuhao { get; set; }
        public string gaiyao { get; set; }
        //gongsidaima
        public string gongsidaima { get; set; }
        //
        //faren
        public string faren { get; set; }
        //选择要分析的日期
        public string selecttime { get; set; }
        // 标记已计算过的KA 
        public string is_mergeKA { get; set; }
    }
  
}
