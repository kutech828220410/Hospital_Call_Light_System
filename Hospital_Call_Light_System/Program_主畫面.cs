using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using MyUI;
using Basic;
using SQLUI;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

using System.Reflection;
using System.Runtime.InteropServices;

namespace Hospital_Call_Light_System
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        int 叫號台01_號碼 = -1;
        int 叫號台02_號碼 = -1;

        private void Program_主畫面()
        {
            this.button_第一台號碼輸入.Click += Button_第一台號碼輸入_Click;
            this.button_第二台號碼輸入.Click += Button_第二台號碼輸入_Click;
            this.plC_RJ_Button_刷新螢幕.MouseDownEvent += PlC_RJ_Button_刷新螢幕_MouseDownEvent;
            this.button_主畫面_存檔.Click += Button_主畫面_存檔_Click;
            this.plC_UI_Init.Add_Method(sub_Program_主畫面);
        }

        private void sub_Program_主畫面()
        {
            sub_Program_刷新螢幕();
        }

        private Bitmap Function_主畫面_取得文字Bitmap(string 標題名稱, Font 標題字體, Size 標題大小, int 標題文字寬度, Color 標題字體顏色, Color 標題背景顏色)
        {
            try
            {
                Bitmap bitmap = new Bitmap(標題大小.Width, 標題大小.Height);
                using (Graphics g_bmp = Graphics.FromImage(bitmap))
                {
                    DrawingClass.Draw.方框繪製(new PointF(0, 0), bitmap.Size, 標題背景顏色, 1, true, g_bmp, 1, 1);
                    Size size_font = TextRenderer.MeasureText(標題名稱, 標題字體);
                    int x = (標題大小.Width - 標題文字寬度) / 2;
                    int y = (bitmap.Height - size_font.Height) / 2;

                    DrawingClass.Draw.文字左上繪製(標題名稱, 標題文字寬度, new PointF(x, y), 標題字體, 標題字體顏色, 標題背景顏色, g_bmp);
                }
                return bitmap;
            }
            catch
            {
                return null;
            }
       
        }

        #region Event
        private void Button_主畫面_存檔_Click(object sender, EventArgs e)
        {
            myConfigClass.機台代碼 = this.textBox_機台代碼.Text;

            string jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(myConfigClass, true);
            List<string> list_jsonstring = new List<string>();
            list_jsonstring.Add(jsonstr);
            if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
            {
                MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                return;
            }
            MyMessageBox.ShowDialog("完成!");

        }
        private void PlC_RJ_Button_刷新螢幕_MouseDownEvent(MouseEventArgs mevent)
        {
            try
            {
                string 機台代碼 = this.textBox_機台代碼.Text;
                using (Graphics g = this.panel_叫號.CreateGraphics())
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    int width = this.panel_叫號.Width;
                    int height = this.panel_叫號.Height;
                    Bitmap bitmap_標題_0 = null;
                    Bitmap bitmap_叫號_0 = null;
                    Bitmap bitmap_標題_1 = null;
                    Bitmap bitmap_叫號_1 = null;
                    List<object[]> list_value = this.sqL_DataGridView_叫號台設定.SQL_GetAllRows(false);
                    list_value = list_value.GetRows((int)enum_叫號台設定.代碼, 機台代碼);
                    list_value.Sort(new Icp_叫號台設定());
                    bool flag_RING = false;
                    for (int i = 0; i < list_value.Count; i++)
                    {
                        int 寬度 = (width - 0) / 2;
                        string 標題名稱 = list_value[i][(int)enum_叫號台設定.標題名稱].ObjectToString();
                        Font 標題字體 = list_value[i][(int)enum_叫號台設定.標題字體].ObjectToString().ToFont();
                        int 標題文字寬度 = list_value[i][(int)enum_叫號台設定.標題文字寬度].StringToInt32();
                        Color 標題字體顏色 = list_value[i][(int)enum_叫號台設定.標題字體顏色].ObjectToString().ToColor();
                        Color 標題背景顏色 = list_value[i][(int)enum_叫號台設定.標題背景顏色].ObjectToString().ToColor();
                        int 標題高度 = list_value[i][(int)enum_叫號台設定.標題高度].StringToInt32();

                        string 叫號名稱 = list_value[i][(int)enum_叫號台設定.叫號號碼].ObjectToString();
                        Font 叫號字體 = list_value[i][(int)enum_叫號台設定.叫號字體].ObjectToString().ToFont();
                        int 叫號文字寬度 = list_value[i][(int)enum_叫號台設定.叫號文字寬度].StringToInt32();
                        Color 叫號字體顏色 = list_value[i][(int)enum_叫號台設定.叫號字體顏色].ObjectToString().ToColor();
                        Color 叫號背景顏色 = list_value[i][(int)enum_叫號台設定.叫號背景顏色].ObjectToString().ToColor();
                        int 叫號高度 = height - 標題高度;
                        if (i == 0)
                        {
                            if (叫號台01_號碼 != list_value[i][(int)enum_叫號台設定.叫號號碼].StringToInt32())
                            {
                                flag_RING = true;
                            }
                            叫號台01_號碼 = list_value[i][(int)enum_叫號台設定.叫號號碼].StringToInt32();
                            bitmap_標題_0 = Function_主畫面_取得文字Bitmap(標題名稱, 標題字體, new Size(寬度, 標題高度), 標題文字寬度, 標題字體顏色, 標題背景顏色);
                            bitmap_叫號_0 = Function_主畫面_取得文字Bitmap(叫號名稱, 叫號字體, new Size(寬度, 叫號高度), 叫號文字寬度, 叫號字體顏色, 叫號背景顏色);
                        }
                        if (i == 1)
                        {
                            if (叫號台02_號碼 != list_value[i][(int)enum_叫號台設定.叫號號碼].StringToInt32())
                            {
                                flag_RING = true;
                            }

                            叫號台02_號碼 = list_value[i][(int)enum_叫號台設定.叫號號碼].StringToInt32();
                            bitmap_標題_1 = Function_主畫面_取得文字Bitmap(標題名稱, 標題字體, new Size(寬度, 標題高度), 標題文字寬度, 標題字體顏色, 標題背景顏色);
                            bitmap_叫號_1 = Function_主畫面_取得文字Bitmap(叫號名稱, 叫號字體, new Size(寬度, 叫號高度), 叫號文字寬度, 叫號字體顏色, 叫號背景顏色);
                        }



                    }
                    int tota_width = 0;
                    if (bitmap_標題_0 != null) tota_width += bitmap_標題_0.Width;
                    if (bitmap_標題_1 != null) tota_width += bitmap_標題_1.Width;
                    int posx = (width - tota_width) / 2;
                    if (bitmap_標題_0 != null)
                    {
                        g.DrawImage(bitmap_標題_0, new PointF(posx, 0));
                        g.DrawImage(bitmap_叫號_0, new PointF(posx, bitmap_標題_0.Height));
                    }
                    if (bitmap_標題_1 != null)
                    {
                        g.DrawImage(bitmap_標題_1, new PointF(posx + bitmap_標題_0.Width, 0));
                        g.DrawImage(bitmap_叫號_1, new PointF(posx + bitmap_標題_0.Width, bitmap_標題_1.Height));
                    }
                    if(flag_RING)
                    {
                        System.Media.SoundPlayer sp = null;
                        try
                        {
                            sp = new System.Media.SoundPlayer(".//RING.wav");
                            sp.Stop();

                            sp.Play();
                        }
                        finally
                        {
                            if (sp != null) sp.Dispose();
                        }
                    }
          

                }
            }
            catch
            {

            }
          
        }
        private void Button_第一台號碼輸入_Click(object sender, EventArgs e)
        {
            string 機台代碼 = this.textBox_機台代碼.Text;
            List<object[]> list_value = this.sqL_DataGridView_叫號台設定.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_叫號台設定.代碼, 機台代碼);
            list_value = list_value.GetRows((int)enum_叫號台設定.台號, "1");
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("找無資料!");
                return;
            }
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            list_value[0][(int)enum_叫號台設定.叫號號碼] = dialog_NumPannel.Value.ToString("0000");
            this.sqL_DataGridView_叫號台設定.SQL_ReplaceExtra(list_value, false);
        }
        private void Button_第二台號碼輸入_Click(object sender, EventArgs e)
        {
            string 機台代碼 = this.textBox_機台代碼.Text;
            List<object[]> list_value = this.sqL_DataGridView_叫號台設定.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_叫號台設定.代碼, 機台代碼);
            list_value = list_value.GetRows((int)enum_叫號台設定.台號, "2");
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("找無資料!");
                return;
            }
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            list_value[0][(int)enum_叫號台設定.叫號號碼] = dialog_NumPannel.Value.ToString("0000");
            this.sqL_DataGridView_叫號台設定.SQL_ReplaceExtra(list_value, false);
        }
        #endregion

        #region PLC_刷新螢幕
        PLC_Device PLC_Device_刷新螢幕 = new PLC_Device("");
        PLC_Device PLC_Device_刷新螢幕_OK = new PLC_Device("");
        Task Task_刷新螢幕;
        MyTimer MyTimer_刷新螢幕_結束延遲 = new MyTimer();
        int cnt_Program_刷新螢幕 = 65534;
        void sub_Program_刷新螢幕()
        {
            if (plC_ScreenPage_Main.PageText == "主畫面") PLC_Device_刷新螢幕.Bool = true;
            if (cnt_Program_刷新螢幕 == 65534)
            {
                this.MyTimer_刷新螢幕_結束延遲.StartTickTime(200);
                PLC_Device_刷新螢幕.SetComment("PLC_刷新螢幕");
                PLC_Device_刷新螢幕_OK.SetComment("PLC_刷新螢幕_OK");
                PLC_Device_刷新螢幕.Bool = false;
                cnt_Program_刷新螢幕 = 65535;
            }
            if (cnt_Program_刷新螢幕 == 65535) cnt_Program_刷新螢幕 = 1;
            if (cnt_Program_刷新螢幕 == 1) cnt_Program_刷新螢幕_檢查按下(ref cnt_Program_刷新螢幕);
            if (cnt_Program_刷新螢幕 == 2) cnt_Program_刷新螢幕_初始化(ref cnt_Program_刷新螢幕);
            if (cnt_Program_刷新螢幕 == 3) cnt_Program_刷新螢幕 = 65500;
            if (cnt_Program_刷新螢幕 > 1) cnt_Program_刷新螢幕_檢查放開(ref cnt_Program_刷新螢幕);

            if (cnt_Program_刷新螢幕 == 65500)
            {
                this.MyTimer_刷新螢幕_結束延遲.TickStop();
                this.MyTimer_刷新螢幕_結束延遲.StartTickTime(200);
                PLC_Device_刷新螢幕.Bool = false;
                PLC_Device_刷新螢幕_OK.Bool = false;
                cnt_Program_刷新螢幕 = 65535;
            }
        }
        void cnt_Program_刷新螢幕_檢查按下(ref int cnt)
        {
            if (PLC_Device_刷新螢幕.Bool) cnt++;
        }
        void cnt_Program_刷新螢幕_檢查放開(ref int cnt)
        {
            if (!PLC_Device_刷新螢幕.Bool) cnt = 65500;
        }
        void cnt_Program_刷新螢幕_初始化(ref int cnt)
        {
            if (this.MyTimer_刷新螢幕_結束延遲.IsTimeOut())
            {
                if (Task_刷新螢幕 == null)
                {
                    Task_刷新螢幕 = new Task(new Action(delegate { PlC_RJ_Button_刷新螢幕_MouseDownEvent(null); }));
                }
                if (Task_刷新螢幕.Status == TaskStatus.RanToCompletion)
                {
                    Task_刷新螢幕 = new Task(new Action(delegate { PlC_RJ_Button_刷新螢幕_MouseDownEvent(null); }));
                }
                if (Task_刷新螢幕.Status == TaskStatus.Created)
                {
                    Task_刷新螢幕.Start();
                }
                cnt++;
            }
        }







        #endregion
    }
}
