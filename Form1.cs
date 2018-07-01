using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniBlinkPinvokeDemo
{
    public partial class Form1 : Form
    {
        static BlinkBrowser mb = null;
        public Form1()
        {
            InitializeComponent();
            blinkBrowser1.GlobalObjectJs = this;
            mb = blinkBrowser1;
        }
        [JSFunctin]
        public void Console_WriteLine(string msg)
        {
            MessageBox.Show("Console_WriteLine ：" + msg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            blinkBrowser1.Url = this.textBox1.Text.Trim();
            blinkBrowser1.Focus();
            //BlinkBrowserPInvoke.wkeSetHandleOffset(blinkBrowser1.handle, Location.X, Location.Y);
        }
        private void blinkBrowser1_OnUrlChangeCall(string url)
        {
            this.textBox1.Text = url;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //BlinkBrowserPInvoke.wkeSetHandle(blinkBrowser1.handle, this.Handle);
            //BlinkBrowserPInvoke.wkeSetHandleOffset(blinkBrowser1.handle, blinkBrowser1.Location.X-2,0);
            blinkBrowser1.OnUrlChange2Call += BlinkBrowser1_OnUrlChange2Call;
            blinkBrowser1.OnTitleChangeCall += BlinkBrowser1_OnTitleChangeCall;
            blinkBrowser1.DocumentReadyCallback += BlinkBrowser1_DocumentReadyCallback;
        }
        [JSFunctin]
        public static async void DOTaskWithAsync(string str)
        {
            await Task.Run(() =>
            {
                Dotaskfunction(str);
            });
        }
        public static void Dotaskfunction(string s)
        {
            Thread.Sleep(1000 * 5);
            if (mb != null)
            {
                mb.Invoke(new Action<string>(
                    (sx) =>
                    {
                        string _str = "ceshi" + sx;
                        _str = System.Web.HttpUtility.UrlEncode(_str);
                        string invokeJSStr = "asyncTest('" + _str + "')";
                        mb.InvokeJS(invokeJSStr);
                    }
                    ), s);
            }

        }
        private void BlinkBrowser1_DocumentReadyCallback()
        {
        }

        private void BlinkBrowser1_OnTitleChangeCall(string title)
        {
            Text = title;
        }

        private void BlinkBrowser1_OnUrlChange2Call(string url)
        {
            textBox1.Text = url;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MiniBlinkPinvoke.BlinkBrowserPInvoke.wkeSetDebugConfig(blinkBrowser1.handle, "showDevTools", @"G:\qqmsg\370588543\FileRecv\miniblink180317\front_end\inspector.html");
        }
    }

}
