﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace OasysGH.Versions.UI {
  internal partial class UpdatePluginsBox : Form {
    internal string ProcessString { get; private set; }
    public UpdatePluginsBox(string header, string text, string processOnOkClick, Bitmap icon) {
      InitializeComponent();
      Text = header;
      ProcessString = processOnOkClick;
      textBox.Text = text;
      pictureBox.Image = icon;
    }

    private void OkButton_Click(object sender, EventArgs e) {
      Process.Start(ProcessString);
      Close();
    }
  }
}
