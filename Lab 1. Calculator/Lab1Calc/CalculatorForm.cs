using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Lab1Calc
{
    public partial class CalculatorForm : Form
    {
        private static readonly char[] Operators = { '+', '-', '*', '/' };
        
        // state
        private int _openParenthesisCount;

        public CalculatorForm()
        {
            InitializeComponent();

            KeyDown += CalculatorForm_KeyDown;
            _resultBox.KeyDown += CalculatorForm_KeyDown;
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            if (ResultBoxEndsWithCloseParenthesis())
            {
                return;
            }

            _resultBox.Text += button.Text;
        }

        private void DecimalButton_Click(object sender, EventArgs e)
        {
            if (ResultBoxEndsWithDigit())
            {
                _resultBox.Text += @".";
            }
        }

        private void ChangeSignButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_resultBox.Text) || _resultBox.Text == @"0")
            {
                return;
            }

            var lastIndex = _resultBox.Text.Length - 1;

            while (lastIndex >= 0 && (char.IsDigit(_resultBox.Text[lastIndex]) || _resultBox.Text[lastIndex] == '.'))
            {
                lastIndex--;
            }

            if (lastIndex == 0 && _resultBox.Text[lastIndex] == '-')
            {
                _resultBox.Text = _resultBox.Text.Substring(1);
                return;
            }

            if (lastIndex < 0 || (_resultBox.Text[lastIndex] != '+' && _resultBox.Text[lastIndex] != '-'))
            {
                _resultBox.Text = _resultBox.Text.Insert(0, "-");
                return;
            }

            _resultBox.Text = _resultBox.Text.Remove(lastIndex, 1).Insert(lastIndex, _resultBox.Text[lastIndex] == '-' ? "+" : "-");
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            if (ResultBoxEndsWithDigit() || ResultBoxEndsWithCloseParenthesis())
            {
                _resultBox.Text += button.Text;
            }
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            var success = TryEvaluateExpression(out var result);

            if (!success)
            {
                return;
            }

            _resultBox.Text = result.ToString(CultureInfo.InvariantCulture);
            _openParenthesisCount = 0;
        }

        private void FunctionButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            switch (button.Text)
            {
                case @"x^2":
                {
                    var success = TryEvaluateExpression(out var result);

                    if (!success)
                    {
                        return;
                    }

                    _resultBox.Text = Math.Pow(result, 2).ToString(CultureInfo.InvariantCulture);
                    break;
                }
                case @"1/x":
                {
                    var success = TryEvaluateExpression(out var result);

                    if (!success)
                    {
                        return;
                    }

                    _resultBox.Text = (1 / result).ToString(CultureInfo.InvariantCulture);

                    break;
                }
            }
        }

        private void MsButton_Click(object sender, EventArgs _)
        {
            if (_resultBox.Text.Any(c => Operators.Contains(c)))
            {
                var success = TryEvaluateExpression(out var result);

                if (success)
                {
                    _memBox.Text = result.ToString(CultureInfo.InvariantCulture);
                }
            }
            else
            {
                _memBox.Text = _resultBox.Text.Replace("(", "").Replace(")", "");
            }
        }

        private void MrButton_Click(object sender, EventArgs _)
        {
            _resultBox.Text = _memBox.Text;
        }

        private void McButton_Click(object sender, EventArgs _)
        {
            _memBox.Clear();
        }

        private void ParenthesisButton_Click(object sender, EventArgs _)
        {
            if (!(sender is Button button))
            {
                return;
            }

            var buttonText = button.Text;

            switch (buttonText)
            {
                case "(":
                    _resultBox.Text += @"(";
                    _openParenthesisCount++;
                    break;
                case ")":
                {
                    if (_openParenthesisCount > 0 && !Operators.Contains(_resultBox.Text[_resultBox.Text.Length - 1]))
                    {
                        _resultBox.Text += @")";
                        _openParenthesisCount--;
                    }

                    break;
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            _resultBox.Clear();
        }
        
        private void ResultBoxTextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }

            _msButton.Enabled = textBox.Text.Length > 0;

            if (textBox.Text.Length > 16)
            {
                textBox.Text = textBox.Text.Substring(0, 16);
                return;
            }

            float fontSize;
            switch (textBox.TextLength)
            {
                case 12:
                    fontSize = 23;
                    break;
                case 13:
                    fontSize = 21;
                    break;
                case 14:
                    fontSize = 19.8f;
                    break;
                case 15:
                    fontSize = 18.8f;
                    break;
                case 16:
                    fontSize = 17.8f;
                    break;
                default:
                    if (textBox.Text.Length < 12 && Math.Abs(textBox.Font.Size - 24f) < 0)
                    {
                        textBox.Font = new System.Drawing.Font("Arial", 24, System.Drawing.FontStyle.Bold);
                    }
                    return;
            }

            textBox.Font = new System.Drawing.Font("Arial", fontSize, System.Drawing.FontStyle.Bold);
        }
        
        private void MemoryBoxTextChanged(object sender, EventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }

            var enableButtons = textBox.Text.Length > 0;
            _mrButton.Enabled = enableButtons;
            _mcButton.Enabled = enableButtons;

            if (textBox.Text.Length > 16)
            {
                textBox.Text = textBox.Text.Substring(0, 16);
                if (textBox.Font.Size > 17.8f)
                {
                    textBox.Font = new System.Drawing.Font("Arial", 17.8f, System.Drawing.FontStyle.Bold);
                }
                return;
            }

            float fontSize;
            switch (textBox.TextLength)
            {
                case 12:
                    fontSize = 23;
                    break;
                case 13:
                    fontSize = 21;
                    break;
                case 14:
                    fontSize = 19.8f;
                    break;
                case 15:
                    fontSize = 18.8f;
                    break;
                case 16:
                    fontSize = 17.8f;
                    break;
                default:
                    if (textBox.Text.Length < 12 && Math.Abs(textBox.Font.Size - 24f) < 0)
                    {
                        textBox.Font = new System.Drawing.Font("Arial", 24, System.Drawing.FontStyle.Bold);
                    }
                    return;
            }

            textBox.Font = new System.Drawing.Font("Arial", fontSize, System.Drawing.FontStyle.Bold);
        }
        
        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    NumberButton_Click(_num0Button, EventArgs.Empty);
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    NumberButton_Click(_num1Button, EventArgs.Empty);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    NumberButton_Click(_num2Button, EventArgs.Empty);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    NumberButton_Click(_num3Button, EventArgs.Empty);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    NumberButton_Click(_num4Button, EventArgs.Empty);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    NumberButton_Click(_num5Button, EventArgs.Empty);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    NumberButton_Click(_num6Button, EventArgs.Empty);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    NumberButton_Click(_num7Button, EventArgs.Empty);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    NumberButton_Click(_num8Button, EventArgs.Empty);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    NumberButton_Click(_num9Button, EventArgs.Empty);
                    break;
                case Keys.Oemplus:
                case Keys.Add:
                    OperatorButton_Click(_plusButton, EventArgs.Empty);
                    break;
                case Keys.OemMinus:
                case Keys.Subtract:
                    OperatorButton_Click(_minusButton, EventArgs.Empty);
                    break;
                case Keys.Multiply:
                    OperatorButton_Click(_multiplyButton, EventArgs.Empty);
                    break;
                case Keys.Divide:
                    OperatorButton_Click(_divideButton, EventArgs.Empty);
                    break;
                case Keys.Enter:
                    ResultButton_Click(_resultButton, EventArgs.Empty);
                    break;
                case Keys.Back:
                    if (_resultBox.Text.Length > 0)
                    {
                        _resultBox.Text = _resultBox.Text.Substring(0, _resultBox.Text.Length - 1);
                    }
                    break;
                case Keys.Decimal:
                case Keys.OemPeriod:
                    DecimalButton_Click(_decimalButton, EventArgs.Empty);
                    break;
                case Keys.C:
                    ClearButton_Click(_clearButton, EventArgs.Empty);
                    break;
                case Keys.M:
                    MsButton_Click(_msButton, EventArgs.Empty);
                    break;
                case Keys.R:
                    MrButton_Click(_mrButton, EventArgs.Empty);
                    break;
                case Keys.L:
                    McButton_Click(_mcButton, EventArgs.Empty);
                    break;
                case Keys.OemOpenBrackets:
                    ParenthesisButton_Click(_openParenthesisButton, EventArgs.Empty);
                    break;
                case Keys.OemCloseBrackets:
                    ParenthesisButton_Click(_closeParenthesisButton, EventArgs.Empty);
                    break;
            }
        }

        private bool TryEvaluateExpression(out float result)
        {
            if (ResultBoxEndsWithDigit() || ResultBoxEndsWithCloseParenthesis())
            {
                AddMissingParenthesis();
                result = Calculator.Evaluate(_resultBox.Text);
                return true;
            }

            result = 0;
            return false;
        }

        private void AddMissingParenthesis()
        {
            while (_openParenthesisCount > 0)
            {
                _resultBox.Text += @")";
                _openParenthesisCount--;
            }
        }

        private bool ResultBoxEndsWithDigit() => _resultBox.Text.Length > 0 && char.IsDigit(_resultBox.Text[_resultBox.Text.Length - 1]);

        private bool ResultBoxEndsWithCloseParenthesis() => _resultBox.Text.Length > 0 && _resultBox.Text[_resultBox.Text.Length - 1] == ')';
    }
}