using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
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
            blinkBrowser1.OnDownloadFile += BlinkBrowser1_OnDownloadFile;
        }
        System.Windows.Forms.Timer queryTimer = new System.Windows.Forms.Timer();
        private IntPtr taskPtr;
        XL.DownTaskInfo taskInfo = new XL.DownTaskInfo();
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        private void BlinkBrowser1_OnDownloadFile(string url)
        {
            //只是简单实现下载，可以模仿其他下载界面，先弹出一个窗体，让用户选择保存路径文件名什么的。
            //使用前先拷贝 libs/XL 目录中文件到软件运行目录。
            if (File.Exists("xldl.dll"))
            {
                var initSuccess = XL.XL_Init();
                if (initSuccess)
                {
                    XL.DownTaskParam p = new XL.DownTaskParam()
                    {
                        IsResume = 0,
                        szTaskUrl = textBox1.Text,
                        szFilename = url.Substring(url.LastIndexOf('/') + 1),//简单处理文件名，实际中还需要单独处理，这里有BUG
                        szSavePath = AppDomain.CurrentDomain.BaseDirectory
                    };
                    taskPtr = XL.XL_CreateTask(p);
                    var startSuccess = XL.XL_StartTask(taskPtr);
                    if (startSuccess)
                    {
                        queryTimer.Interval = 500;//半秒查询一次状态
                        queryTimer.Tick += (s, e) =>
                        {
                            var queryRes = XL.XL_QueryTaskInfoEx(taskPtr, taskInfo);
                            if (queryRes)
                            {
                                if (taskInfo.stat == XL.DOWN_TASK_STATUS.TSC_COMPLETE)
                                {
                                    queryTimer.Enabled = false;
                                    MessageBox.Show("下载完成。" + taskInfo.stat);
                                    //下载完成。
                                    Console.WriteLine("下载完成");
                                }
                                else if (taskInfo.stat == XL.DOWN_TASK_STATUS.TSC_ERROR)
                                {
                                    queryTimer.Enabled = false;
                                    MessageBox.Show("下载失败。"+ taskInfo.stat);
                                }
                                else
                                {
                                    Console.WriteLine(string.Format("{0} {1} 进度{2},速度{3},状态{4}", DateTime.Now, taskInfo.szFilename, taskInfo.fPercent, taskInfo.nSpeed, taskInfo.stat));
                                }
                            }
                        };
                        queryTimer.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("XL_Init初始化失败");
                }
            }
            else
            {
                MessageBox.Show("请先将 libs/XL 目录中文件到软件运行目录。");
            }
        }


        //[JSFunctin]
        //public static async void DOTaskWithAsync(string str)
        //{
        //    await Task.Run(() =>
        //    {
        //        Dotaskfunction(str);
        //    });
        //}
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
            blinkBrowser1.ShowDevtools(Application.StartupPath + @"\front_end\inspector.html");
            //MiniBlinkPinvoke.BlinkBrowserPInvoke.wkeSetDebugConfig(blinkBrowser1.handle, "showDevTools", Application.StartupPath + @"\front_end\inspector.html");
        }
    }

}
