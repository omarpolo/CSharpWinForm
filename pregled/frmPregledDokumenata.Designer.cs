namespace Poslovanje.forme
{
    partial class frmPregledDokumenata
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPregledDokumenata));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlSlika = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkSve = new System.Windows.Forms.CheckBox();
            this.optDokumentiIzlaza = new System.Windows.Forms.RadioButton();
            this.optDokumentiUlaza = new System.Windows.Forms.RadioButton();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnReklamacija = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.btnBrisanje = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.grid1 = new System.Windows.Forms.DataGridView();
            this.n = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.racun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.br = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zak = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gridDetalji = new System.Windows.Forms.DataGridView();
            this.Sifra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Naziv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cijena = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iznos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otkjlučajRačunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zaključiRačunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblMoveDetalje = new System.Windows.Forms.Panel();
            this.btnCloseDetalje = new System.Windows.Forms.Button();
            this.Label12 = new System.Windows.Forms.Label();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.DetaljiBox = new System.Windows.Forms.GroupBox();
            this.cmbPlacanje = new System.Windows.Forms.ComboBox();
            this.btnKupacLookUp = new System.Windows.Forms.Button();
            this.txtKupacIme = new System.Windows.Forms.TextBox();
            this.txtKupac = new System.Windows.Forms.TextBox();
            this.btnOdustaniOdDetalja = new System.Windows.Forms.Button();
            this.btnUpisi = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.pnlSlika.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalji)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.lblMoveDetalje.SuspendLayout();
            this.DetaljiBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSlika
            // 
            this.pnlSlika.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.pnlSlika.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlSlika.Controls.Add(this.panel1);
            this.pnlSlika.Controls.Add(this.Label1);
            this.pnlSlika.Controls.Add(this.PictureBox1);
            this.pnlSlika.Location = new System.Drawing.Point(0, -5);
            this.pnlSlika.Name = "pnlSlika";
            this.pnlSlika.Size = new System.Drawing.Size(772, 116);
            this.pnlSlika.TabIndex = 48;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.chkSve);
            this.panel1.Controls.Add(this.optDokumentiIzlaza);
            this.panel1.Controls.Add(this.optDokumentiUlaza);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnReklamacija);
            this.panel1.Controls.Add(this.btnOdustani);
            this.panel1.Controls.Add(this.btnBrisanje);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(772, 52);
            this.panel1.TabIndex = 35;
            // 
            // chkSve
            // 
            this.chkSve.AutoSize = true;
            this.chkSve.ForeColor = System.Drawing.Color.White;
            this.chkSve.Location = new System.Drawing.Point(269, 33);
            this.chkSve.Name = "chkSve";
            this.chkSve.Size = new System.Drawing.Size(138, 17);
            this.chkSve.TabIndex = 103;
            this.chkSve.Text = "selektuj sve dokumente";
            this.chkSve.UseVisualStyleBackColor = true;
            this.chkSve.Click += new System.EventHandler(this.chkSve_Click);
            // 
            // optDokumentiIzlaza
            // 
            this.optDokumentiIzlaza.AutoSize = true;
            this.optDokumentiIzlaza.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.optDokumentiIzlaza.ForeColor = System.Drawing.Color.White;
            this.optDokumentiIzlaza.Location = new System.Drawing.Point(127, 32);
            this.optDokumentiIzlaza.Name = "optDokumentiIzlaza";
            this.optDokumentiIzlaza.Size = new System.Drawing.Size(105, 17);
            this.optDokumentiIzlaza.TabIndex = 102;
            this.optDokumentiIzlaza.TabStop = true;
            this.optDokumentiIzlaza.Text = "Dokumenti izlaza";
            this.optDokumentiIzlaza.UseVisualStyleBackColor = true;
            this.optDokumentiIzlaza.Click += new System.EventHandler(this.optDokumentiIzlaza_Click);
            // 
            // optDokumentiUlaza
            // 
            this.optDokumentiUlaza.AutoSize = true;
            this.optDokumentiUlaza.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.optDokumentiUlaza.ForeColor = System.Drawing.Color.White;
            this.optDokumentiUlaza.Location = new System.Drawing.Point(7, 32);
            this.optDokumentiUlaza.Name = "optDokumentiUlaza";
            this.optDokumentiUlaza.Size = new System.Drawing.Size(104, 17);
            this.optDokumentiUlaza.TabIndex = 101;
            this.optDokumentiUlaza.TabStop = true;
            this.optDokumentiUlaza.Text = "Dokumenti ulaza";
            this.optDokumentiUlaza.UseVisualStyleBackColor = true;
            this.optDokumentiUlaza.Click += new System.EventHandler(this.optDokumentiUlaza_Click);
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
            this.btnPrint.Location = new System.Drawing.Point(200, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 23);
            this.btnPrint.TabIndex = 100;
            this.btnPrint.Text = "Štam&pa";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnReklamacija
            // 
            this.btnReklamacija.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReklamacija.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnReklamacija.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReklamacija.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnReklamacija.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReklamacija.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnReklamacija.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReklamacija.ForeColor = System.Drawing.Color.White;
            this.btnReklamacija.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReklamacija.Location = new System.Drawing.Point(104, 4);
            this.btnReklamacija.Name = "btnReklamacija";
            this.btnReklamacija.Size = new System.Drawing.Size(95, 23);
            this.btnReklamacija.TabIndex = 36;
            this.btnReklamacija.Text = "Reklamacija";
            this.btnReklamacija.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReklamacija.UseVisualStyleBackColor = false;
            this.btnReklamacija.Click += new System.EventHandler(this.btnReklamacija_Click);
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
            this.btnBrisanje.Location = new System.Drawing.Point(8, 4);
            this.btnBrisanje.Name = "btnBrisanje";
            this.btnBrisanje.Size = new System.Drawing.Size(95, 23);
            this.btnBrisanje.TabIndex = 33;
            this.btnBrisanje.Text = "&Brisanje (Del)";
            this.btnBrisanje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrisanje.UseVisualStyleBackColor = false;
            this.btnBrisanje.Click += new System.EventHandler(this.btnBrisanje_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Label1.ForeColor = System.Drawing.Color.White;
            this.Label1.Location = new System.Drawing.Point(59, 34);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(191, 18);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Pregled dokumenata";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(4, 1);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(52, 62);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 0;
            this.PictureBox1.TabStop = false;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.Color.White;
            this.grid1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.ColumnHeadersHeight = 34;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.n,
            this.racun,
            this.br,
            this.fis,
            this.zak});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid1.DefaultCellStyle = dataGridViewCellStyle2;
            this.grid1.EnableHeadersVisualStyles = false;
            this.grid1.GridColor = System.Drawing.Color.LightSlateGray;
            this.grid1.Location = new System.Drawing.Point(0, 112);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.Height = 18;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(304, 490);
            this.grid1.TabIndex = 50;
            this.grid1.SelectionChanged += new System.EventHandler(this.grid1_SelectionChanged);
            this.grid1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid1_MouseDown);
            // 
            // n
            // 
            this.n.DataPropertyName = "n";
            this.n.HeaderText = "#";
            this.n.Name = "n";
            this.n.Width = 30;
            // 
            // racun
            // 
            this.racun.DataPropertyName = "racun";
            this.racun.HeaderText = "Dne.br.";
            this.racun.Name = "racun";
            this.racun.ReadOnly = true;
            this.racun.Width = 70;
            // 
            // br
            // 
            this.br.DataPropertyName = "br";
            this.br.HeaderText = "Račun";
            this.br.Name = "br";
            this.br.ReadOnly = true;
            this.br.Width = 70;
            // 
            // fis
            // 
            this.fis.DataPropertyName = "fis";
            this.fis.HeaderText = "Fis.br.";
            this.fis.Name = "fis";
            this.fis.ReadOnly = true;
            this.fis.Width = 70;
            // 
            // zak
            // 
            this.zak.DataPropertyName = "zak";
            this.zak.FalseValue = "";
            this.zak.HeaderText = "Z";
            this.zak.Name = "zak";
            this.zak.ReadOnly = true;
            this.zak.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.zak.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.zak.TrueValue = "";
            this.zak.Width = 30;
            // 
            // gridDetalji
            // 
            this.gridDetalji.AllowUserToAddRows = false;
            this.gridDetalji.AllowUserToDeleteRows = false;
            this.gridDetalji.AllowUserToResizeRows = false;
            this.gridDetalji.BackgroundColor = System.Drawing.Color.White;
            this.gridDetalji.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetalji.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridDetalji.ColumnHeadersHeight = 34;
            this.gridDetalji.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridDetalji.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sifra,
            this.Naziv,
            this.kol,
            this.cijena,
            this.iznos});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDetalji.DefaultCellStyle = dataGridViewCellStyle4;
            this.gridDetalji.EnableHeadersVisualStyles = false;
            this.gridDetalji.GridColor = System.Drawing.Color.LightSlateGray;
            this.gridDetalji.Location = new System.Drawing.Point(305, 112);
            this.gridDetalji.MultiSelect = false;
            this.gridDetalji.Name = "gridDetalji";
            this.gridDetalji.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetalji.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gridDetalji.RowHeadersVisible = false;
            this.gridDetalji.RowTemplate.Height = 17;
            this.gridDetalji.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetalji.Size = new System.Drawing.Size(464, 490);
            this.gridDetalji.TabIndex = 51;
            // 
            // Sifra
            // 
            this.Sifra.DataPropertyName = "sifra";
            this.Sifra.HeaderText = "Šifra";
            this.Sifra.Name = "Sifra";
            this.Sifra.ReadOnly = true;
            this.Sifra.Visible = false;
            // 
            // Naziv
            // 
            this.Naziv.DataPropertyName = "nazivrobe";
            this.Naziv.HeaderText = "Naziv";
            this.Naziv.Name = "Naziv";
            this.Naziv.ReadOnly = true;
            this.Naziv.Width = 200;
            // 
            // kol
            // 
            this.kol.DataPropertyName = "kol";
            this.kol.HeaderText = "Kol";
            this.kol.Name = "kol";
            this.kol.ReadOnly = true;
            this.kol.Width = 50;
            // 
            // cijena
            // 
            this.cijena.DataPropertyName = "cijena";
            this.cijena.HeaderText = "Cijena";
            this.cijena.Name = "cijena";
            this.cijena.ReadOnly = true;
            this.cijena.Width = 70;
            // 
            // iznos
            // 
            this.iznos.DataPropertyName = "iznos";
            this.iznos.HeaderText = "Iznos";
            this.iznos.Name = "iznos";
            this.iznos.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem,
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem,
            this.otkjlučajRačunToolStripMenuItem,
            this.zaključiRačunToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(248, 114);
            // 
            // pojedinačnoBrisanjeDokumenataToolStripMenuItem
            // 
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem.Name = "pojedinačnoBrisanjeDokumenataToolStripMenuItem";
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem.Text = "Pojedinačno brisanje dokumenata";
            this.pojedinačnoBrisanjeDokumenataToolStripMenuItem.Click += new System.EventHandler(this.pojedinačnoBrisanjeDokumenataToolStripMenuItem_Click);
            // 
            // štampaMaloprodajnogRačunaA4ToolStripMenuItem
            // 
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem.Name = "štampaMaloprodajnogRačunaA4ToolStripMenuItem";
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem.Text = "Štampa maloprodajnog računa A4";
            this.štampaMaloprodajnogRačunaA4ToolStripMenuItem.Click += new System.EventHandler(this.štampaMaloprodajnogRačunaA4ToolStripMenuItem_Click);
            // 
            // otkjlučajRačunToolStripMenuItem
            // 
            this.otkjlučajRačunToolStripMenuItem.Name = "otkjlučajRačunToolStripMenuItem";
            this.otkjlučajRačunToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.otkjlučajRačunToolStripMenuItem.Text = "Otkjlučaj račun";
            this.otkjlučajRačunToolStripMenuItem.Click += new System.EventHandler(this.otkjlučajRačunToolStripMenuItem_Click);
            // 
            // zaključiRačunToolStripMenuItem
            // 
            this.zaključiRačunToolStripMenuItem.Name = "zaključiRačunToolStripMenuItem";
            this.zaključiRačunToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.zaključiRačunToolStripMenuItem.Text = "Zaključi račun";
            this.zaključiRačunToolStripMenuItem.Click += new System.EventHandler(this.zaključiRačunToolStripMenuItem_Click);
            // 
            // lblMoveDetalje
            // 
            this.lblMoveDetalje.AllowDrop = true;
            this.lblMoveDetalje.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblMoveDetalje.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMoveDetalje.Controls.Add(this.btnCloseDetalje);
            this.lblMoveDetalje.Controls.Add(this.Label12);
            this.lblMoveDetalje.Controls.Add(this.Panel2);
            this.lblMoveDetalje.Controls.Add(this.DetaljiBox);
            this.lblMoveDetalje.ForeColor = System.Drawing.Color.White;
            this.lblMoveDetalje.Location = new System.Drawing.Point(167, 236);
            this.lblMoveDetalje.Name = "lblMoveDetalje";
            this.lblMoveDetalje.Size = new System.Drawing.Size(436, 130);
            this.lblMoveDetalje.TabIndex = 52;
            this.lblMoveDetalje.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblMoveDetalje_MouseDown);
            this.lblMoveDetalje.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblMoveDetalje_MouseMove);
            this.lblMoveDetalje.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblMoveDetalje_MouseUp);
            // 
            // btnCloseDetalje
            // 
            this.btnCloseDetalje.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCloseDetalje.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseDetalje.BackgroundImage = global::Poslovanje.Properties.Resources.CloseButton__1_;
            this.btnCloseDetalje.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseDetalje.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnCloseDetalje.FlatAppearance.BorderSize = 0;
            this.btnCloseDetalje.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCloseDetalje.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnCloseDetalje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseDetalje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCloseDetalje.ForeColor = System.Drawing.Color.White;
            this.btnCloseDetalje.Location = new System.Drawing.Point(396, 5);
            this.btnCloseDetalje.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseDetalje.Name = "btnCloseDetalje";
            this.btnCloseDetalje.Size = new System.Drawing.Size(28, 27);
            this.btnCloseDetalje.TabIndex = 102;
            this.btnCloseDetalje.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseDetalje.UseVisualStyleBackColor = false;
            this.btnCloseDetalje.Click += new System.EventHandler(this.btnCloseDetalje_Click);
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.BackColor = System.Drawing.Color.Transparent;
            this.Label12.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(49)))), ((int)(((byte)(73)))));
            this.Label12.Location = new System.Drawing.Point(242, 6);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(156, 23);
            this.Label12.TabIndex = 99;
            this.Label12.Text = "Zaključivanje";
            // 
            // Panel2
            // 
            this.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel2.Enabled = false;
            this.Panel2.ForeColor = System.Drawing.Color.White;
            this.Panel2.Location = new System.Drawing.Point(3, 3);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(236, 29);
            this.Panel2.TabIndex = 98;
            // 
            // DetaljiBox
            // 
            this.DetaljiBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DetaljiBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.DetaljiBox.Controls.Add(this.cmbPlacanje);
            this.DetaljiBox.Controls.Add(this.btnKupacLookUp);
            this.DetaljiBox.Controls.Add(this.txtKupacIme);
            this.DetaljiBox.Controls.Add(this.txtKupac);
            this.DetaljiBox.Controls.Add(this.btnOdustaniOdDetalja);
            this.DetaljiBox.Controls.Add(this.btnUpisi);
            this.DetaljiBox.Controls.Add(this.Label2);
            this.DetaljiBox.Controls.Add(this.Label3);
            this.DetaljiBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DetaljiBox.ForeColor = System.Drawing.Color.Black;
            this.DetaljiBox.Location = new System.Drawing.Point(4, 32);
            this.DetaljiBox.Name = "DetaljiBox";
            this.DetaljiBox.Size = new System.Drawing.Size(426, 93);
            this.DetaljiBox.TabIndex = 49;
            this.DetaljiBox.TabStop = false;
            // 
            // cmbPlacanje
            // 
            this.cmbPlacanje.FormattingEnabled = true;
            this.cmbPlacanje.Location = new System.Drawing.Point(68, 33);
            this.cmbPlacanje.Name = "cmbPlacanje";
            this.cmbPlacanje.Size = new System.Drawing.Size(119, 21);
            this.cmbPlacanje.TabIndex = 97;
            this.cmbPlacanje.SelectedValueChanged += new System.EventHandler(this.cmbPlacanje_SelectedValueChanged);
            // 
            // btnKupacLookUp
            // 
            this.btnKupacLookUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnKupacLookUp.Location = new System.Drawing.Point(159, 11);
            this.btnKupacLookUp.Name = "btnKupacLookUp";
            this.btnKupacLookUp.Size = new System.Drawing.Size(25, 20);
            this.btnKupacLookUp.TabIndex = 100;
            this.btnKupacLookUp.Text = "...";
            this.btnKupacLookUp.UseVisualStyleBackColor = true;
            this.btnKupacLookUp.Click += new System.EventHandler(this.btnKupacLookUp_Click);
            // 
            // txtKupacIme
            // 
            this.txtKupacIme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtKupacIme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKupacIme.Enabled = false;
            this.txtKupacIme.Location = new System.Drawing.Point(186, 11);
            this.txtKupacIme.Name = "txtKupacIme";
            this.txtKupacIme.Size = new System.Drawing.Size(233, 20);
            this.txtKupacIme.TabIndex = 97;
            // 
            // txtKupac
            // 
            this.txtKupac.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKupac.Location = new System.Drawing.Point(68, 11);
            this.txtKupac.Name = "txtKupac";
            this.txtKupac.Size = new System.Drawing.Size(92, 20);
            this.txtKupac.TabIndex = 96;
            this.txtKupac.TextChanged += new System.EventHandler(this.txtKupac_TextChanged);
            this.txtKupac.Validated += new System.EventHandler(this.txtKupac_Validated);
            // 
            // btnOdustaniOdDetalja
            // 
            this.btnOdustaniOdDetalja.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOdustaniOdDetalja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(79)))), ((int)(((byte)(112)))));
            this.btnOdustaniOdDetalja.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOdustaniOdDetalja.FlatAppearance.BorderColor = System.Drawing.Color.LightSlateGray;
            this.btnOdustaniOdDetalja.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOdustaniOdDetalja.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.btnOdustaniOdDetalja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOdustaniOdDetalja.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOdustaniOdDetalja.ForeColor = System.Drawing.Color.White;
            this.btnOdustaniOdDetalja.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOdustaniOdDetalja.Location = new System.Drawing.Point(298, 62);
            this.btnOdustaniOdDetalja.Name = "btnOdustaniOdDetalja";
            this.btnOdustaniOdDetalja.Size = new System.Drawing.Size(93, 25);
            this.btnOdustaniOdDetalja.TabIndex = 95;
            this.btnOdustaniOdDetalja.Text = "&Odustani";
            this.btnOdustaniOdDetalja.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOdustaniOdDetalja.UseVisualStyleBackColor = false;
            this.btnOdustaniOdDetalja.Click += new System.EventHandler(this.btnOdustaniOdDetalja_Click);
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
            this.btnUpisi.Location = new System.Drawing.Point(198, 62);
            this.btnUpisi.Name = "btnUpisi";
            this.btnUpisi.Size = new System.Drawing.Size(93, 25);
            this.btnUpisi.TabIndex = 98;
            this.btnUpisi.Text = "&Upiši";
            this.btnUpisi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpisi.UseVisualStyleBackColor = false;
            this.btnUpisi.Click += new System.EventHandler(this.btnUpisi_Click);
            this.btnUpisi.Enter += new System.EventHandler(this.btnUpisi_Enter);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(4, 38);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(54, 13);
            this.Label2.TabIndex = 58;
            this.Label2.Text = "Plaćanje :";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(4, 15);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(34, 13);
            this.Label3.TabIndex = 57;
            this.Label3.Text = "Šifra :";
            // 
            // frmPregledDokumenata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(770, 603);
            this.Controls.Add(this.lblMoveDetalje);
            this.Controls.Add(this.gridDetalji);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.pnlSlika);
            this.KeyPreview = true;
            this.Name = "frmPregledDokumenata";
            this.Text = "Pregled dokumenata";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPregledDokumenata_FormClosing);
            this.Load += new System.EventHandler(this.frmPregledDokumenata_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPregledDokumenata_KeyDown);
            this.Resize += new System.EventHandler(this.frmPregledDokumenata_Resize);
            this.pnlSlika.ResumeLayout(false);
            this.pnlSlika.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalji)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.lblMoveDetalje.ResumeLayout(false);
            this.lblMoveDetalje.PerformLayout();
            this.DetaljiBox.ResumeLayout(false);
            this.DetaljiBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel pnlSlika;
        internal System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnReklamacija;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.Button btnBrisanje;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.RadioButton optDokumentiIzlaza;
        private System.Windows.Forms.RadioButton optDokumentiUlaza;
        private System.Windows.Forms.DataGridView grid1;
        internal System.Windows.Forms.DataGridView gridDetalji;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sifra;
        private System.Windows.Forms.DataGridViewTextBoxColumn Naziv;
        private System.Windows.Forms.DataGridViewTextBoxColumn kol;
        private System.Windows.Forms.DataGridViewTextBoxColumn cijena;
        private System.Windows.Forms.DataGridViewTextBoxColumn iznos;
        private System.Windows.Forms.CheckBox chkSve;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pojedinačnoBrisanjeDokumenataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem štampaMaloprodajnogRačunaA4ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn n;
        private System.Windows.Forms.DataGridViewTextBoxColumn racun;
        private System.Windows.Forms.DataGridViewTextBoxColumn br;
        private System.Windows.Forms.DataGridViewTextBoxColumn fis;
        private System.Windows.Forms.DataGridViewCheckBoxColumn zak;
        private System.Windows.Forms.ToolStripMenuItem otkjlučajRačunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zaključiRačunToolStripMenuItem;
        internal System.Windows.Forms.Panel lblMoveDetalje;
        private System.Windows.Forms.Button btnCloseDetalje;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.Panel Panel2;
        internal System.Windows.Forms.GroupBox DetaljiBox;
        private System.Windows.Forms.Button btnOdustaniOdDetalja;
        private System.Windows.Forms.Button btnUpisi;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Button btnKupacLookUp;
        internal System.Windows.Forms.TextBox txtKupacIme;
        internal System.Windows.Forms.TextBox txtKupac;
        private System.Windows.Forms.ComboBox cmbPlacanje;
    }
}