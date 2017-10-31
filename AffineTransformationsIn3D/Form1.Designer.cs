namespace AffineTransformationsIn3D
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
            AffineTransformationsIn3D.Primitives.Transformation transformation1 = new AffineTransformationsIn3D.Primitives.Transformation();
            AffineTransformationsIn3D.Primitives.Transformation transformation2 = new AffineTransformationsIn3D.Primitives.Transformation();
            AffineTransformationsIn3D.Primitives.Transformation transformation3 = new AffineTransformationsIn3D.Primitives.Transformation();
            AffineTransformationsIn3D.Primitives.Transformation transformation4 = new AffineTransformationsIn3D.Primitives.Transformation();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sceneView3 = new AffineTransformationsIn3D.SceneView();
            this.sceneView4 = new AffineTransformationsIn3D.SceneView();
            this.sceneView1 = new AffineTransformationsIn3D.SceneView();
            this.sceneView2 = new AffineTransformationsIn3D.SceneView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1354, 803);
            this.splitContainer1.SplitterDistance = 1029;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.sceneView3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.sceneView4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.sceneView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sceneView2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1029, 803);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(517, 401);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Изометрическая проекция";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 401);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Проекция ZOX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(517, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Проекция ZOY";
            // 
            // sceneView3
            // 
            this.sceneView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneView3.Location = new System.Drawing.Point(3, 417);
            this.sceneView3.Name = "sceneView3";
            this.sceneView3.Projection = transformation1;
            this.sceneView3.Scene = null;
            this.sceneView3.Size = new System.Drawing.Size(508, 383);
            this.sceneView3.TabIndex = 2;
            this.sceneView3.Text = "sceneView3";
            // 
            // sceneView4
            // 
            this.sceneView4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneView4.Location = new System.Drawing.Point(517, 417);
            this.sceneView4.Name = "sceneView4";
            this.sceneView4.Projection = transformation2;
            this.sceneView4.Scene = null;
            this.sceneView4.Size = new System.Drawing.Size(509, 383);
            this.sceneView4.TabIndex = 3;
            this.sceneView4.Text = "sceneView4";
            // 
            // sceneView1
            // 
            this.sceneView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneView1.Location = new System.Drawing.Point(3, 16);
            this.sceneView1.Name = "sceneView1";
            this.sceneView1.Projection = transformation3;
            this.sceneView1.Scene = null;
            this.sceneView1.Size = new System.Drawing.Size(508, 382);
            this.sceneView1.TabIndex = 0;
            this.sceneView1.Text = "sceneView1";
            // 
            // sceneView2
            // 
            this.sceneView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneView2.Location = new System.Drawing.Point(517, 16);
            this.sceneView2.Name = "sceneView2";
            this.sceneView2.Projection = transformation4;
            this.sceneView2.Scene = null;
            this.sceneView2.Size = new System.Drawing.Size(509, 382);
            this.sceneView2.TabIndex = 1;
            this.sceneView2.Text = "sceneView2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Проекция XOY";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 803);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Affine transformation in 3D";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SceneView sceneView4;
        private SceneView sceneView3;
        private SceneView sceneView2;
        private SceneView sceneView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

