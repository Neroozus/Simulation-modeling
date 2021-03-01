namespace DistributionModels
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.timeSumulationUserBox = new System.Windows.Forms.TextBox();
            this.sigmaRecoveryUserBox = new System.Windows.Forms.TextBox();
            this.muRecoveryUserBox = new System.Windows.Forms.TextBox();
            this.lambdaRecoveryUserBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.InfelicityUserBox = new System.Windows.Forms.TextBox();
            this.radioButtonTime = new System.Windows.Forms.RadioButton();
            this.radioButtonInfelicity = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lambdaFailureUserBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.muFailureUserBox = new System.Windows.Forms.TextBox();
            this.sigmaFailureUserBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StartModelingBtn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.workLabel = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.CountIterationBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.BackColor = System.Drawing.SystemColors.Control;
            this.zedGraphControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zedGraphControl1.Location = new System.Drawing.Point(36, 280);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(757, 331);
            this.zedGraphControl1.TabIndex = 1;
            // 
            // timeSumulationUserBox
            // 
            this.timeSumulationUserBox.Location = new System.Drawing.Point(212, 170);
            this.timeSumulationUserBox.Name = "timeSumulationUserBox";
            this.timeSumulationUserBox.Size = new System.Drawing.Size(66, 20);
            this.timeSumulationUserBox.TabIndex = 3;
            this.timeSumulationUserBox.TextChanged += new System.EventHandler(this.timeSumulationUserBox_TextChanged);
            this.timeSumulationUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.timeSumulationUserBox_KeyPress);
            // 
            // sigmaRecoveryUserBox
            // 
            this.sigmaRecoveryUserBox.Location = new System.Drawing.Point(8, 106);
            this.sigmaRecoveryUserBox.Name = "sigmaRecoveryUserBox";
            this.sigmaRecoveryUserBox.Size = new System.Drawing.Size(100, 20);
            this.sigmaRecoveryUserBox.TabIndex = 4;
            this.sigmaRecoveryUserBox.TextChanged += new System.EventHandler(this.sigmaRecoveryUserBox_TextChanged);
            this.sigmaRecoveryUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sigmaRecoveryUserBox_KeyPress);
            // 
            // muRecoveryUserBox
            // 
            this.muRecoveryUserBox.Location = new System.Drawing.Point(8, 55);
            this.muRecoveryUserBox.Name = "muRecoveryUserBox";
            this.muRecoveryUserBox.Size = new System.Drawing.Size(100, 20);
            this.muRecoveryUserBox.TabIndex = 5;
            this.muRecoveryUserBox.TextChanged += new System.EventHandler(this.muRecoveryUserBox_TextChanged);
            this.muRecoveryUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.muRecoveryUserBox_KeyPress);
            // 
            // lambdaRecoveryUserBox
            // 
            this.lambdaRecoveryUserBox.Location = new System.Drawing.Point(171, 55);
            this.lambdaRecoveryUserBox.Name = "lambdaRecoveryUserBox";
            this.lambdaRecoveryUserBox.Size = new System.Drawing.Size(100, 20);
            this.lambdaRecoveryUserBox.TabIndex = 6;
            this.lambdaRecoveryUserBox.TextChanged += new System.EventHandler(this.lambdaRecoveryUserBox_TextChanged);
            this.lambdaRecoveryUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lambdaRecoveryUserBox_KeyPress);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.InfelicityUserBox);
            this.panel1.Controls.Add(this.radioButtonTime);
            this.panel1.Controls.Add(this.radioButtonInfelicity);
            this.panel1.Controls.Add(this.timeSumulationUserBox);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(222, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 198);
            this.panel1.TabIndex = 7;
            // 
            // InfelicityUserBox
            // 
            this.InfelicityUserBox.Location = new System.Drawing.Point(443, 170);
            this.InfelicityUserBox.Name = "InfelicityUserBox";
            this.InfelicityUserBox.Size = new System.Drawing.Size(66, 20);
            this.InfelicityUserBox.TabIndex = 3;
            this.InfelicityUserBox.TextChanged += new System.EventHandler(this.InfelicityUserBox_TextChanged);
            this.InfelicityUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InfelicityUserBox_KeyPress);
            // 
            // radioButtonTime
            // 
            this.radioButtonTime.AutoSize = true;
            this.radioButtonTime.Location = new System.Drawing.Point(16, 172);
            this.radioButtonTime.Name = "radioButtonTime";
            this.radioButtonTime.Size = new System.Drawing.Size(190, 17);
            this.radioButtonTime.TabIndex = 15;
            this.radioButtonTime.TabStop = true;
            this.radioButtonTime.Text = "Время моделирования (в часах):";
            this.radioButtonTime.UseVisualStyleBackColor = true;
            this.radioButtonTime.CheckedChanged += new System.EventHandler(this.radioButtonTime_CheckedChanged);
            // 
            // radioButtonInfelicity
            // 
            this.radioButtonInfelicity.AutoSize = true;
            this.radioButtonInfelicity.Location = new System.Drawing.Point(304, 172);
            this.radioButtonInfelicity.Name = "radioButtonInfelicity";
            this.radioButtonInfelicity.Size = new System.Drawing.Size(133, 17);
            this.radioButtonInfelicity.TabIndex = 15;
            this.radioButtonInfelicity.TabStop = true;
            this.radioButtonInfelicity.Text = "Задать погрешность:";
            this.radioButtonInfelicity.UseVisualStyleBackColor = true;
            this.radioButtonInfelicity.CheckedChanged += new System.EventHandler(this.radioButtonInfelicity_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lambdaFailureUserBox);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.muFailureUserBox);
            this.panel4.Controls.Add(this.sigmaFailureUserBox);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Location = new System.Drawing.Point(8, 31);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(282, 133);
            this.panel4.TabIndex = 14;
            // 
            // lambdaFailureUserBox
            // 
            this.lambdaFailureUserBox.Location = new System.Drawing.Point(170, 55);
            this.lambdaFailureUserBox.Name = "lambdaFailureUserBox";
            this.lambdaFailureUserBox.Size = new System.Drawing.Size(100, 20);
            this.lambdaFailureUserBox.TabIndex = 16;
            this.lambdaFailureUserBox.TextChanged += new System.EventHandler(this.lambdaFailureUserBox_TextChanged);
            this.lambdaFailureUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lambdaFailureUserBox_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(167, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "T среднее:";
            // 
            // muFailureUserBox
            // 
            this.muFailureUserBox.Location = new System.Drawing.Point(7, 55);
            this.muFailureUserBox.Name = "muFailureUserBox";
            this.muFailureUserBox.Size = new System.Drawing.Size(100, 20);
            this.muFailureUserBox.TabIndex = 15;
            this.muFailureUserBox.TextChanged += new System.EventHandler(this.muFailureUserBox_TextChanged);
            this.muFailureUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.muFailureUserBox_KeyPress);
            // 
            // sigmaFailureUserBox
            // 
            this.sigmaFailureUserBox.Location = new System.Drawing.Point(7, 106);
            this.sigmaFailureUserBox.Name = "sigmaFailureUserBox";
            this.sigmaFailureUserBox.Size = new System.Drawing.Size(100, 20);
            this.sigmaFailureUserBox.TabIndex = 14;
            this.sigmaFailureUserBox.TextChanged += new System.EventHandler(this.sigmaFailureUserBox_TextChanged);
            this.sigmaFailureUserBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sigmaFailureUserBox_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 78);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 26);
            this.label13.TabIndex = 18;
            this.label13.Text = "Среднеквадратичное \r\nотклонение:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(53, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(171, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Параметры для отказа (в часах)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 26);
            this.label14.TabIndex = 17;
            this.label14.Text = "Математическое\r\nожидание:";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.lambdaRecoveryUserBox);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.muRecoveryUserBox);
            this.panel3.Controls.Add(this.sigmaRecoveryUserBox);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(295, 31);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(281, 133);
            this.panel3.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(219, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Параметры для восстановления (в часах)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Т среднее:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "Среднеквадратичное \r\nотклонение:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 26);
            this.label2.TabIndex = 8;
            this.label2.Text = "Математическое\r\nожидание:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(226, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Введите исходные данные";
            // 
            // StartModelingBtn
            // 
            this.StartModelingBtn.Location = new System.Drawing.Point(337, 247);
            this.StartModelingBtn.Name = "StartModelingBtn";
            this.StartModelingBtn.Size = new System.Drawing.Size(155, 23);
            this.StartModelingBtn.TabIndex = 8;
            this.StartModelingBtn.Text = "Начать моделирование!";
            this.StartModelingBtn.UseVisualStyleBackColor = true;
            this.StartModelingBtn.Click += new System.EventHandler(this.StartModelingBtn_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(832, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(200, 20);
            this.toolStripMenuItem1.Text = "Вернуть к исходному состоянию";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 619);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(832, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // workLabel
            // 
            this.workLabel.AutoSize = true;
            this.workLabel.Location = new System.Drawing.Point(110, 624);
            this.workLabel.Name = "workLabel";
            this.workLabel.Size = new System.Drawing.Size(35, 13);
            this.workLabel.TabIndex = 11;
            this.workLabel.Text = "label6";
            this.workLabel.TextChanged += new System.EventHandler(this.workLabel_TextChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(5, 104);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 13;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(5, 52);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Выберите распределения:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Для отказа:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Для восстановления:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 137);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 26);
            this.label11.TabIndex = 17;
            this.label11.Text = "Введите количество\r\nитераций:";
            // 
            // CountIterationBox
            // 
            this.CountIterationBox.Location = new System.Drawing.Point(9, 166);
            this.CountIterationBox.Name = "CountIterationBox";
            this.CountIterationBox.Size = new System.Drawing.Size(100, 20);
            this.CountIterationBox.TabIndex = 18;
            this.CountIterationBox.TextChanged += new System.EventHandler(this.CountIterationBox_TextChanged);
            this.CountIterationBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CountIterationBox_KeyPress);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.CountIterationBox);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Location = new System.Drawing.Point(23, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(181, 198);
            this.panel2.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 641);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.workLabel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.StartModelingBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.menuStrip1);
            this.MaximumSize = new System.Drawing.Size(848, 680);
            this.MinimumSize = new System.Drawing.Size(848, 680);
            this.Name = "Form1";
            this.Text = "Моделирование отказ - восстановление";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.TextBox timeSumulationUserBox;
        private System.Windows.Forms.TextBox sigmaRecoveryUserBox;
        private System.Windows.Forms.TextBox muRecoveryUserBox;
        private System.Windows.Forms.TextBox lambdaRecoveryUserBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartModelingBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label workLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox lambdaFailureUserBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox muFailureUserBox;
        private System.Windows.Forms.TextBox sigmaFailureUserBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton radioButtonTime;
        private System.Windows.Forms.RadioButton radioButtonInfelicity;
        private System.Windows.Forms.TextBox InfelicityUserBox;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox CountIterationBox;
        private System.Windows.Forms.Panel panel2;
    }
}

