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

namespace TemplateMaker
{
    /// <summary>
    /// Verify.xaml 的交互逻辑
    /// </summary>
    public partial class Verify : Window
    {
        public Dictionary<string, Dictionary<string, bool>> TreeViewContent { get; set; }

        public Verify(Dictionary<string, Dictionary<string, bool>> tvc)
        {
            InitializeComponent();
            TreeViewContent = tvc;
        }

        public Verify()
        {
            InitializeComponent();
        }

        public bool InitFromContent() {
            if (TreeViewContent.Count == 0)
                return false;
            try
            {
                tv_verifyResult.Items.Clear();
                foreach (var item in TreeViewContent)
                {
                    TreeViewItem tvi = new TreeViewItem();
                    tvi.Header = item.Key;
                    foreach (var field in item.Value)
                    {
                        tvi.Items.Add(new TreeViewItem()
                        {
                            Header = field.Key,
                            Style = FindResource(field.Value ? "existField" : "nonExistField") as Style
                        });
                    }
                    tvi.IsExpanded = true;
                    int count = item.Value.Count(x => !x.Value);
                    tvi.Header = string.Format("({0}/{1}){2}", count,item.Value.Count(), tvi.Header);
                    tvi.Style = FindResource(count == 0 ? "existField" : "nonExistField") as Style;
                    tv_verifyResult.Items.Add(tvi);
                }
                
                //this.MinWidth = tv_verifyResult.ActualWidth + 20;//TODO
                if (MinWidth > Width)
                    Width = MinWidth;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
