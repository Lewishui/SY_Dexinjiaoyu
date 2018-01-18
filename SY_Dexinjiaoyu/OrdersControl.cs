using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SY_Dexinjiaoyu
{
    public partial class OrdersControl : DockContent
    {
        private bool Is_AdminIS;


        public OrdersControl(string user, string password)
        {
            InitializeComponent();
          
            this.Disposed += new EventHandler(OrdersControl_Disposed);
            Is_AdminIS = true;

            if (Is_AdminIS == true)
            {
                crystalButton2.Enabled = true;

                crystalButton5.Enabled = true;
            }
            else
            {
                crystalButton2.Enabled = false;
                crystalButton5.Enabled = false;

            }
        }


        private void pendingButton_Click(object sender, EventArgs e)
        {

            //var form = new frmInfoCenter("");

            //if (form.ShowDialog() == DialogResult.OK)
            //{

            //}



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //var form = new frmLogCenter("");

            //if (form.ShowDialog() == DialogResult.OK)
            //{

            //}

        }

        private void shippedOrderButton_Click(object sender, EventArgs e)
        {
            //if (shippingOrderForm == null)
            //{
            //    shippingOrderForm = new ShippingOrderForm();
            //} 
            //AdjustSubformSize(shippingOrderForm);
            //shippingOrderForm.InitializeDataSource();
            //shippingOrderForm.ShowDialog();
            //this.Close();
            Application.Exit();

        }
        //  Application.Exit();
        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void OrdersControl_Load(object sender, EventArgs e)
        {
            //            Console.WriteLine(" orders control demension {0}, {1}", this.Width, this.Height);
            //            contentPanel.Left = (this.Width - contentPanel.Width) / 2;
            //            contentPanel.Top = (this.Height - contentPanel.Height) / 2;
            //            Console.WriteLine(" orders control demension {0}, {1}", contentPanel.Left, contentPanel.Top );
        }

        private void OrdersControl_Paint(object sender, PaintEventArgs e)
        {
            contentPanel.Left = (this.Width - contentPanel.Width) / 2;
            contentPanel.Top = (this.Height - contentPanel.Height) / 2;

        }


        private void receiveOrderButton_Click(object sender, EventArgs e)
        {

            var form = new frmOrder(Is_AdminIS);

            if (form.ShowDialog() == DialogResult.OK)
            {

            }


        }


        private void AdjustSubformSize(Form form)
        {
            var size = this.Parent.Size;
            size.Height = size.Height - 100;
            size.Width = size.Width - 50;
            form.Size = size;
        }

        private void OrdersControl_ControlRemoved(object sender, ControlEventArgs e)
        {
        }


        //Fix error 卸载 Appdomain 时出错
        void OrdersControl_Disposed(object sender, EventArgs e)
        {
            //this.pendingOrderForm.Dispose();
            //this.waitToShipOrderForm.Dispose();
            //this.shippingOrderForm.Dispose();
        }

        private void newButton_Click(object sender, EventArgs e)
        {

            //var form = new frmOrderMain("");

            //if (form.ShowDialog() == DialogResult.OK)
            //{

            //}



        }

        private void button6_Click(object sender, EventArgs e)
        {
            //if (OrderHistoryForm == null)
            //{
            //    OrderHistoryForm = new OrderHistoryForm();
            //}
            //AdjustSubformSize(OrderHistoryForm);
            //// 显示之前重新加载数据，订单数据可能已更新。
            //// OrderHistoryForm.InitializeOrderData();
            //OrderHistoryForm.pager1.Bind();
            //OrderHistoryForm.ShowDialog();

        }

        private void orderConfirmButton_Click(object sender, EventArgs e)
        {
            //new ConnectServerForReceivedOrderForm().ShowDialog();
            Is_AdminIS = true;

            //var form = new frmOrder(Is_AdminIS);

            //if (form.ShowDialog() == DialogResult.OK)
            //{

            //}



        }
    }
}
