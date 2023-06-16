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
        private string last_keyData = "";
        private bool 全螢幕 = false;
        MyTimer myTimer_ESC = new MyTimer();
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(plC_ScreenPage_Main.PageText == "設定")
            {
                if (rJ_TextBox_第一台_加一號.IsFocused) rJ_TextBox_第一台_加一號.Text = keyData.ToString();
                if (rJ_TextBox_第一台_減一號.IsFocused) rJ_TextBox_第一台_減一號.Text = keyData.ToString();
                if (rJ_TextBox_第一台_加兩號.IsFocused) rJ_TextBox_第一台_加兩號.Text = keyData.ToString();
                if (rJ_TextBox_第一台_減兩號.IsFocused) rJ_TextBox_第一台_減兩號.Text = keyData.ToString();

                if (rJ_TextBox_第二台_加一號.IsFocused) rJ_TextBox_第二台_加一號.Text = keyData.ToString();
                if (rJ_TextBox_第二台_減一號.IsFocused) rJ_TextBox_第二台_減一號.Text = keyData.ToString();
                if (rJ_TextBox_第二台_加兩號.IsFocused) rJ_TextBox_第二台_加兩號.Text = keyData.ToString();
                if (rJ_TextBox_第二台_減兩號.IsFocused) rJ_TextBox_第二台_減兩號.Text = keyData.ToString();
            }
            if (plC_ScreenPage_Main.PageText == "主畫面")
            {
                if(keyData.ToString() == "Escape")
                {
                    myTimer_ESC.StartTickTime(1000);
                    if(myTimer_ESC.IsTimeOut())
                    {
                        Basic.Screen.FullScreen(this.FindForm(), 0, false);
                        panel_Main.Visible = true;
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
               
                }
                else
                {
                    myTimer_ESC.IsTimeOut();
                }
                if (!全螢幕) Function_號碼增減檢查(myConfigClass.機台代碼, keyData.ToString());


            }
            else
            {
            }
            last_keyData = keyData.ToString();
            System.Threading.Thread.Sleep(200);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Function_號碼增減檢查(string 機台代碼 , string keyData)
        {
            List<object[]> list_value = this.sqL_DataGridView_叫號台設定.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_叫號台設定.代碼, 機台代碼);
            if (list_value.Count == 0) return;
            bool flag_replace = false;
            int num = list_value[0][(int)enum_叫號台設定.叫號號碼].StringToInt32();
            if (list_value[0][(int)enum_叫號台設定.加一號按鈕].ObjectToString() == keyData)
            {
                list_value[0][(int)enum_叫號台設定.叫號號碼] = (num + 1).ToString("0000");
                flag_replace = true;
            }
            if (list_value[0][(int)enum_叫號台設定.減一號按鈕].ObjectToString() == keyData)
            {
                if ((num - 1) < 0) return;
                list_value[0][(int)enum_叫號台設定.叫號號碼] = (num - 1).ToString("0000");
                flag_replace = true;
            }
            if (list_value[0][(int)enum_叫號台設定.加二號按鈕].ObjectToString() == keyData)
            {
                list_value[0][(int)enum_叫號台設定.叫號號碼] = (num + 2).ToString("0000");
                flag_replace = true;
            }
            if (list_value[0][(int)enum_叫號台設定.減二號按鈕].ObjectToString() == keyData)
            {
                if ((num - 2) < 0) return;
                list_value[0][(int)enum_叫號台設定.叫號號碼] = (num - 2).ToString("0000");
                flag_replace = true;
            }

            if(flag_replace)this.sqL_DataGridView_叫號台設定.SQL_ReplaceExtra(list_value, false);
        }


        #region DBConfigClass
        private const string DBConfigFileName = "DBConfig.txt";
        public DBConfigClass dBConfigClass = new DBConfigClass();
        public class DBConfigClass
        {
            private SQL_DataGridView.ConnentionClass dB_Basic = new SQL_DataGridView.ConnentionClass();

            public SQL_DataGridView.ConnentionClass DB_Basic { get => dB_Basic; set => dB_Basic = value; }
        }
        private void LoadDBConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{DBConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {

                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(new DBConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{DBConfigFileName}");
                Application.Exit();
            }
            else
            {
                dBConfigClass = Basic.Net.JsonDeserializet<DBConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(dBConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }

            }


        }
        #endregion
        #region MyConfigClass
        private const string MyConfigFileName = "MyConfig.txt";
        public MyConfigClass myConfigClass = new MyConfigClass();
        public class MyConfigClass
        {

            private string _機台代碼 = "";

            public string 機台代碼 { get => _機台代碼; set => _機台代碼 = value; }
        }
        private void LoadMyConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{MyConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(new MyConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{MyConfigFileName}");
                Application.Exit();
            }
            else
            {
                myConfigClass = Basic.Net.JsonDeserializet<MyConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(myConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }

            }

        }
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            MyMessageBox.音效 = false;
            MyMessageBox.form = this.FindForm();
            Dialog_螢幕選擇.form = this.FindForm();
            Dialog_NumPannel.form = this.FindForm();
            this.LoadDBConfig();
            this.LoadMyConfig();
            textBox_機台代碼.Text = myConfigClass.機台代碼;

            this.plC_RJ_Button_全螢幕顯示.MouseDownEvent += PlC_RJ_Button_全螢幕顯示_MouseDownEvent;
            this.plC_UI_Init.Run(this.FindForm(), this.lowerMachine_Panel);
            this.plC_UI_Init.UI_Finished_Event += PlC_UI_Init_UI_Finished_Event;

        }
        private void PlC_RJ_Button_全螢幕顯示_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_螢幕選擇 dialog_螢幕選擇 = new Dialog_螢幕選擇();
      
            this.Invoke(new Action(delegate
            {
                if (dialog_螢幕選擇.ShowDialog() == DialogResult.Yes)
                {
                    try
                    {
                        Basic.Screen.FullScreen(this.FindForm(), dialog_螢幕選擇.Value, true);
                        panel_Main.Visible = false;
                        this.全螢幕 = true;
                    }
                    catch
                    {
                        Basic.Screen.FullScreen(this.FindForm(), 0, false);
                        MyMessageBox.ShowDialog("找無此螢幕!");
                    }
                }
            }));
            
         
        }
        private void PlC_UI_Init_UI_Finished_Event()
        {
            PLC_UI_Init.Set_PLC_ScreenPage(this.panel_Main, this.plC_ScreenPage_Main);
            this.plC_ScreenPage_Main.TabChangeEvent += PlC_ScreenPage_Main_TabChangeEvent;
            this.Program_系統();
            this.Program_設定();
            this.Program_主畫面();

            this.Function_設定讀取(myConfigClass.機台代碼);
        }
        private void PlC_ScreenPage_Main_TabChangeEvent(string PageText)
        {
            if(PageText == "設定")
            {
                List<object[]> list_value = sqL_DataGridView_叫號台設定.SQL_GetAllRows(false);
                List<string> list_代碼 = new List<string>();
                for (int i = 0; i < list_value.Count; i++)
                {
                    bool flag_add = true;
                    string 代碼 = list_value[i][(int)enum_叫號台設定.代碼].ObjectToString();
                    for (int k = 0; k < list_代碼.Count; k++)
                    {
                        if (list_代碼[k] == 代碼)
                        {
                            flag_add = false;
                        }
                    }
                    if (flag_add) list_代碼.Add(代碼);
                }


                this.Invoke(new Action(delegate
                {
                    comboBox_代碼.Items.Clear();
                    for (int k = 0; k < list_代碼.Count; k++) comboBox_代碼.Items.Add(list_代碼[k]);
                    if (comboBox_代碼.Items.Count > 0) comboBox_代碼.SelectedIndex = 0;
                }));
            }
        }

        #region PLC_Method
        PLC_Device PLC_Device_Method = new PLC_Device("");
        PLC_Device PLC_Device_Method_OK = new PLC_Device("");
        Task Task_Method;
        MyTimer MyTimer_Method_結束延遲 = new MyTimer();
        int cnt_Program_Method = 65534;
        void sub_Program_Method()
        {
            if (cnt_Program_Method == 65534)
            {
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.SetComment("PLC_Method");
                PLC_Device_Method_OK.SetComment("PLC_Method_OK");
                PLC_Device_Method.Bool = false;
                cnt_Program_Method = 65535;
            }
            if (cnt_Program_Method == 65535) cnt_Program_Method = 1;
            if (cnt_Program_Method == 1) cnt_Program_Method_檢查按下(ref cnt_Program_Method);
            if (cnt_Program_Method == 2) cnt_Program_Method_初始化(ref cnt_Program_Method);
            if (cnt_Program_Method == 3) cnt_Program_Method = 65500;
            if (cnt_Program_Method > 1) cnt_Program_Method_檢查放開(ref cnt_Program_Method);

            if (cnt_Program_Method == 65500)
            {
                this.MyTimer_Method_結束延遲.TickStop();
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.Bool = false;
                PLC_Device_Method_OK.Bool = false;
                cnt_Program_Method = 65535;
            }
        }
        void cnt_Program_Method_檢查按下(ref int cnt)
        {
            if (PLC_Device_Method.Bool) cnt++;
        }
        void cnt_Program_Method_檢查放開(ref int cnt)
        {
            if (!PLC_Device_Method.Bool) cnt = 65500;
        }
        void cnt_Program_Method_初始化(ref int cnt)
        {
            if (this.MyTimer_Method_結束延遲.IsTimeOut())
            {
                if (Task_Method == null)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.RanToCompletion)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.Created)
                {
                    Task_Method.Start();
                }
                cnt++;
            }
        }







        #endregion
    }
}
