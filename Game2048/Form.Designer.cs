namespace Game2048
{
    partial class Form
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

        private void InitializeForm() {
            InitializeComponent();
            Game = new GameScreen();
            Game.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            Game.BackColor = SystemColors.ControlDark;
            Game.Location = new System.Drawing.Point(0, 32);
            Game.Margin = new Padding(0);
            Game.Name = "GameScreen";

            Game.TabIndex = 0;
            Game.Size = new Size(ClientSize.Width, ClientSize.Height - 32);
            Controls.Add(Game);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FinishButton = new Button();
            ScoreText = new Label();
            BackButton = new Button();
            IncreaseCount = new Button();
            DecreaseCount = new Button();
            SuspendLayout();
            // 
            // FinishButton
            // 
            FinishButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FinishButton.BackgroundImageLayout = ImageLayout.None;
            FinishButton.FlatAppearance.BorderSize = 0;
            FinishButton.ForeColor = SystemColors.ActiveCaptionText;
            FinishButton.Location = new Point(242, 0);
            FinishButton.Margin = new Padding(0);
            FinishButton.Name = "FinishButton";
            FinishButton.Size = new Size(70, 23);
            FinishButton.TabIndex = 1;
            FinishButton.Text = "Restart";
            FinishButton.UseVisualStyleBackColor = true;
            FinishButton.Click += FinishButton_Click;
            // 
            // ScoreText
            // 
            ScoreText.AutoSize = true;
            ScoreText.Font = new Font("Segoe UI", 11F);
            ScoreText.Location = new Point(0, 0);
            ScoreText.Name = "ScoreText";
            ScoreText.Size = new Size(81, 20);
            ScoreText.TabIndex = 2;
            ScoreText.Text = "____________";
            // 
            // BackButton
            // 
            BackButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackButton.BackgroundImageLayout = ImageLayout.None;
            BackButton.FlatAppearance.BorderSize = 0;
            BackButton.ForeColor = SystemColors.ActiveCaptionText;
            BackButton.Location = new Point(192, 0);
            BackButton.Margin = new Padding(0);
            BackButton.Name = "BackButton";
            BackButton.Size = new Size(50, 23);
            BackButton.TabIndex = 3;
            BackButton.Text = "Back";
            BackButton.UseVisualStyleBackColor = true;
            BackButton.Click += BackButton_Click;
            // 
            // IncreaseCount
            // 
            IncreaseCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            IncreaseCount.BackgroundImageLayout = ImageLayout.None;
            IncreaseCount.FlatAppearance.BorderSize = 0;
            IncreaseCount.ForeColor = SystemColors.ActiveCaptionText;
            IncreaseCount.Location = new Point(172, 0);
            IncreaseCount.Margin = new Padding(0);
            IncreaseCount.Name = "IncreaseCount";
            IncreaseCount.Size = new Size(20, 23);
            IncreaseCount.TabIndex = 4;
            IncreaseCount.Text = "+";
            IncreaseCount.UseVisualStyleBackColor = true;
            IncreaseCount.Click += button1_Click;
            // 
            // DecreaseCount
            // 
            DecreaseCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DecreaseCount.BackgroundImageLayout = ImageLayout.None;
            DecreaseCount.FlatAppearance.BorderSize = 0;
            DecreaseCount.ForeColor = SystemColors.ActiveCaptionText;
            DecreaseCount.Location = new Point(152, 0);
            DecreaseCount.Margin = new Padding(0);
            DecreaseCount.Name = "DecreaseCount";
            DecreaseCount.Size = new Size(20, 23);
            DecreaseCount.TabIndex = 5;
            DecreaseCount.Text = "-";
            DecreaseCount.UseVisualStyleBackColor = true;
            DecreaseCount.Click += DecreaseCount_Click;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(311, 309);
            Controls.Add(DecreaseCount);
            Controls.Add(IncreaseCount);
            Controls.Add(BackButton);
            Controls.Add(ScoreText);
            Controls.Add(FinishButton);
            DoubleBuffered = true;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Form";
            RightToLeft = RightToLeft.No;
            Text = "2048";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button FinishButton;
        GameScreen Game;
        private Label ScoreText;
        private Button BackButton;
        private Button IncreaseCount;
        private Button DecreaseCount;
    }
}
