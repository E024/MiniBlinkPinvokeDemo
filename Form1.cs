using MiniBlinkPinvoke;
using Newtonsoft.Json;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            blinkBrowser1.Url = this.textBox1.Text.Trim();

            blinkBrowser1.Focus();
            BlinkBrowserPInvoke.wkeSetHandleOffset(blinkBrowser1.handle, Location.X, Location.Y);
        }
        private void blinkBrowser1_OnUrlChangeCall(string url)
        {
            this.textBox1.Text = url;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            blinkBrowser1.OnUrlChange2Call += BlinkBrowser1_OnUrlChange2Call;
            blinkBrowser1.OnTitleChangeCall += BlinkBrowser1_OnTitleChangeCall;
            blinkBrowser1.DocumentReadyCallback += BlinkBrowser1_DocumentReadyCallback;
        }

        private void BlinkBrowser1_DocumentReadyCallback()
        {
        }

        private void BlinkBrowser1_OnTitleChangeCall(IntPtr webView, IntPtr param, IntPtr title)
        {
            Text = MiniBlinkPinvoke.BlinkBrowserPInvoke.wkeGetString(title).Utf8IntptrToString();
        }

        private void BlinkBrowser1_OnUrlChange2Call(string url)
        {
            textBox1.Text = url;
        }
    }

}
