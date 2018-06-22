using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniBlinkPinvokeDemo
{
    public partial class TabForm : Form
    {
        public TabForm()
        {
            InitializeComponent();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTab(0);
        }

        /// <summary>
        /// 切换页
        /// </summary>
        /// <param name="type">0 ，切换选项卡 1 跳转地址</param>
        private void ChangeTab(int type)
        {
            var tab = tabControl1.SelectedTab;
            if (tab != null && tab.Controls.Count != 0)
            {
                var browser = tab.Controls[0] as BlinkBrowser;
                if (browser != null)
                {
                    if (type == 0)
                    {
                        textBox1.Text = browser.Url;
                    }
                    else if (type == 1)
                    {
                        browser.Url = textBox1.Text;
                    }
                }
            }
        }

        private IntPtr BlinkBrowser1_OnCreateViewEvent(IntPtr webView, IntPtr param, MiniBlinkPinvoke.wkeNavigationType navigationType, string url)
        {
            return CreateNewTab().handle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeTab(1);
        }

        private void TabForm_Load(object sender, EventArgs e)
        {
            BlinkBrowser browser = CreateNewTab();
            browser.Url = textBox1.Text;
        }

        private BlinkBrowser CreateNewTab()
        {
            var tabPage = new TabPage() { Text = "" };

            MiniBlinkPinvoke.BlinkBrowser browser = new MiniBlinkPinvoke.BlinkBrowser();
            tabPage.Controls.Add(browser);
            browser.OnCreateViewEvent += BlinkBrowser1_OnCreateViewEvent;

            browser.OnTitleChangeCall += (title) =>
            {
                tabPage.Invoke((EventHandler)delegate
                {
                    tabPage.Text = title;
                });
            };
            browser.OnUrlChange2Call += (nowUrl) =>
            {
                tabPage.Invoke((EventHandler)delegate
                {
                    if (tabControl1.SelectedTab == tabPage)
                    {
                        textBox1.Invoke((EventHandler)delegate
                        {
                            textBox1.Text = nowUrl;
                        });
                    }
                });
            };
            browser.Dock = DockStyle.Fill;
            tabControl1.TabPages.Add(tabPage);
            tabControl1.SelectTab(tabPage);
            return browser;
        }

    }
}
