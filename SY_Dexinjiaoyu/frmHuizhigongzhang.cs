using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SY_Dexinjiaoyu
{
    public partial class frmHuizhigongzhang : Form
    {
        string inputWords;
        Rectangle rect;
        Font Var_Font = new System.Drawing.Font("Microsoft Sans Serif", 17, System.Drawing.FontStyle.Bold);
         public frmHuizhigongzhang()
        {
            InitializeComponent();
    
        }
        #region 绘制公章
        private void simpleButton_绘制公章_Click(object sender, EventArgs e)
         {
             inputWords = textBox_文字.Text;
            int tem_Line = 0;  //圆的直径
            int circularity_W = 5; //画笔的粗细
            string star_Str = "★";  //星星
            Font star_Font = new Font("Arial", 30, FontStyle.Regular);//设置星号的字体样式

            #region 画圆

            if (panel_绘制公章.Height > panel_绘制公章.Width)  //如果panel控件的高度大于等于宽度
            {
                tem_Line = panel_绘制公章.Width;  //设置宽度为圆的直径
            }
            else
            {
                tem_Line = panel_绘制公章.Height;  //设置高度为圆的直径
            }
            //设置圆的绘制区域=>现在是正方形的区域
            rect = new Rectangle(circularity_W, circularity_W, tem_Line - 2 * circularity_W, tem_Line - 2 * circularity_W);

            //补充：Graphics必须有载体，也就是在哪里绘
            //所以必须是this.CreateGraphics或者Panel..CreateGraphics等格式
            Graphics g = panel_绘制公章.CreateGraphics();//实例化Graphics类
            //消除绘制图形的锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;  //System.Drawing.Drawing2D;           
            g.Clear(Color.White);  //以白色清空panel1控件的背景，防止重复画           
            Pen myPen = new Pen(Color.Red, circularity_W);  //设置画笔（颜色和粗细）
            g.DrawEllipse(myPen, rect);  //绘制圆

            #endregion

            #region 画星星

            SizeF Var_Size = new SizeF(rect.Width, rect.Height);  //实例化SizeF类
            Var_Size = g.MeasureString(star_Str, star_Font);  //对指定字符串进行测量

            //正中间的位置绘制星号
            float star_x = (rect.Width / 2F) + circularity_W - Var_Size.Width / 2F;
            float star_y = rect.Height / 2F - Var_Size.Width / 2F;
            g.DrawString(star_Str, star_Font, myPen.Brush, new PointF(star_x, star_y));

            #endregion

            #region 画文字

            Var_Size = g.MeasureString("本人专用章", Var_Font);//对指定字符串进行测量

            //绘制文字：在中间，但是在星星下面
            float m = (rect.Width / 2F) + circularity_W - Var_Size.Width / 2F;
            float n = rect.Height / 2F + Var_Size.Height * 2;
            g.DrawString("本人专用章", Var_Font, myPen.Brush, new PointF(m, n));

            int len = 0;
            if (inputWords != null) //如果没有输入文字，加判断
            {
                len = inputWords.Length;//获取字符串的长度
            }

            float angle = 180;//设置文字的初始旋转角度

            float change = 0;

            if (len > 1) //一个字的需要特殊处理
            {
                change = 180 / (len - 1);
            }
            for (int i = 0; i < len; i++)//将文字以指定的弧度进行绘制
            {
                if (len > 1)
                {
                    //相当于把坐标系移动到了正中间
                    float x = (tem_Line + circularity_W / 2) / 2;
                    float y = (tem_Line + circularity_W / 2) / 2;
                    //将指定的平移添加到g的变换矩阵前         
                    g.TranslateTransform(x, y);
                    g.RotateTransform(angle);//将指定的旋转用于g的变换矩阵   
                    Brush myBrush = Brushes.Red;//定义画刷

                    //需要注意，这时文字的位置的坐标位置是以新的坐标系为基础得到的
                    float words_x = tem_Line / 2 - 6 * circularity_W;
                    float words_y = 0;
                    g.DrawString(inputWords.Substring(i, 1), Var_Font, myBrush, words_x, words_y);//显示旋转文字
                    g.ResetTransform();//将g的全局变换矩阵重置为单位矩阵=>对应TranslateTransform，相当于恢复操作
                    angle += change;//设置下一个文字的角度

                }
                else
                {
                    //输入的文字为一个时候是特殊情况，单独考虑
                    float x = (tem_Line + circularity_W / 2) / 2;
                    float y = (tem_Line + circularity_W / 2) / 2;
                    g.TranslateTransform(x, y);
                    g.RotateTransform(0);
                    Brush myBrush = Brushes.Red;
                    float words_x = -circularity_W * 3;
                    float words_y = -(tem_Line / 2 - 2 * circularity_W);
                    g.DrawString(inputWords.Substring(i, 1), Var_Font, myBrush, words_x, words_y);
                    g.ResetTransform();
                }

            }

            #endregion

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            inputWords = textBox_文字.Text;
            MessageBox.Show("保存成功！");
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
           
            string filename = @"C:\Users\IBM_ADMIN\Desktop\QQ截图20180121224116.jpg";

            Image img = Image.FromFile(filename);

            // Assumes myImage is the PNG you are converting

            using (var b = new Bitmap(img.Width, img.Height))
            {
                b.SetResolution(img.HorizontalResolution, img.VerticalResolution);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(img, 0, 0);
                }

                // Now save b as a JPEG like you normally would

                b.Save(@"C:\Users\IBM_ADMIN\Desktop\2.gif", System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            Image image;
            image = Image.FromFile(@"C:\Users\IBM_ADMIN\Desktop\2.jpg");
            Bitmap bitmap = new Bitmap(image);
            bitmap.MakeTransparent(Color.White);
            bitmap.Save(@"C:\Users\IBM_ADMIN\Desktop\3.jpg");


        }


    }
}
