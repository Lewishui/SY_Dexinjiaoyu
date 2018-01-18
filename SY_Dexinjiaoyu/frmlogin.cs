using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Order.Common;

namespace SY_Dexinjiaoyu
{
    public partial class frmlogin : Form
    {
        // 后台执行控件
        private BackgroundWorker bgWorker;
        // 消息显示窗体
        private frmMessageShow frmMessageShow;
        // 后台操作是否正常完成
        private bool blnBackGroundWorkIsOK = false;
        //后加的后台属性显
        private bool backGroundRunResult;
        string prc_folderpath;

        public log4net.ILog ProcessLogger;
        public log4net.ILog ExceptionLogger;
        private TextBox txtSAPPassword;
        private CheckBox chkSaveInfo;
        Sunisoft.IrisSkin.SkinEngine se = null;
        frmAboutBox aboutbox;
        //private frmMain frmMain;
        //存放要显示的信息
        List<string> messages;
        //要显示信息的下标索引
        int index = 0;
        private OrdersControl OrdersControl;
        public frmlogin()
        {
            InitializeComponent();
            aboutbox = new frmAboutBox();
            InitialSystemInfo();
            #region 引用皮肤
            //se = new Sunisoft.IrisSkin.SkinEngine();
            //se.SkinAllForm = true;
            //se.SkinFile = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""), "PageColor1.ssk"); 
            #endregion

            InitialPassword();
        }
        private void InitialSystemInfo()
        {
            #region 初始化配置
            ProcessLogger = log4net.LogManager.GetLogger("ProcessLogger");
            ExceptionLogger = log4net.LogManager.GetLogger("SystemExceptionLogger");
            ProcessLogger.Fatal("System Start " + DateTime.Now.ToString());
            #endregion
        }
        private void InitialPassword()
        {
            try
            {
                txtSAPPassword = new TextBox();
                txtSAPPassword.PasswordChar = '*';
                ToolStripControlHost t = new ToolStripControlHost(txtSAPPassword);
                t.Width = 100;
                t.AutoSize = false;
                t.Alignment = ToolStripItemAlignment.Right;
                this.toolStrip1.Items.Insert(this.toolStrip1.Items.Count - 4, t);

                chkSaveInfo = new CheckBox();
                chkSaveInfo.Text = "";
                chkSaveInfo.Padding = new Padding(5, 2, 0, 0);
                ToolStripControlHost t1 = new ToolStripControlHost(chkSaveInfo);
                t1.AutoSize = true;

                t1.ToolTipText = clsShowMessage.MSG_002;
                t1.Alignment = ToolStripItemAlignment.Right;
                this.toolStrip1.Items.Insert(this.toolStrip1.Items.Count - 5, t1);
                getUserAndPassword();
                chkSaveInfo.Checked = false;

            }
            catch (Exception ex)
            {
                //clsLogPrint.WriteLog("<frmMain> InitialPassword:" + ex.Message);
                throw ex;
            }
        }
        private void tsbLogin_Click(object sender, EventArgs e)
        {
            #region Noway
            DateTime oldDate = DateTime.Now;
            DateTime dt3;
            string endday = DateTime.Now.ToString("yyyy/MM/dd");
            dt3 = Convert.ToDateTime(endday);
            DateTime dt2;
            dt2 = Convert.ToDateTime("2018/08/25");

            TimeSpan ts = dt2 - dt3;
            int timeTotal = ts.Days;
            if (timeTotal < 0)
            {
                MessageBox.Show("六个月运行期已到，请将剩余费用付清 !");
                return;
            }
            // MessageBox.Show("当前为测试系统 !");

            #endregion


            //, 
            if (this.txtSAPUserId.Text == "Admin" || this.txtSAPUserId.Text == "Lewis")
            {
                if (this.txtSAPPassword.Text.Trim() == "123")
                {
                    this.WindowState = FormWindowState.Maximized;
                    tsbLogin.Text = "登录成功";
                    toolStripDropDownButton1.Enabled = true;
                    toolStripDropDownButton2.Enabled = true;
                    if (chkSaveInfo.Checked == true)
                        saveUserAndPassword();

                }
                else
                    tsbLogin.Text = "登录失败，密码错误";

            }
            else
            {
                tsbLogin.Text = "登录失败，用户名错误";

            }
        }
        private void getUserAndPassword()
        {
            try
            {
                RegistryKey rkLocalMachine = Registry.LocalMachine;
                RegistryKey rkSoftWare = rkLocalMachine.OpenSubKey(clsConstant.RegEdit_Key_SoftWare);
                RegistryKey rkAmdape2e = rkSoftWare.OpenSubKey(clsConstant.RegEdit_Key_AMDAPE2E);
                if (rkAmdape2e != null)
                {
                    this.txtSAPUserId.Text = clsCommHelp.encryptString(clsCommHelp.NullToString(rkAmdape2e.GetValue(clsConstant.RegEdit_Key_User)));
                    this.txtSAPPassword.Text = clsCommHelp.encryptString(clsCommHelp.NullToString(rkAmdape2e.GetValue(clsConstant.RegEdit_Key_PassWord)));
                    if (clsCommHelp.NullToString(rkAmdape2e.GetValue(clsConstant.RegEdit_Key_Date)) != "")
                    {
                        this.chkSaveInfo.Checked = true;
                    }
                    else
                    {
                        this.chkSaveInfo.Checked = false;
                    }
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
        private void saveUserAndPassword()
        {
            try
            {
                RegistryKey rkLocalMachine = Registry.LocalMachine;
                RegistryKey rkSoftWare = rkLocalMachine.OpenSubKey(clsConstant.RegEdit_Key_SoftWare, true);
                RegistryKey rkAmdape2e = rkSoftWare.CreateSubKey(clsConstant.RegEdit_Key_AMDAPE2E);
                if (rkAmdape2e != null)
                {
                    rkAmdape2e.SetValue(clsConstant.RegEdit_Key_User, clsCommHelp.encryptString(this.txtSAPUserId.Text.Trim()));
                    rkAmdape2e.SetValue(clsConstant.RegEdit_Key_PassWord, clsCommHelp.encryptString(this.txtSAPPassword.Text.Trim()));
                    rkAmdape2e.SetValue(clsConstant.RegEdit_Key_Date, DateTime.Now.ToString("yyyMMdd"));
                }
                rkAmdape2e.Close();
                rkSoftWare.Close();
                rkLocalMachine.Close();

            }
            catch (Exception ex)
            {
                //ClsLogPrint.WriteLog("<frmMain> saveUserAndPassword:" + ex.Message);
                throw ex;
            }
        }

        private void 关于系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutbox.ShowDialog();

        }
     

        private void pBBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //prc_folderpath = "";

            //OpenFileDialog tbox = new OpenFileDialog();
            //tbox.Multiselect = false;
            //tbox.Filter = "Excel Files(*.xls,*.xlsx,*.xlsm,*.xlsb)|*.xls;*.xlsx;*.xlsm;*.xlsb";
            //if (tbox.ShowDialog() == DialogResult.OK)
            //{
            //    prc_folderpath = tbox.FileName;
            //}
            //if (prc_folderpath == null || prc_folderpath == "")
            //    return;
            //if (MessageBox.Show("是否继续上传 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            //{
            //}
            //else
            //    return;


            try
            {
                var form = new ImportHotelCSV();
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
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

        private void 导入彩票数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = false;


            if (OrdersControl == null)
            {
                OrdersControl = new OrdersControl(this.txtSAPUserId.Text, this.txtSAPPassword.Text.Trim());
                OrdersControl.FormClosed += new FormClosedEventHandler(FrmOMS_FormClosed);
            }
            if (OrdersControl == null)
            {
                OrdersControl = new OrdersControl(this.txtSAPUserId.Text, this.txtSAPPassword.Text.Trim());
            }
            OrdersControl.Show(this.dockPanel2);
        }
        void FrmOMS_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is OrdersControl)
            {
                OrdersControl = null;
            }
        }

    }
}
