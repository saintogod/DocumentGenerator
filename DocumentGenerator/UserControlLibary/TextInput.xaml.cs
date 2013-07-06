using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Common = DataManager.Utils.Common;
using TextInputField = DataManager.Models.TextInputField;

namespace UserControlLibary
{
    /// <summary>
    /// TextInput.xaml 的交互逻辑
    /// </summary>
    public partial class TextInput : UserControl, ISharedAttribute<TextInputField> 
    {
        private TextInputField _info;
        private SolidColorBrush brush = new SolidColorBrush();
        public TextInputField Info
        {
            get
            {
                return _info;
            }
            set
            {
                SwitchMode(value.Layout);
                _info = value;
                brush.Color = (Color)ColorConverter.ConvertFromString(_info.BackgroundColor);
                this.Background = brush;
                this.Value.Text = _info.Defaulstring;
                this.ToolTip = _info.Description;
                this.Width = _info.Width;
                this.Height = _info.Height;
                this.Margin = new Thickness() { Left = _info.Left, Top = _info.Top };
            }
        }

        private bool editorable;
        public bool Editorable { get { return editorable; } set { editorable = value; Value.IsReadOnly = !editorable; } }

        public TextInput()
        {
            this._info = new TextInputField();
            InitializeComponent();
        }

        public TextInput(bool editorable, TextInputField tif)
        {
            InitializeComponent();
            this._info = new TextInputField(tif);

            this.Title.Content = _info.Title;
            this.Value.Text = _info.Defaulstring;
            this.ToolTip = _info.Description;
            if(!_info.Layout.Equals(Common.LayoutType.LeftRight))
                SwitchMode(_info.Layout);
            Editorable = editorable;
        }

        public TextInput GetACopy()
        {
            var ti = new TextInput(false, this._info);
            ti.Margin = this.Margin;
            ti.Width = this.Width;
            ti.Height = this.Height;
            ti.Opacity = 0.5;
            return ti;
        }

        public void setBackgroundColor(string bgColor)
        {
            _info.BackgroundColor = bgColor;
            brush.Color = (Color)ColorConverter.ConvertFromString(bgColor);
            this.Background = brush;
        }

        public void setTextInputField(string title, double width, double height, double left, double top, Common.LayoutType layout,
            string bgColor = "#00000000", string description = "", string defaulstring = "")
        {
            _info.Title = title;
            _info.Width = width;
            _info.Height = height;
            _info.Left = left;
            _info.Top = top;
            setBackgroundColor(bgColor);
            SwitchMode(layout);
            _info.Description = description;
            _info.Defaulstring = defaulstring;
        }

        public void SwitchMode(Common.LayoutType layout)
        {
            this.Info.Layout = layout;
            switch (layout)
            {
                case Common.LayoutType.UpDown:
                    this.Width = this.content.Width = 150;
                    this.Height = this.content.Height = 86;
                    this.Title.Margin = new Thickness(5, 10, 5, 53);
                    this.Value.Width = 140;
                    this.Value.Margin = new Thickness(5, 53, 5, 10);
                    break;
                case Common.LayoutType.LeftRight:
                    this.Width = this.content.Width = 300;
                    this.Height = this.content.Height = 43;

                    this.Title.Margin = new Thickness(10, 10, 170, 10);
                    this.Value.Width = 150;
                    this.Value.Margin = new Thickness(140, 10, 10, 10);
                    break;
                default:
                    break;
            }
        }

        private void content_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = (Brush)(new BrushConverter().ConvertFromString("#FF0A8DD8"));
        }

        private void content_MouseLeave(object sender, MouseEventArgs e)
        {
            if(!this.Value.IsFocused)
                this.Background = brush;
        }
        
        private void uc_GotMouseCapture(object sender, MouseEventArgs e)
        {
            this.Background = (Brush)(new BrushConverter().ConvertFromString("#FF0A8DD8"));
        }

        private void uc_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this.Background = brush;
        }

        private void Value_GotFocus(object sender, RoutedEventArgs e)
        {
            this.Background = (Brush)(new BrushConverter().ConvertFromString("#FF0A8DD8"));
        }
        
        private void Value_LostFocus(object sender, RoutedEventArgs e)
        {
            this.Background = brush;
        }

        private void uc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextInputFieldProperty tifp = new TextInputFieldProperty(Info);

            var res = tifp.ShowDialog();
            if (res.HasValue && res.Value)
            {
                Info.SetValue(tifp.GetFieldInfo());
                Margin = new Thickness() { Top = Info.Top, Left = Info.Left };
            }
            e.Handled = true;
        }

    }
}
