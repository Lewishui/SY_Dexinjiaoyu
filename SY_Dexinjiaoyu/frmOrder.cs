using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DCTS.CustomComponents;
using Dexin.Buiness;
using Order.Common;
using Order.DB;

namespace SY_Dexinjiaoyu
{
    public partial class frmOrder : Form
    {
        // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        DateTime startAt;
        DateTime endAt;
        List<clsDATAinfo> Orderinfolist_Server;
        int rowcount;
        string txfind;
        private SortableBindingList<clsDATAinfo> sortableOrderList;
        List<int> changeindex;
        List<clsDATAinfo> deletedorderList;
        string prc_folderpath;

        private Hashtable dataGridChanges = null;
        private bool is_AdminIS;
        bool changedtp;

        public frmOrder(bool Is_AdminIS)
        {
            InitializeComponent();
            is_AdminIS = Is_AdminIS;

            this.dataGridChanges = new Hashtable();
            changeindex = new List<int>();
            Orderinfolist_Server = new List<clsDATAinfo>();
            deletedorderList = new List<clsDATAinfo>();
            this.BindDataGridView();
            changedtp = false;
            this.WindowState = FormWindowState.Maximized;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //var form = new frmAddOrder("");

            //if (form.ShowDialog() == DialogResult.OK)
            //{

            //}
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            this.dataGridChanges = new Hashtable();

            this.pbStatus.Value = 0;
            this.toolStripLabel1.Text = "";

            startAt = this.stockOutDateTimePicker.Value.AddDays(0).Date;
            endAt = this.stockInDateTimePicker1.Value.AddDays(0).Date;
            txfind = this.textBox8.Text;

            string strSelect = "select * from PeopleData where Input_Date>='" + startAt.ToString("yyyy/MM/dd") + "'" + "and " + "Input_Date<='" + endAt.ToString("yyyy/MM/dd") + "'";

            #region 判断汉字或字母
            int istrue = 0;

            bool ischina = clsCommHelp.HasChineseTest(txfind.ToString());
            if (ischina == false && txfind != "")
            {
                istrue = 1;
            }
            if (Regex.Matches(txfind.ToString(), "[a-zA-Z]").Count > 0 && !txfind.ToString().Contains("/"))
            {
                istrue = 1;

            }
            #endregion

            if (txfind.Length > 0 && istrue == 1)
            {
                strSelect += " And zhenghao like '%" + txfind + "%'";
                if (txfind == "所有")
                    strSelect = "select * from PeopleData";
            }

            if (txfind.Length > 0 && istrue == 0)
            {
                strSelect += " And xingming like '%" + txfind + "%'";
                if (txfind == "所有")
                    strSelect = "select * from PeopleData";

            }


            strSelect += " order by Item_ID asc";

            clsAllnew BusinessHelp = new clsAllnew();
            Orderinfolist_Server = new List<clsDATAinfo>();
            Orderinfolist_Server = BusinessHelp.findOrder(strSelect);

            this.BindDataGridView();
        }
        private void BindDataGridView()
        {
            if (Orderinfolist_Server != null)
            {

                sortableOrderList = new SortableBindingList<clsDATAinfo>(Orderinfolist_Server);
                bindingSource1.DataSource = new SortableBindingList<clsDATAinfo>(Orderinfolist_Server);
                dataGridView1.AutoGenerateColumns = false;

                dataGridView1.DataSource = bindingSource1;
                this.toolStripLabel1.Text = "条数：" + sortableOrderList.Count.ToString();
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" 确认删除这条信息 , 继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

            }
            else
                return;

            var oids = GetOrderIdsBySelectedGridCell();
            for (int j = 0; j < oids.Count; j++)
            {
                var filtered = Orderinfolist_Server.FindAll(s => s.Item_ID == oids[j]);
                clsAllnew BusinessHelp = new clsAllnew();
                //批量删 
                int istu = BusinessHelp.deleteitem(filtered[0].Item_ID.ToString());

                for (int i = 0; i < filtered.Count; i++)
                {
                    //单个删除
                    if (filtered[i].Item_ID != 0)
                    {
                        Orderinfolist_Server.Remove(Orderinfolist_Server.Where(o => o.Item_ID == filtered[i].Item_ID).Single());
                    }
                    if (istu != 1)
                    {
                        MessageBox.Show("删除失败，请查看" + filtered[i].zhenghao + filtered[i].xingming, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            BindDataGridView();
        }
        private List<long> GetOrderIdsBySelectedGridCell()
        {

            List<long> Item_IDs = new List<long>();
            var rows = GetSelectedRowsBySelectedCells(dataGridView1);
            foreach (DataGridViewRow row in rows)
            {
                var Diningorder = row.DataBoundItem as clsDATAinfo;
                Item_IDs.Add((long)Diningorder.Item_ID);
            }

            return Item_IDs;
        }
        private IEnumerable<DataGridViewRow> GetSelectedRowsBySelectedCells(DataGridView dgv)
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                rows.Add(cell.OwningRow);

            }
            rowcount = dgv.SelectedCells.Count;

            return rows.Distinct();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            clsDATAinfo item = new clsDATAinfo();
            item.Input_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
            item.xinzeng = "true";

            this.bindingSource1.Add(item);
            this.dataGridView1.Refresh();
        }

        private void delScheduleButton_Click(object sender, EventArgs e)
        {
            var schedule = GetSelectedSchedule();

            if (schedule != null)
            {
                deletedorderList.Add(schedule);
                bindingSource1.Remove(schedule);
                this.dataGridView1.Refresh();
            }
        }
        private clsDATAinfo GetSelectedSchedule()
        {
            clsDATAinfo schedule = null;
            var row = this.dataGridView1.CurrentRow;
            if (row != null)
            {
                schedule = row.DataBoundItem as clsDATAinfo;
            }
            return schedule;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int s = this.tabControl1.SelectedIndex;
                if (s == 0)
                {
                    dataGridView1.Enabled = false;
                    if (changeindex.Count < 1)
                    {
                        IEnumerable<int> orderIds = GetChangedOrderIds();
                        foreach (var id in orderIds.Distinct())
                        {
                            changeindex.Add(id);
                        }
                    }
                }

                if (backgroundWorker2.IsBusy != true)
                {
                    backgroundWorker2.RunWorkerAsync(new WorkerArgument { OrderCount = 0, CurrentIndex = 0 });

                }
                dataGridChanges.Clear();

            }
            catch (Exception ex)
            {
                dataGridChanges.Clear();
                return;
                throw;
            }
        }

        private IEnumerable<int> GetChangedOrderIds()
        {

            List<int> rows = new List<int>();
            foreach (DictionaryEntry entry in dataGridChanges)
            {
                var key = entry.Key as string;
                if (key.EndsWith("_changed"))
                {
                    int row = Int32.Parse(key.Split('_')[0]);
                    rows.Add(row);
                }
            }
            return rows.Distinct();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow dgrSingle = dataGridView1.Rows[e.RowIndex];
            string cell_key = e.RowIndex.ToString() + "_" + e.ColumnIndex.ToString();

            if (!dataGridChanges.ContainsKey(cell_key))
            {
                dataGridChanges[cell_key] = dgrSingle.Cells[e.ColumnIndex].Value;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string cell_key = e.RowIndex.ToString() + "_" + e.ColumnIndex.ToString() + "_changed";

            if (dataGridChanges.ContainsKey(cell_key))
            {
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.SelectionBackColor = Color.DarkRed;

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string cell_key = e.RowIndex.ToString() + "_" + e.ColumnIndex.ToString();
            var new_cell_value = row.Cells[e.ColumnIndex].Value;
            var original_cell_value = dataGridChanges[cell_key];
            if (new_cell_value == null && original_cell_value == null)
            {
                dataGridChanges.Remove(cell_key + "_changed");
            }
            else if ((new_cell_value == null && original_cell_value != null) || (new_cell_value != null && original_cell_value == null) || !new_cell_value.Equals(original_cell_value))
            {
                dataGridChanges[cell_key + "_changed"] = new_cell_value;
            }
            else
            {
                dataGridChanges.Remove(cell_key + "_changed");
            }
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            bool success = dailysaveList(worker, e);
        }
        private bool dailysaveList(BackgroundWorker worker, DoWorkEventArgs e)
        {
            WorkerArgument arg = e.Argument as WorkerArgument;
            clsAllnew BusinessHelp = new clsAllnew();
            bool success = true;
            try
            {

                int rowCount = changeindex.Count;
                arg.OrderCount = rowCount;
                int j = 1;
                int progress = 0;
                #region MyRegion
                for (int ik = 0; ik < changeindex.Count; ik++)
                {
                    j = ik;

                    arg.CurrentIndex = j + 1;
                    progress = Convert.ToInt16(((j + 1) * 1.0 / rowCount) * 100);

                    int i = changeindex[ik];
                    var row = dataGridView1.Rows[i];

                    var model = row.DataBoundItem as clsDATAinfo;

                    clsDATAinfo item = new clsDATAinfo();

                    item.zhenghao = Convert.ToString(dataGridView1.Rows[i].Cells["zhenghao"].EditedFormattedValue.ToString());

                    item.xingming = Convert.ToString(dataGridView1.Rows[i].Cells["xingming"].EditedFormattedValue.ToString());

                    item.xingbie = Convert.ToString(dataGridView1.Rows[i].Cells["xingbie"].EditedFormattedValue.ToString());

                    item.zuoyeleibie = Convert.ToString(dataGridView1.Rows[i].Cells["zuoyeleibie"].EditedFormattedValue.ToString());

                    item.zhuncaoxiangmu = Convert.ToString(dataGridView1.Rows[i].Cells["zhuncaoxiangmu"].EditedFormattedValue.ToString());

                    item.chulingriqi = Convert.ToString(dataGridView1.Rows[i].Cells["chulingriqi"].EditedFormattedValue.ToString());

                    item.youxiangqixian = Convert.ToString(dataGridView1.Rows[i].Cells["youxiangqixian"].EditedFormattedValue.ToString());

                    item.fushenriqi = Convert.ToString(dataGridView1.Rows[i].Cells["fushenriqi"].EditedFormattedValue.ToString());


                    item.xinzeng = Convert.ToString(dataGridView1.Rows[i].Cells["xinzeng"].EditedFormattedValue.ToString());
                    // item.Message = Convert.ToString(dataGridView1.Rows[i].Cells["Message"].EditedFormattedValue.ToString());

                    item.Input_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
                    item.Item_ID = model.Item_ID;

                #endregion

                    #region MyRegion
                    var startAt = this.stockOutDateTimePicker.Value.AddDays(0).Date;
                    #region  构造查询条件
                    string conditions = "";
                    conditions = BusinessHelp.sql_yuju(e, item, conditions);
                    #endregion
                    #endregion

                    int isrun = BusinessHelp.update_Server(conditions, item);
                    if (item.xinzeng == "true" && isrun == 1)
                        item.xinzeng = "";

                    if (arg.CurrentIndex % 5 == 0)
                    {
                        backgroundWorker2.ReportProgress(progress, arg);
                    }
                }
                backgroundWorker2.ReportProgress(100, arg);
                e.Result = string.Format("{0} 已保存 ！", changeindex.Count);

            }
            catch (Exception ex)
            {
                if (!e.Cancel)
                {

                    e.Result = ex.Message + "";
                }
                success = false;
            }

            return success;
        }


        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show(string.Format("It is cancelled!"));
            }
            else
            {
                if (!e.Result.ToString().Contains("Dear"))
                {
                    toolStripLabel1.Text = "" + "(" + e.Result + ")";
                }
                else
                    toolStripLabel1.Text = e.Result.ToString();

                changeindex = new List<int>();

                dataGridView1.Enabled = true;
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WorkerArgument arg = e.UserState as WorkerArgument;
            if (!arg.HasError)
            {
                this.toolStripLabel1.Text = String.Format("{0}/{1}", arg.CurrentIndex, arg.OrderCount);
                this.ProgressValue = e.ProgressPercentage;
            }
            else
            {
                this.toolStripLabel1.Text = arg.ErrorMessage;
            }

        }
        public int ProgressValue
        {
            get { return this.pbStatus.Value; }
            set { pbStatus.Value = value; }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Sorry , No Data Output !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "csv|*.csv";
            string strFileName = " 德信教育" + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            saveFileDialog.FileName = strFileName;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                strFileName = saveFileDialog.FileName.ToString();
            }
            else
            {
                return;
            }
            FileStream fa = new FileStream(strFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fa, Encoding.Unicode);
            string delimiter = "\t";
            string strHeader = "";
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                strHeader += this.dataGridView1.Columns[i].HeaderText + delimiter;
            }
            sw.WriteLine(strHeader);

            //output rows data
            for (int j = 0; j < this.dataGridView1.Rows.Count; j++)
            {
                string strRowValue = "";

                for (int k = 0; k < this.dataGridView1.Columns.Count; k++)
                {
                    if (this.dataGridView1.Rows[j].Cells[k].Value != null)
                    {
                        strRowValue += this.dataGridView1.Rows[j].Cells[k].Value.ToString().Replace("\r\n", " ").Replace("\n", "") + delimiter;
                        if (this.dataGridView1.Rows[j].Cells[k].Value.ToString() == "LIP201507-35")
                        {

                        }

                    }
                    else
                    {
                        strRowValue += this.dataGridView1.Rows[j].Cells[k].Value + delimiter;
                    }
                }
                sw.WriteLine(strRowValue);
            }
            sw.Close();
            fa.Close();
            MessageBox.Show("Dear User, Down File  Successful ！", "System", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                blnBackGroundWorkIsOK = false;
            }
            else if (e.Cancelled)
            {
                blnBackGroundWorkIsOK = true;
            }
            else
            {
                blnBackGroundWorkIsOK = true;
            }
        }
        private void InitialBackGroundWorker()
        {
            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged +=
                new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (frmMessageShow != null && frmMessageShow.Visible == true)
            {
                //设置显示的消息
                frmMessageShow.setMessage(e.UserState.ToString());
                //设置显示的按钮文字
                if (e.ProgressPercentage == clsConstant.Thread_Progress_OK)
                {
                    frmMessageShow.setStatus(clsConstant.Dialog_Status_Enable);
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            prc_folderpath = "";

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹路径";


            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.toolStripLabel1.Text = dialog.SelectedPath;
                prc_folderpath = dialog.SelectedPath; ;

            }
            if (prc_folderpath == null || prc_folderpath == "")
                return;
            //if (MessageBox.Show("是否继续上传 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //{
            //}
            //else
            //    return;


            try
            {
                #region 获取信息
                Orderinfolist_Server = new List<clsDATAinfo>();

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if ((bool)dataGridView1.Rows[i].Cells[1].EditedFormattedValue == true)
                    {
                        clsDATAinfo item = new clsDATAinfo();
                        item.zhenghao = Convert.ToString(dataGridView1.Rows[i].Cells["zhenghao"].EditedFormattedValue.ToString());

                        item.xingming = Convert.ToString(dataGridView1.Rows[i].Cells["xingming"].EditedFormattedValue.ToString());

                        item.xingbie = Convert.ToString(dataGridView1.Rows[i].Cells["xingbie"].EditedFormattedValue.ToString());

                        item.zuoyeleibie = Convert.ToString(dataGridView1.Rows[i].Cells["zuoyeleibie"].EditedFormattedValue.ToString());

                        item.zhuncaoxiangmu = Convert.ToString(dataGridView1.Rows[i].Cells["zhuncaoxiangmu"].EditedFormattedValue.ToString());

                        item.chulingriqi = Convert.ToString(dataGridView1.Rows[i].Cells["chulingriqi"].EditedFormattedValue.ToString());

                        item.youxiangqixian = Convert.ToString(dataGridView1.Rows[i].Cells["youxiangqixian"].EditedFormattedValue.ToString());

                        item.fushenriqi = Convert.ToString(dataGridView1.Rows[i].Cells["fushenriqi"].EditedFormattedValue.ToString());
                        Orderinfolist_Server.Add(item);

                    }
                }
                if (Orderinfolist_Server == null || Orderinfolist_Server.Count == 0)
                {
                    MessageBox.Show("请选择要制证的信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                
                }
                #endregion
                InitialBackGroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(CheckCaseList);

                bgWorker.RunWorkerAsync();
                // 启动消息显示画面
                frmMessageShow = new frmMessageShow(clsShowMessage.MSG_001,
                                                    clsShowMessage.MSG_007,
                                                    clsConstant.Dialog_Status_Disable);
                frmMessageShow.ShowDialog();
                // 数据读取成功后在画面显示
                if (blnBackGroundWorkIsOK)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("12320" + ex);
                return;

                throw ex;
            }
        }
        private void CheckCaseList(object sender, DoWorkEventArgs e)
        {
            try
            {
             
                //初始化信息
                clsAllnew BusinessHelp = new clsAllnew();

                DateTime oldDate = DateTime.Now;
                foreach (clsDATAinfo item in Orderinfolist_Server)
                {
                  BusinessHelp.ReplaceToExcel(ref this.bgWorker, item, prc_folderpath);

                    BusinessHelp.Run(item, prc_folderpath);

                }
                DateTime FinishTime = DateTime.Now;
                TimeSpan s = DateTime.Now - oldDate;
                string timei = s.Minutes.ToString() + ":" + s.Seconds.ToString();
                string Showtime = clsShowMessage.MSG_029 + timei.ToString();
                bgWorker.ReportProgress(clsConstant.Thread_Progress_OK, clsShowMessage.MSG_031 + "\r\n" + Showtime);
            }
            catch (Exception ex)
            {

                MessageBox.Show("12320" + ex);
                return;

                throw;
            }
        }
     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];

            if (column == xingbie)
            {

            }
        }


    }
}
