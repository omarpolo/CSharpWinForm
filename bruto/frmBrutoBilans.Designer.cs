namespace Poslovanje.forme
{
    partial class frmBrutoBilans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrutoBilans));
            this.dlg = new System.Windows.Forms.OpenFileDialog();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatDo = new System.Windows.Forms.DateTimePicker();
            this.txtDatOd = new System.Windows.Forms.DateTimePicker();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.optKIF = new System.Windows.Forms.RadioButton();
            this.optKUF = new System.Windows.Forms.RadioButton();
            this.btnOdaberi = new System.Windows.Forms.Button();
            this.btnIzmjena = new System.Windows.Forms.Button();
            this.btnDodavanje = new System.Windows.Forms.Button();
            this.btnDetalji = new System.Windows.Forms.Button();
            this.btnBrisanje = new System.Windows.Forms.Button();
            this.pnlSlika = new System.Windows.Forms.Panel();
            this.Label1 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.grid1 = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.grid2 = new C1.Win.C1TrueDBGrid.C1TrueDBGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.štampaBrutoBilansaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlSlika.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlg
            // 
            this.dlg.FileName = "OpenFileDialog1";
            // 
            // PictureBox2
            // 
            this.PictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox2.Image")));
            this.PictureBox2.Location = new System.Drawing.Point(0, 29);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(26, 21);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox2.TabIndex = 2;
            this.PictureBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtDatDo);
            this.panel1.Controls.Add(this.txtDatOd);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.PictureBox2);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.btnOdustani);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 52);
            this.panel1.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(104, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 107;
            this.label2.Text = "Period";
            // 
            // txtDatDo
            // 
            this.txtDatDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDatDo.Location = new System.Drawing.Point(238, 6);
            this.txtDatDo.Name = "txtDatDo";
            this.txtDatDo.Size = new System.Drawing.Size(88, 20);
            this.txtDatDo.TabIndex = 106;
            this.txtDatDo.ValueChanged += new System.EventHandler(this.txtDatDo_ValueChanged);
            // 
            // txtDatOd
            // 
            this.txtDatOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDatOd.Location = new System.Drawing.Point(144, 6);
            this.txtDatOd.Name = "txtDatOd";
            this.txtDatOd.Size = new System.Drawing.Size(88, 20);
            this.txtDatOd.TabIndex = 105;
            this.txtDatOd.ValueChanged += new System.EventHandler(this.txtDatOd_ValueChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrint.Location = new System.Drawing.Point(5, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 23);
            this.btnPrint.TabIndex = 100;
            this.btnPrint.Text = "Štam&pa";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(26, 30);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(301, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
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
            this.btnOdustani.ForeColor = System.Drawing.Color.White;
            this.btnOdustani.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOdustani.Location = new System.Drawing.Point(674, 4);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(95, 23);
            this.btnOdustani.TabIndex = 35;
            this.btnOdustani.Text = "Odu&stani (Esc)";
            this.btnOdustani.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOdustani.UseVisualStyleBackColor = false;
            this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
            // 
            // optKIF
            // 
            this.optKIF.AutoSize = true;
            this.optKIF.ForeColor = System.Drawing.Color.White;
            this.optKIF.Location = new System.Drawing.Point(369, 26);
            this.optKIF.Name = "optKIF";
            this.optKIF.Size = new System.Drawing.Size(41, 17);
            this.optKIF.TabIndex = 104;
            this.optKIF.TabStop = true;
            this.optKIF.Text = "KIF";
            this.optKIF.UseVisualStyleBackColor = true;
            this.optKIF.Visible = false;
            // 
            // optKUF
            // 
            this.optKUF.AutoSize = true;
            this.optKUF.ForeColor = System.Drawing.Color.White;
            this.optKUF.Location = new System.Drawing.Point(323, 25);
            this.optKUF.Name = "optKUF";
            this.optKUF.Size = new System.Drawing.Size(46, 17);
            this.optKUF.TabIndex = 103;
            this.optKUF.TabStop = true;
            this.optKUF.Text = "KUF";
            this.optKUF.UseVisualStyleBackColor = true;
            this.optKUF.Visible = false;
            // 
            // btnOdaberi
            // 
            this.btnOdaberi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOdaberi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnOdaberi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOdaberi.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnOdaberi.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOdaberi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnOdaberi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOdaberi.ForeColor = System.Drawing.Color.White;
            this.btnOdaberi.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOdaberi.Location = new System.Drawing.Point(666, 27);
            this.btnOdaberi.Name = "btnOdaberi";
            this.btnOdaberi.Size = new System.Drawing.Size(95, 23);
            this.btnOdaberi.TabIndex = 36;
            this.btnOdaberi.Text = "Vrati u obradu";
            this.btnOdaberi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOdaberi.UseVisualStyleBackColor = false;
            this.btnOdaberi.Visible = false;
            // 
            // btnIzmjena
            // 
            this.btnIzmjena.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnIzmjena.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnIzmjena.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnIzmjena.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIzmjena.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnIzmjena.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnIzmjena.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnIzmjena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIzmjena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnIzmjena.ForeColor = System.Drawing.Color.White;
            this.btnIzmjena.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIzmjena.Location = new System.Drawing.Point(581, 26);
            this.btnIzmjena.Name = "btnIzmjena";
            this.btnIzmjena.Size = new System.Drawing.Size(48, 23);
            this.btnIzmjena.TabIndex = 99;
            this.btnIzmjena.Text = "&Izmjena (End)";
            this.btnIzmjena.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIzmjena.UseVisualStyleBackColor = false;
            this.btnIzmjena.Visible = false;
            // 
            // btnDodavanje
            // 
            this.btnDodavanje.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDodavanje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnDodavanje.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDodavanje.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDodavanje.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnDodavanje.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDodavanje.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnDodavanje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDodavanje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnDodavanje.ForeColor = System.Drawing.Color.White;
            this.btnDodavanje.Location = new System.Drawing.Point(527, 26);
            this.btnDodavanje.Name = "btnDodavanje";
            this.btnDodavanje.Size = new System.Drawing.Size(48, 23);
            this.btnDodavanje.TabIndex = 98;
            this.btnDodavanje.Text = "&Dodavanje (Ins)";
            this.btnDodavanje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDodavanje.UseVisualStyleBackColor = false;
            this.btnDodavanje.Visible = false;
            // 
            // btnDetalji
            // 
            this.btnDetalji.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDetalji.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnDetalji.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDetalji.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnDetalji.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDetalji.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnDetalji.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDetalji.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnDetalji.ForeColor = System.Drawing.Color.White;
            this.btnDetalji.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDetalji.Location = new System.Drawing.Point(426, 26);
            this.btnDetalji.Name = "btnDetalji";
            this.btnDetalji.Size = new System.Drawing.Size(95, 23);
            this.btnDetalji.TabIndex = 36;
            this.btnDetalji.Text = "D&etalji (Home)";
            this.btnDetalji.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDetalji.UseVisualStyleBackColor = false;
            this.btnDetalji.Visible = false;
            // 
            // btnBrisanje
            // 
            this.btnBrisanje.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBrisanje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnBrisanje.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrisanje.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnBrisanje.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBrisanje.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnBrisanje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrisanje.ForeColor = System.Drawing.Color.White;
            this.btnBrisanje.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrisanje.Location = new System.Drawing.Point(635, 25);
            this.btnBrisanje.Name = "btnBrisanje";
            this.btnBrisanje.Size = new System.Drawing.Size(48, 23);
            this.btnBrisanje.TabIndex = 33;
            this.btnBrisanje.Text = "&Brisanje (Del)";
            this.btnBrisanje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrisanje.UseVisualStyleBackColor = false;
            this.btnBrisanje.Visible = false;
            // 
            // pnlSlika
            // 
            this.pnlSlika.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.pnlSlika.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSlika.Controls.Add(this.panel1);
            this.pnlSlika.Controls.Add(this.Label1);
            this.pnlSlika.Controls.Add(this.optKIF);
            this.pnlSlika.Controls.Add(this.PictureBox1);
            this.pnlSlika.Controls.Add(this.btnOdaberi);
            this.pnlSlika.Controls.Add(this.optKUF);
            this.pnlSlika.Controls.Add(this.btnDodavanje);
            this.pnlSlika.Controls.Add(this.btnBrisanje);
            this.pnlSlika.Controls.Add(this.btnDetalji);
            this.pnlSlika.Controls.Add(this.btnIzmjena);
            this.pnlSlika.Location = new System.Drawing.Point(0, -5);
            this.pnlSlika.Name = "pnlSlika";
            this.pnlSlika.Size = new System.Drawing.Size(772, 116);
            this.pnlSlika.TabIndex = 47;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(61, 27);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(112, 18);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Bruto bilans";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = global::Poslovanje.Properties.Resources.knjiga1;
            this.PictureBox1.Location = new System.Drawing.Point(4, 0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(52, 62);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // grid1
            // 
            this.grid1.AllowRowSizing = C1.Win.C1TrueDBGrid.RowSizingEnum.None;
            this.grid1.AllowUpdate = false;
            this.grid1.BackColor = System.Drawing.Color.White;
            this.grid1.ChildGrid = this.grid2;
            this.grid1.ColumnFooters = true;
            this.grid1.FlatStyle = C1.Win.C1TrueDBGrid.FlatModeEnum.Flat;
            this.grid1.GroupByCaption = "Drag a column header here to group by that column";
            this.grid1.Images.Add(((System.Drawing.Image)(resources.GetObject("grid1.Images"))));
            this.grid1.Location = new System.Drawing.Point(-1, 111);
            this.grid1.MarqueeStyle = C1.Win.C1TrueDBGrid.MarqueeEnum.HighlightRow;
            this.grid1.Name = "grid1";
            this.grid1.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.grid1.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.grid1.PreviewInfo.ZoomFactor = 75D;
            this.grid1.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("grid1.PrintInfo.PageSettings")));
            this.grid1.RowDivider.Color = System.Drawing.Color.LightSlateGray;
            this.grid1.RowDivider.Style = C1.Win.C1TrueDBGrid.LineStyleEnum.Single;
            this.grid1.RowHeight = 17;
            this.grid1.RowSubDividerColor = System.Drawing.Color.LightSlateGray;
            this.grid1.Size = new System.Drawing.Size(774, 177);
            this.grid1.TabIndex = 48;
            this.grid1.Text = "c1TrueDBGrid1";
            this.grid1.HeadClick += new C1.Win.C1TrueDBGrid.ColEventHandler(this.grid1_HeadClick);
            this.grid1.PropBag = resources.GetString("grid1.PropBag");
            // 
            // grid2
            // 
            this.grid2.AllowRowSizing = C1.Win.C1TrueDBGrid.RowSizingEnum.None;
            this.grid2.AllowUpdate = false;
            this.grid2.BackColor = System.Drawing.Color.White;
            this.grid2.ColumnFooters = true;
            this.grid2.GroupByCaption = "Drag a column header here to group by that column";
            this.grid2.Images.Add(((System.Drawing.Image)(resources.GetObject("grid2.Images"))));
            this.grid2.Location = new System.Drawing.Point(13, 153);
            this.grid2.MarqueeStyle = C1.Win.C1TrueDBGrid.MarqueeEnum.HighlightRow;
            this.grid2.Name = "grid2";
            this.grid2.PreviewInfo.Location = new System.Drawing.Point(0, 0);
            this.grid2.PreviewInfo.Size = new System.Drawing.Size(0, 0);
            this.grid2.PreviewInfo.ZoomFactor = 75D;
            this.grid2.PrintInfo.PageSettings = ((System.Drawing.Printing.PageSettings)(resources.GetObject("grid2.PrintInfo.PageSettings")));
            this.grid2.RowDivider.Color = System.Drawing.Color.LightSlateGray;
            this.grid2.RowDivider.Style = C1.Win.C1TrueDBGrid.LineStyleEnum.Single;
            this.grid2.RowHeight = 17;
            this.grid2.RowSubDividerColor = System.Drawing.Color.LightSlateGray;
            this.grid2.Size = new System.Drawing.Size(759, 102);
            this.grid2.TabIndex = 49;
            this.grid2.TabStop = false;
            this.grid2.Text = "c1TrueDBGrid1";
            this.grid2.PropBag = resources.GetString("grid2.PropBag");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.štampaBrutoBilansaToolStripMenuItem,
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(252, 70);
            // 
            // štampaBrutoBilansaToolStripMenuItem
            // 
            this.štampaBrutoBilansaToolStripMenuItem.Name = "štampaBrutoBilansaToolStripMenuItem";
            this.štampaBrutoBilansaToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.štampaBrutoBilansaToolStripMenuItem.Text = "Štampa bruto bilansa";
            this.štampaBrutoBilansaToolStripMenuItem.Click += new System.EventHandler(this.štampaBrutoBilansaToolStripMenuItem_Click);
            // 
            // štampaBrutoBilansaNazivKontaToolStripMenuItem
            // 
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem.Name = "štampaBrutoBilansaNazivKontaToolStripMenuItem";
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem.Size = new System.Drawing.Size(251, 22);
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem.Text = "Štampa bruto bilansa - naziv konta";
            this.štampaBrutoBilansaNazivKontaToolStripMenuItem.Click += new System.EventHandler(this.štampaBrutoBilansaNazivKontaToolStripMenuItem_Click);
            // 
            // frmBrutoBilans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(773, 493);
            this.Controls.Add(this.grid2);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.pnlSlika);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.KeyPreview = true;
            this.Name = "frmBrutoBilans";
            this.Text = "Bruto bilans";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBrutoBilans_FormClosing);
            this.Load += new System.EventHandler(this.frmBrutoBilans_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBrutoBilans_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmBrutoBilans_KeyPress);
            this.Resize += new System.EventHandler(this.frmBrutoBilans_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlSlika.ResumeLayout(false);
            this.pnlSlika.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.OpenFileDialog dlg;
        private System.Windows.Forms.Button btnPrint;
        internal System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnIzmjena;
        private System.Windows.Forms.Button btnDodavanje;
        private System.Windows.Forms.Button btnDetalji;
        internal System.Windows.Forms.PictureBox PictureBox2;
        private System.Windows.Forms.Button btnOdaberi;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.Button btnBrisanje;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Panel pnlSlika;
        private System.Windows.Forms.RadioButton optKIF;
        private System.Windows.Forms.RadioButton optKUF;
        private System.Windows.Forms.DateTimePicker txtDatOd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker txtDatDo;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid grid1;
        private C1.Win.C1TrueDBGrid.C1TrueDBGrid grid2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem štampaBrutoBilansaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem štampaBrutoBilansaNazivKontaToolStripMenuItem;
    }
}