using System;
using System.Windows.Forms;

namespace Lab1Calc
{
    partial class CalculatorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox _resultBox;
        private System.Windows.Forms.TextBox _memBox;

        // Приватні поля для всіх кнопок
        private Button _mcButton, _msButton, _mrButton, _openParenthesisButton, _closeParenthesisButton;
        private Button _squareButton, _reciprocalButton, _clearButton;
        private Button _num0Button, _num1Button, _num2Button, _num3Button, _num4Button;
        private Button _num5Button, _num6Button, _num7Button, _num8Button, _num9Button;
        private Button _decimalButton, _changeSignButton, _resultButton;
        private Button _plusButton, _minusButton, _multiplyButton, _divideButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            int startX = 30, startY = 120, buttonWidth = 50, buttonHeight = 50;
            int spacingX = 10, spacingY = 10;

            int startYTop = startY - (buttonHeight + spacingY);

            // Ініціалізація кнопок для пам'яті
            _mcButton = CreateButton("MC", startX, startY, McButton_Click, enabled: false);
            _msButton = CreateButton("MS", startX + (buttonWidth + spacingX), startY, MsButton_Click, enabled: false);
            _mrButton = CreateButton("MR", startX + 2 * (buttonWidth + spacingX), startY, MrButton_Click, enabled: false);
            _openParenthesisButton = CreateButton("(", startX + 3 * (buttonWidth + spacingX), startY, ParenthesisButton_Click, width: 25);
            _closeParenthesisButton = CreateButton(")", startX + 3 * (buttonWidth + spacingX) + 25, startY, ParenthesisButton_Click, width: 25);

            this.Controls.Add(_mcButton);
            this.Controls.Add(_msButton);
            this.Controls.Add(_mrButton);
            this.Controls.Add(_openParenthesisButton);
            this.Controls.Add(_closeParenthesisButton);

            // Ініціалізація функціональних кнопок
            _squareButton = CreateButton("x^2", startX, startY + buttonHeight + spacingY, FunctionButton_Click);
            _reciprocalButton = CreateButton("1/x", startX + (buttonWidth + spacingX), startY + buttonHeight + spacingY, FunctionButton_Click);
            _clearButton = CreateButton("C", startX + 2 * (buttonWidth + spacingX), startY + buttonHeight + spacingY, ClearButton_Click);

            this.Controls.Add(_squareButton);
            this.Controls.Add(_reciprocalButton);
            this.Controls.Add(_clearButton);

            // Ініціалізація цифрових кнопок
            _num1Button = CreateButton("1", startX, startY + 2 * buttonHeight + spacingY, NumberButton_Click);
            _num2Button = CreateButton("2", startX + (buttonWidth + spacingX), startY + 2 * buttonHeight + spacingY, NumberButton_Click);
            _num3Button = CreateButton("3", startX + 2 * (buttonWidth + spacingX), startY + 2 * buttonHeight + spacingY, NumberButton_Click);

            this.Controls.Add(_num1Button);
            this.Controls.Add(_num2Button);
            this.Controls.Add(_num3Button);

            _num4Button = CreateButton("4", startX, startY + 3 * buttonHeight + spacingY, NumberButton_Click);
            _num5Button = CreateButton("5", startX + (buttonWidth + spacingX), startY + 3 * buttonHeight + spacingY, NumberButton_Click);
            _num6Button = CreateButton("6", startX + 2 * (buttonWidth + spacingX), startY + 3 * buttonHeight + spacingY, NumberButton_Click);

            this.Controls.Add(_num4Button);
            this.Controls.Add(_num5Button);
            this.Controls.Add(_num6Button);

            _num7Button = CreateButton("7", startX, startY + 4 * buttonHeight + spacingY, NumberButton_Click);
            _num8Button = CreateButton("8", startX + (buttonWidth + spacingX), startY + 4 * buttonHeight + spacingY, NumberButton_Click);
            _num9Button = CreateButton("9", startX + 2 * (buttonWidth + spacingX), startY + 4 * buttonHeight + spacingY, NumberButton_Click);

            this.Controls.Add(_num7Button);
            this.Controls.Add(_num8Button);
            this.Controls.Add(_num9Button);

            _num0Button = CreateButton("0", startX, startY + 5 * buttonHeight + spacingY, NumberButton_Click);
            _decimalButton = CreateButton(".", startX + (buttonWidth + spacingX), startY + 5 * buttonHeight + spacingY, DecimalButton_Click);
            _changeSignButton = CreateButton("+/-", startX + 2 * (buttonWidth + spacingX), startY + 5 * buttonHeight + spacingY, ChangeSignButton_Click);
            _resultButton = CreateButton("=", startX + 3 * (buttonWidth + spacingX), startY + 5 * buttonHeight + spacingY, ResultButton_Click);

            this.Controls.Add(_num0Button);
            this.Controls.Add(_decimalButton);
            this.Controls.Add(_changeSignButton);
            this.Controls.Add(_resultButton);

            // Ініціалізація операторів
            _divideButton = CreateButton("/", startX + 3 * (buttonWidth + spacingX), startY + buttonHeight + spacingY, OperatorButton_Click);
            _plusButton = CreateButton("+", startX + 3 * (buttonWidth + spacingX), startY + 4 * buttonHeight + spacingY, OperatorButton_Click);
            _minusButton = CreateButton("-", startX + 3 * (buttonWidth + spacingX), startY + 2 * buttonHeight + spacingY, OperatorButton_Click);
            _multiplyButton = CreateButton("*", startX + 3 * (buttonWidth + spacingX), startY + 3 * buttonHeight + spacingY, OperatorButton_Click);

            this.Controls.Add(_divideButton);
            this.Controls.Add(_plusButton);
            this.Controls.Add(_minusButton);
            this.Controls.Add(_multiplyButton);

            // Ініціалізація полів введення
            this._resultBox = new System.Windows.Forms.TextBox();
            this._resultBox.Location = new System.Drawing.Point(startX, startY - 65);
            this._resultBox.Size = new System.Drawing.Size(230, 50);
            this._resultBox.Multiline = true;
            this._resultBox.ReadOnly = true;
            this._resultBox.TextAlign = HorizontalAlignment.Right;
            this._resultBox.CharacterCasing = CharacterCasing.Upper;
            this._resultBox.Font = new System.Drawing.Font("Arial", 24, System.Drawing.FontStyle.Bold);
            this._resultBox.TextChanged += ResultBoxTextChanged;
            this.Controls.Add(this._resultBox);

            this._memBox = new System.Windows.Forms.TextBox();
            this._memBox.Location = new System.Drawing.Point(startX, startY - 100);
            this._memBox.Size = new System.Drawing.Size(230, 30);
            this._memBox.Multiline = true;
            this._memBox.ReadOnly = true;
            this._memBox.TextAlign = HorizontalAlignment.Right;
            this._memBox.CharacterCasing = CharacterCasing.Upper;
            this._memBox.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            this._memBox.TextChanged += MemoryBoxTextChanged;
            this.Controls.Add(this._memBox);

            // Параметри форми
            this.ClientSize = new System.Drawing.Size(285, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Text = "Calculator";
        }

        private Button CreateButton(string text,
            int x,
            int y,
            EventHandler onClick,
            int width = 50,
            int height = 50,
            bool enabled = true)
        {
            Button button = new Button();
            button.Enabled = enabled;
            button.Text = text;
            button.Size = new System.Drawing.Size(width, height);
            button.Location = new System.Drawing.Point(x, y);
            button.Click += onClick;
            return button;
        }
    }
}