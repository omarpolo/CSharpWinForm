namespace Poslovanje.forme
{
    partial class frmSetup
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetup));
            this.Label5 = new System.Windows.Forms.Label();
            this.txtImeMreze = new System.Windows.Forms.TextBox();
            this.lstCMP = new System.Windows.Forms.ListView();
            this.ImageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtLozinka = new System.Windows.Forms.TextBox();
            this.txtKorisnickoIme = new System.Windows.Forms.TextBox();
            this.txtOsnovnaBaza = new System.Windows.Forms.TextBox();
            this.txtPutanjaDoServera = new System.Windows.Forms.TextBox();
            this.dialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLokacijaTmpDirektorijuma = new System.Windows.Forms.TextBox();
            this.optMysql = new System.Windows.Forms.RadioButton();
            this.optAccess = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.dlg = new System.Windows.Forms.OpenFileDialog();
            this.btnUpisi = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.btnPretraga = new System.Windows.Forms.Button();
            this.pnlSlika = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlSlika.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(2, 278);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(61, 13);
            this.Label5.TabIndex = 62;
            this.Label5.Text = "Ime mreže :";
            // 
            // txtImeMreze
            // 
            this.txtImeMreze.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImeMreze.Location = new System.Drawing.Point(69, 273);
            this.txtImeMreze.Name = "txtImeMreze";
            this.txtImeMreze.Size = new System.Drawing.Size(271, 20);
            this.txtImeMreze.TabIndex = 60;
            // 
            // lstCMP
            // 
            this.lstCMP.BackColor = System.Drawing.Color.GhostWhite;
            this.lstCMP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCMP.LargeImageList = this.ImageList1;
            this.lstCMP.Location = new System.Drawing.Point(2, 299);
            this.lstCMP.Name = "lstCMP";
            this.lstCMP.Size = new System.Drawing.Size(439, 94);
            this.lstCMP.SmallImageList = this.ImageList1;
            this.lstCMP.TabIndex = 59;
            this.lstCMP.UseCompatibleStateImageBehavior = false;
            this.lstCMP.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstCMP_ItemSelectionChanged);
            // 
            // ImageList1
            // 
            this.ImageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList1.ImageStream")));
            this.ImageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList1.Images.SetKeyName(0, "1.ico");
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(2, 181);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(50, 13);
            this.Label4.TabIndex = 58;
            this.Label4.Text = "Lozinka :";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(2, 160);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(50, 13);
            this.Label3.TabIndex = 57;
            this.Label3.Text = "Korisnik :";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(2, 139);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(110, 13);
            this.Label2.TabIndex = 56;
            this.Label2.Text = "Naziv osnovne baze :";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(2, 120);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(78, 13);
            this.Label1.TabIndex = 54;
            this.Label1.Text = "Naziv servera :";
            // 
            // txtLozinka
            // 
            this.txtLozinka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLozinka.Location = new System.Drawing.Point(116, 181);
            this.txtLozinka.Name = "txtLozinka";
            this.txtLozinka.Size = new System.Drawing.Size(108, 20);
            this.txtLozinka.TabIndex = 53;
            // 
            // txtKorisnickoIme
            // 
            this.txtKorisnickoIme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKorisnickoIme.Location = new System.Drawing.Point(116, 160);
            this.txtKorisnickoIme.Name = "txtKorisnickoIme";
            this.txtKorisnickoIme.Size = new System.Drawing.Size(108, 20);
            this.txtKorisnickoIme.TabIndex = 52;
            // 
            // txtOsnovnaBaza
            // 
            this.txtOsnovnaBaza.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOsnovnaBaza.Location = new System.Drawing.Point(116, 139);
            this.txtOsnovnaBaza.Name = "txtOsnovnaBaza";
            this.txtOsnovnaBaza.Size = new System.Drawing.Size(325, 20);
            this.txtOsnovnaBaza.TabIndex = 51;
            // 
            // txtPutanjaDoServera
            // 
            this.txtPutanjaDoServera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPutanjaDoServera.Location = new System.Drawing.Point(116, 118);
            this.txtPutanjaDoServera.Name = "txtPutanjaDoServera";
            this.txtPutanjaDoServera.Size = new System.Drawing.Size(302, 20);
            this.txtPutanjaDoServera.TabIndex = 50;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 65;
            this.label6.Text = "Lok.dir.za tmp.fajlove :";
            // 
            // txtLokacijaTmpDirektorijuma
            // 
            this.txtLokacijaTmpDirektorijuma.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLokacijaTmpDirektorijuma.Location = new System.Drawing.Point(116, 202);
            this.txtLokacijaTmpDirektorijuma.Name = "txtLokacijaTmpDirektorijuma";
            this.txtLokacijaTmpDirektorijuma.Size = new System.Drawing.Size(108, 20);
            this.txtLokacijaTmpDirektorijuma.TabIndex = 64;
            // 
            // optMysql
            // 
            this.optMysql.AutoSize = true;
            this.optMysql.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.optMysql.Location = new System.Drawing.Point(119, 94);
            this.optMysql.Name = "optMysql";
            this.optMysql.Size = new System.Drawing.Size(60, 17);
            this.optMysql.TabIndex = 66;
            this.optMysql.TabStop = true;
            this.optMysql.Text = "MySQL";
            this.optMysql.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.optMysql.UseVisualStyleBackColor = true;
            // 
            // optAccess
            // 
            this.optAccess.AutoSize = true;
            this.optAccess.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.optAccess.Location = new System.Drawing.Point(201, 94);
            this.optAccess.Name = "optAccess";
            this.optAccess.Size = new System.Drawing.Size(86, 17);
            this.optAccess.TabIndex = 67;
            this.optAccess.TabStop = true;
            this.optAccess.Text = "MS ACCESS";
            this.optAccess.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.optAccess.UseVisualStyleBackColor = true;
            this.optAccess.CheckedChanged += new System.EventHandler(this.optAccess_CheckedChanged);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(417, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 20);
            this.button1.TabIndex = 68;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dlg
            // 
            this.dlg.FileName = "OpenFileDialog1";
            // 
            // btnUpisi
            // 
            this.btnUpisi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUpisi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnUpisi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUpisi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpisi.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnUpisi.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpisi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnUpisi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnUpisi.ForeColor = System.Drawing.Color.White;
            this.btnUpisi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpisi.Location = new System.Drawing.Point(247, 202);
            this.btnUpisi.Name = "btnUpisi";
            this.btnUpisi.Size = new System.Drawing.Size(93, 25);
            this.btnUpisi.TabIndex = 69;
            this.btnUpisi.Text = "&Upiši";
            this.btnUpisi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpisi.UseVisualStyleBackColor = false;
            this.btnUpisi.Click += new System.EventHandler(this.btnUpisi_Click);
            // 
            // btnOdustani
            // 
            this.btnOdustani.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOdustani.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnOdustani.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOdustani.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnOdustani.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOdustani.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnOdustani.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOdustani.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOdustani.ForeColor = System.Drawing.Color.White;
            this.btnOdustani.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOdustani.Location = new System.Drawing.Point(348, 202);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(93, 25);
            this.btnOdustani.TabIndex = 96;
            this.btnOdustani.Text = "&Odustani";
            this.btnOdustani.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOdustani.UseVisualStyleBackColor = false;
            this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
            // 
            // btnPretraga
            // 
            this.btnPretraga.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPretraga.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnPretraga.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPretraga.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPretraga.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnPretraga.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPretraga.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnPretraga.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPretraga.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPretraga.ForeColor = System.Drawing.Color.White;
            this.btnPretraga.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPretraga.Location = new System.Drawing.Point(348, 270);
            this.btnPretraga.Name = "btnPretraga";
            this.btnPretraga.Size = new System.Drawing.Size(93, 25);
            this.btnPretraga.TabIndex = 97;
            this.btnPretraga.Text = "Pretraži mrežu";
            this.btnPretraga.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPretraga.UseVisualStyleBackColor = false;
            this.btnPretraga.Click += new System.EventHandler(this.btnPretraga_Click);
            // 
            // pnlSlika
            // 
            this.pnlSlika.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.pnlSlika.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSlika.Controls.Add(this.label7);
            this.pnlSlika.Controls.Add(this.PictureBox1);
            this.pnlSlika.Location = new System.Drawing.Point(0, 0);
            this.pnlSlika.Name = "pnlSlika";
            this.pnlSlika.Size = new System.Drawing.Size(449, 87);
            this.pnlSlika.TabIndex = 98;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(59, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(232, 18);
            this.label7.TabIndex = 1;
            this.label7.Text = "Podešavanje parametara";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = global::Poslovanje.Properties.Resources.network_icon;
            this.PictureBox1.Location = new System.Drawing.Point(5, 9);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(52, 62);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // frmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(445, 396);
            this.Controls.Add(this.pnlSlika);
            this.Controls.Add(this.btnPretraga);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnUpisi);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.optAccess);
            this.Controls.Add(this.optMysql);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLokacijaTmpDirektorijuma);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.txtImeMreze);
            this.Controls.Add(this.lstCMP);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtLozinka);
            this.Controls.Add(this.txtKorisnickoIme);
            this.Controls.Add(this.txtOsnovnaBaza);
            this.Controls.Add(this.txtPutanjaDoServera);
            this.Name = "frmSetup";
            this.Text = "frmSetup";
            this.Load += new System.EventHandler(this.frmSetup_Load);
            this.pnlSlika.ResumeLayout(false);
            this.pnlSlika.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtImeMreze;
        internal System.Windows.Forms.ListView lstCMP;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox txtLozinka;
        internal System.Windows.Forms.TextBox txtKorisnickoIme;
        internal System.Windows.Forms.TextBox txtOsnovnaBaza;
        internal System.Windows.Forms.TextBox txtPutanjaDoServera;
        internal System.Windows.Forms.FolderBrowserDialog dialog1;
        internal System.Windows.Forms.ImageList ImageList1;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtLokacijaTmpDirektorijuma;
        private System.Windows.Forms.RadioButton optMysql;
        private System.Windows.Forms.RadioButton optAccess;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.OpenFileDialog dlg;
        private System.Windows.Forms.Button btnUpisi;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.Button btnPretraga;
        internal System.Windows.Forms.Panel pnlSlika;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.PictureBox PictureBox1;


    }
}