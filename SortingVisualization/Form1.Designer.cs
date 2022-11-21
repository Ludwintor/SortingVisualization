namespace SortingVisualization
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._sortButton = new System.Windows.Forms.Button();
            this._createButton = new System.Windows.Forms.Button();
            this._generationComboBox = new System.Windows.Forms.ComboBox();
            this._elementsCounter = new System.Windows.Forms.NumericUpDown();
            this._generationTypeLabel = new System.Windows.Forms.Label();
            this._delayLabel = new System.Windows.Forms.Label();
            this._delayCounter = new System.Windows.Forms.NumericUpDown();
            this._countLabel = new System.Windows.Forms.Label();
            this._sortPicture = new System.Windows.Forms.PictureBox();
            this._displayComboBox = new System.Windows.Forms.ComboBox();
            this._displayTypeLabel = new System.Windows.Forms.Label();
            this._reverseCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._elementsCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._delayCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._sortPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // _sortButton
            // 
            this._sortButton.Location = new System.Drawing.Point(770, 27);
            this._sortButton.Name = "_sortButton";
            this._sortButton.Size = new System.Drawing.Size(131, 34);
            this._sortButton.TabIndex = 1;
            this._sortButton.Text = "Sort";
            this._sortButton.UseVisualStyleBackColor = true;
            this._sortButton.Click += new System.EventHandler(this.SortButton_Click);
            // 
            // _createButton
            // 
            this._createButton.Location = new System.Drawing.Point(620, 27);
            this._createButton.Name = "_createButton";
            this._createButton.Size = new System.Drawing.Size(131, 34);
            this._createButton.TabIndex = 2;
            this._createButton.Text = "Create array";
            this._createButton.UseVisualStyleBackColor = true;
            this._createButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // _generationComboBox
            // 
            this._generationComboBox.FormattingEnabled = true;
            this._generationComboBox.Location = new System.Drawing.Point(252, 34);
            this._generationComboBox.Name = "_generationComboBox";
            this._generationComboBox.Size = new System.Drawing.Size(142, 23);
            this._generationComboBox.TabIndex = 3;
            // 
            // _elementsCounter
            // 
            this._elementsCounter.Location = new System.Drawing.Point(413, 35);
            this._elementsCounter.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this._elementsCounter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._elementsCounter.Name = "_elementsCounter";
            this._elementsCounter.Size = new System.Drawing.Size(82, 23);
            this._elementsCounter.TabIndex = 4;
            this._elementsCounter.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _generationTypeLabel
            // 
            this._generationTypeLabel.AutoSize = true;
            this._generationTypeLabel.Location = new System.Drawing.Point(252, 16);
            this._generationTypeLabel.Name = "_generationTypeLabel";
            this._generationTypeLabel.Size = new System.Drawing.Size(96, 15);
            this._generationTypeLabel.TabIndex = 6;
            this._generationTypeLabel.Text = "Array Generation";
            // 
            // _delayLabel
            // 
            this._delayLabel.AutoSize = true;
            this._delayLabel.Location = new System.Drawing.Point(510, 16);
            this._delayLabel.Name = "_delayLabel";
            this._delayLabel.Size = new System.Drawing.Size(36, 15);
            this._delayLabel.TabIndex = 7;
            this._delayLabel.Text = "Delay";
            // 
            // _delayCounter
            // 
            this._delayCounter.Location = new System.Drawing.Point(510, 35);
            this._delayCounter.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._delayCounter.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._delayCounter.Name = "_delayCounter";
            this._delayCounter.Size = new System.Drawing.Size(82, 23);
            this._delayCounter.TabIndex = 8;
            this._delayCounter.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // _countLabel
            // 
            this._countLabel.AutoSize = true;
            this._countLabel.Location = new System.Drawing.Point(413, 16);
            this._countLabel.Name = "_countLabel";
            this._countLabel.Size = new System.Drawing.Size(40, 15);
            this._countLabel.TabIndex = 9;
            this._countLabel.Text = "Count";
            // 
            // _sortPicture
            // 
            this._sortPicture.Location = new System.Drawing.Point(12, 67);
            this._sortPicture.Name = "_sortPicture";
            this._sortPicture.Size = new System.Drawing.Size(889, 669);
            this._sortPicture.TabIndex = 10;
            this._sortPicture.TabStop = false;
            this._sortPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.SortPicture_Paint);
            // 
            // _displayComboBox
            // 
            this._displayComboBox.FormattingEnabled = true;
            this._displayComboBox.Location = new System.Drawing.Point(92, 34);
            this._displayComboBox.Name = "_displayComboBox";
            this._displayComboBox.Size = new System.Drawing.Size(142, 23);
            this._displayComboBox.TabIndex = 11;
            // 
            // _displayTypeLabel
            // 
            this._displayTypeLabel.AutoSize = true;
            this._displayTypeLabel.Location = new System.Drawing.Point(92, 16);
            this._displayTypeLabel.Name = "_displayTypeLabel";
            this._displayTypeLabel.Size = new System.Drawing.Size(72, 15);
            this._displayTypeLabel.TabIndex = 12;
            this._displayTypeLabel.Text = "Display Type";
            // 
            // _reverseCheckBox
            // 
            this._reverseCheckBox.AutoSize = true;
            this._reverseCheckBox.Location = new System.Drawing.Point(774, 7);
            this._reverseCheckBox.Name = "_reverseCheckBox";
            this._reverseCheckBox.Size = new System.Drawing.Size(66, 19);
            this._reverseCheckBox.TabIndex = 13;
            this._reverseCheckBox.Text = "Reverse";
            this._reverseCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 748);
            this.Controls.Add(this._reverseCheckBox);
            this.Controls.Add(this._displayTypeLabel);
            this.Controls.Add(this._displayComboBox);
            this.Controls.Add(this._sortPicture);
            this.Controls.Add(this._countLabel);
            this.Controls.Add(this._delayCounter);
            this.Controls.Add(this._delayLabel);
            this.Controls.Add(this._generationTypeLabel);
            this.Controls.Add(this._elementsCounter);
            this.Controls.Add(this._generationComboBox);
            this.Controls.Add(this._createButton);
            this.Controls.Add(this._sortButton);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this._elementsCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._delayCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._sortPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button _sortButton;
        private Button _createButton;
        private ComboBox _generationComboBox;
        private NumericUpDown _elementsCounter;
        private Label _generationTypeLabel;
        private Label _delayLabel;
        private NumericUpDown _delayCounter;
        private Label _countLabel;
        private PictureBox _sortPicture;
        private ComboBox _displayComboBox;
        private Label _displayTypeLabel;
        private CheckBox _reverseCheckBox;
    }
}