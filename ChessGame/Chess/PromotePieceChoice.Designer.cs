namespace Chess
{
    partial class PromotePieceChoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromotePieceChoice));
            this.promoteBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.queen = new System.Windows.Forms.RadioButton();
            this.rook = new System.Windows.Forms.RadioButton();
            this.bishop = new System.Windows.Forms.RadioButton();
            this.knight = new System.Windows.Forms.RadioButton();
            this.promote = new System.Windows.Forms.Button();
            this.promoteBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // promoteBox
            // 
            this.promoteBox.Controls.Add(this.knight);
            this.promoteBox.Controls.Add(this.bishop);
            this.promoteBox.Controls.Add(this.rook);
            this.promoteBox.Controls.Add(this.queen);
            this.promoteBox.Controls.Add(this.label1);
            this.promoteBox.Location = new System.Drawing.Point(12, 12);
            this.promoteBox.Name = "promoteBox";
            this.promoteBox.Size = new System.Drawing.Size(235, 220);
            this.promoteBox.TabIndex = 0;
            this.promoteBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Promote Piece";
            // 
            // queen
            // 
            this.queen.AutoSize = true;
            this.queen.Location = new System.Drawing.Point(33, 84);
            this.queen.Name = "queen";
            this.queen.Size = new System.Drawing.Size(57, 17);
            this.queen.TabIndex = 2;
            this.queen.TabStop = true;
            this.queen.Text = "Queen";
            this.queen.UseVisualStyleBackColor = true;
            // 
            // rook
            // 
            this.rook.AutoSize = true;
            this.rook.Location = new System.Drawing.Point(33, 116);
            this.rook.Name = "rook";
            this.rook.Size = new System.Drawing.Size(51, 17);
            this.rook.TabIndex = 3;
            this.rook.TabStop = true;
            this.rook.Text = "Rook";
            this.rook.UseVisualStyleBackColor = true;
            // 
            // bishop
            // 
            this.bishop.AutoSize = true;
            this.bishop.Location = new System.Drawing.Point(33, 150);
            this.bishop.Name = "bishop";
            this.bishop.Size = new System.Drawing.Size(57, 17);
            this.bishop.TabIndex = 4;
            this.bishop.TabStop = true;
            this.bishop.Text = "Bishop";
            this.bishop.UseVisualStyleBackColor = true;
            // 
            // knight
            // 
            this.knight.AutoSize = true;
            this.knight.Location = new System.Drawing.Point(33, 183);
            this.knight.Name = "knight";
            this.knight.Size = new System.Drawing.Size(55, 17);
            this.knight.TabIndex = 5;
            this.knight.TabStop = true;
            this.knight.Text = "Knight";
            this.knight.UseVisualStyleBackColor = true;
            // 
            // promote
            // 
            this.promote.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.promote.Location = new System.Drawing.Point(149, 247);
            this.promote.Name = "promote";
            this.promote.Size = new System.Drawing.Size(99, 41);
            this.promote.TabIndex = 1;
            this.promote.Text = "Promote";
            this.promote.UseVisualStyleBackColor = true;
            this.promote.Click += new System.EventHandler(this.promote_Click);
            // 
            // PromotePieceChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 298);
            this.Controls.Add(this.promote);
            this.Controls.Add(this.promoteBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotePieceChoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Promote";
            this.promoteBox.ResumeLayout(false);
            this.promoteBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox promoteBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton knight;
        private System.Windows.Forms.RadioButton bishop;
        private System.Windows.Forms.RadioButton rook;
        private System.Windows.Forms.RadioButton queen;
        private System.Windows.Forms.Button promote;
    }
}