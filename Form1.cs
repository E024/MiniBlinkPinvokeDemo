﻿using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        }

        private void blinkBrowser1_OnUrlChangeCall(string url)
        {
            this.textBox1.Text = url;
        }

    }
}
