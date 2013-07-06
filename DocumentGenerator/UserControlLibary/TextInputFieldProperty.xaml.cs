using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DataManager.Models;
using DataManager.Utils;
namespace UserControlLibary
{
    /// <summary>
    /// FieldProperty.xaml 的交互逻辑
    /// </summary>
    public partial class TextInputFieldProperty : Window
    {
        private TextInputField field;

        public TextInputFieldProperty()
        {
            field = new TextInputField();
            InitializeComponent();
            setDefaultData();
        }

        public TextInputFieldProperty(TextInputField _field = null)
        {
            field = new TextInputField(_field);
            InitializeComponent();
            SetByObject();
            this.btn_submit.Content = "Modify";
        }

        void setDefaultData()
        {
            this.tb_top.Text = UserControlLibary.Properties.Settings.Default.MINTOP.ToString();
            this.tb_left.Text = UserControlLibary.Properties.Settings.Default.MINLEFT.ToString();
        }

        public void WriteToObject()
        {
            field.Title = this.tb_title.Text;
            field.Width = string.IsNullOrEmpty(this.tb_width.Text) ? 0 : Double.Parse(this.tb_width.Text);
            field.Height = string.IsNullOrEmpty(this.tb_height.Text) ? 0 : Double.Parse(this.tb_height.Text);
            field.Left = string.IsNullOrEmpty(this.tb_left.Text) ? 0 : Double.Parse(this.tb_left.Text);
            field.Top = string.IsNullOrEmpty(this.tb_top.Text) ? 0 : Double.Parse(this.tb_top.Text);
            field.BackgroundColor = this.cp_backgroundColor.SelectedColor.ToString();
            field.Defaulstring = this.tb_defaulstring.Text;
            field.Description = this.tb_description.Text;

            //var _layout = (from rb in this.sp_floatTyp.Children.OfType<RadioButton>()
            //              where rb.IsChecked.HasValue && rb.IsChecked.Value
            //              select rb).First<RadioButton>();
            //field.Layout = Common.getLayout(_layout.Content.ToString());
            field.Layout = rb_leftRight.IsChecked.HasValue && rb_leftRight.IsChecked.Value ? Common.LayoutType.LeftRight : Common.LayoutType.UpDown;
        }

        public void SetByObject()
        {
            this.tb_title.Text = field.Title;
            this.tb_width.Text = field.Width.ToString();
            this.tb_height.Text = field.Height.ToString();
            this.tb_left.Text = field.Left.ToString();
            this.tb_top.Text = field.Top.ToString();
            this.cp_backgroundColor.SelectedColor = (Color)ColorConverter.ConvertFromString(field.BackgroundColor);
            this.tb_defaulstring.Text = field.Defaulstring;
            this.tb_description.Text = field.Description;

            //var rb = (from rb in this.sp_floatTyp.Children.OfType<RadioButton>()
            //               where rb.Content.ToString().Equals(Common.getLayoutString(field.Layout))
            //               select rb).First<RadioButton>();

            var rb = field.Layout.Equals(Common.LayoutType.LeftRight) ? rb_leftRight : rb_upDown;
            rb.IsChecked = true;
            
        }

        /// <summary>
        /// Setting the operation type
        /// </summary>
        /// <param name="type">Submit or Modify</param>
        public void SettingOperationType(string type)
        {
            this.btn_submit.Content = type;
        }

        public TextInputField GetFieldInfo()
        {
            return field ?? new TextInputField();
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            WriteToObject();
            this.DialogResult = true;
            this.Close();

        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        
        private void rb_floatChanged(object sender, RoutedEventArgs e)
        {
            if (rb_leftRight.IsChecked.HasValue && rb_leftRight.IsChecked.Value)
            {
                this.tb_height.Text = "43.0";
                this.tb_width.Text = "300.0";
            }
            else
            {
                this.tb_height.Text = "86.0";
                this.tb_width.Text = "150.0";
            }
        }
    }
}
