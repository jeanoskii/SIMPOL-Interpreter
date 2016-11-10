namespace SIMPOL
{
    partial class simpolInterpreter
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
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnInterpret = new System.Windows.Forms.Button();
            this.dgvLexemeTable = new System.Windows.Forms.DataGridView();
            this.dgvSymbolTable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexemeTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbolTable)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(12, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(93, 14);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(310, 20);
            this.txtFile.TabIndex = 1;
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(12, 41);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(215, 338);
            this.txtOutput.TabIndex = 2;
            // 
            // btnInterpret
            // 
            this.btnInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInterpret.Location = new System.Drawing.Point(409, 12);
            this.btnInterpret.Name = "btnInterpret";
            this.btnInterpret.Size = new System.Drawing.Size(75, 23);
            this.btnInterpret.TabIndex = 3;
            this.btnInterpret.Text = "Interpret";
            this.btnInterpret.UseVisualStyleBackColor = true;
            this.btnInterpret.Click += new System.EventHandler(this.btnInterpret_Click);
            // 
            // dgvLexemeTable
            // 
            this.dgvLexemeTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLexemeTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLexemeTable.Location = new System.Drawing.Point(233, 41);
            this.dgvLexemeTable.Name = "dgvLexemeTable";
            this.dgvLexemeTable.Size = new System.Drawing.Size(251, 165);
            this.dgvLexemeTable.TabIndex = 4;
            // 
            // dgvSymbolTable
            // 
            this.dgvSymbolTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSymbolTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSymbolTable.Location = new System.Drawing.Point(233, 214);
            this.dgvSymbolTable.Name = "dgvSymbolTable";
            this.dgvSymbolTable.Size = new System.Drawing.Size(251, 165);
            this.dgvSymbolTable.TabIndex = 5;
            // 
            // simpolInterpreter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 391);
            this.Controls.Add(this.dgvSymbolTable);
            this.Controls.Add(this.dgvLexemeTable);
            this.Controls.Add(this.btnInterpret);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnOpenFile);
            this.Name = "simpolInterpreter";
            this.Text = "SIMPOL Interpreter";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLexemeTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbolTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnInterpret;
        private System.Windows.Forms.DataGridView dgvLexemeTable;
        private System.Windows.Forms.DataGridView dgvSymbolTable;
    }
}

