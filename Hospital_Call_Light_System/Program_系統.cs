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
        public enum enum_參數
        {
            GUID,
            Name,
            Value,
        }

        public enum enum_樣式設定
        {
            GUID,
            代碼,
            名稱,
            台號,
            寬度,
            標題名稱,
            標題字體,
            標題文字寬度,
            標題字體顏色,
            標題背景顏色,
            標題高度,
            英文標題高度,
            英文標題字體,
            叫號號碼,
            叫號字體,
            叫號文字寬度,
            叫號字體顏色,
            叫號背景顏色,
            叫號備註高度,
            叫號備註字體,
            加一號,
            減一號按鈕,
            加二號按鈕,
            減二號按鈕,

        }

        private void Program_系統()
        {
            SQL_DataGridView.SQL_Set_Properties(sqL_DataGridView_樣式設定, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_樣式設定.Init();
            if (this.sqL_DataGridView_樣式設定.SQL_IsTableCreat() == false) sqL_DataGridView_樣式設定.SQL_CreateTable();
            else sqL_DataGridView_樣式設定.SQL_CheckAllColumnName(true);

            SQL_DataGridView.SQL_Set_Properties(sqL_DataGridView_參數, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_參數.Init();
            if (this.sqL_DataGridView_參數.SQL_IsTableCreat() == false) sqL_DataGridView_參數.SQL_CreateTable();
            else sqL_DataGridView_參數.SQL_CheckAllColumnName(true);
        }

        public class Icp_叫號台設定 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string 台號_0 = x[(int)enum_樣式設定.台號].ObjectToString();
                string 台號_1 = y[(int)enum_樣式設定.台號].ObjectToString();
                return 台號_0.CompareTo(台號_1);
            }
        }
    }
}
