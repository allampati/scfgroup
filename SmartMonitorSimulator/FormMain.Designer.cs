namespace SmartMonitorSimulator
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxGpsStartStop = new System.Windows.Forms.CheckBox();
            this.comboBoxGpsType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxGpsInterval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewReceivedMsgs = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxGpsStartStop);
            this.groupBox1.Controls.Add(this.comboBoxGpsType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxGpsInterval);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 185);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS";
            // 
            // checkBoxGpsStartStop
            // 
            this.checkBoxGpsStartStop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxGpsStartStop.AutoSize = true;
            this.checkBoxGpsStartStop.Location = new System.Drawing.Point(179, 156);
            this.checkBoxGpsStartStop.Name = "checkBoxGpsStartStop";
            this.checkBoxGpsStartStop.Size = new System.Drawing.Size(90, 23);
            this.checkBoxGpsStartStop.TabIndex = 5;
            this.checkBoxGpsStartStop.Text = "Start Simulation";
            this.checkBoxGpsStartStop.UseVisualStyleBackColor = true;
            this.checkBoxGpsStartStop.CheckedChanged += new System.EventHandler(this.checkBoxGpsStartStop_CheckedChanged);
            // 
            // comboBoxGpsType
            // 
            this.comboBoxGpsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGpsType.FormattingEnabled = true;
            this.comboBoxGpsType.Items.AddRange(new object[] {
            "City",
            "Country",
            "World"});
            this.comboBoxGpsType.Location = new System.Drawing.Point(101, 60);
            this.comboBoxGpsType.Name = "comboBoxGpsType";
            this.comboBoxGpsType.Size = new System.Drawing.Size(168, 21);
            this.comboBoxGpsType.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Simulation Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Seconds";
            // 
            // textBoxGpsInterval
            // 
            this.textBoxGpsInterval.Location = new System.Drawing.Point(84, 26);
            this.textBoxGpsInterval.Name = "textBoxGpsInterval";
            this.textBoxGpsInterval.Size = new System.Drawing.Size(38, 20);
            this.textBoxGpsInterval.TabIndex = 1;
            this.textBoxGpsInterval.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Interval:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewReceivedMsgs);
            this.groupBox2.Location = new System.Drawing.Point(12, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(905, 330);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Received Messages";
            // 
            // listViewReceivedMsgs
            // 
            this.listViewReceivedMsgs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnData});
            this.listViewReceivedMsgs.FullRowSelect = true;
            this.listViewReceivedMsgs.GridLines = true;
            this.listViewReceivedMsgs.Location = new System.Drawing.Point(10, 30);
            this.listViewReceivedMsgs.Name = "listViewReceivedMsgs";
            this.listViewReceivedMsgs.Size = new System.Drawing.Size(889, 294);
            this.listViewReceivedMsgs.TabIndex = 0;
            this.listViewReceivedMsgs.UseCompatibleStateImageBehavior = false;
            this.listViewReceivedMsgs.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 100;
            // 
            // columnData
            // 
            this.columnData.Text = "Data";
            this.columnData.Width = 100;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 545);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormMain";
            this.Text = "Smart Monitor Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGpsInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxGpsType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxGpsStartStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewReceivedMsgs;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnData;
    }
}

