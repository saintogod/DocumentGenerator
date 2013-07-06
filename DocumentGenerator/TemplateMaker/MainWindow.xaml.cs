using DataManager.Contorls;
using DataManager.Models;
using DataManager.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using UserControlLibary;

namespace TemplateMaker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private double MINTOP = TemplateMaker.Properties.Settings.Default.MINTOP;
        private double MINLEFT = TemplateMaker.Properties.Settings.Default.MINLEFT;
        private double MAXTOP;
        private double MAXLEFT;
        private Point startMousePos;
        private Thickness startUCPos;
        UserControl userControl = new UserControl();
        Brush oriBGColor;
        private bool isMoving = false;
        private bool isDragging = false;

        private List<TextInput> fields = new List<TextInput>();
        private Dictionary<string, string> docxs = new Dictionary<string, string>();
        private static string temp;
        private Verify verify = new Verify();

        static MainWindow()
        {
            temp = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create), "DocumentGenerator");
            if (!Directory.Exists(temp))
                Directory.CreateDirectory(temp);
        }

        public MainWindow()
        {
            InitializeComponent();

            this.input.Info.Title = "Title";
            this.input.Info.Width = this.input.Width;
            this.input.Info.Height = this.input.Height;
            this.input.Info.Left = this.input.Margin.Left;
            this.input.Info.Top = this.input.Margin.Top;
            this.input.Info.Defaulstring = this.input.Value.Text;
            this.input.Info.BackgroundColor = "#00000000";
            this.input.ToolTip = this.input.Info.Description ="Hello world";
            this.input.Info.Layout = DataManager.Utils.Common.LayoutType.LeftRight;
            this.input.Editorable = false;

            fields.Add(this.input);
        }
        #region Drag & Drop

        private void TextInput_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.Source is UserControl))
                return;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var uctrl = (TextInput)e.Source;
                isDragging = true;
                userControl = uctrl.GetACopy();
                userControl.MouseLeftButtonDown += TextInput_MouseLeftButtonDown;
                userControl.MouseLeftButtonUp += TextInput_MouseLeftButtonUp;
                userControl.MouseMove += TextInput_MouseMove;
                template.Children.Add(userControl);
                userControl.Margin = uctrl.Margin;
            }
            else
            {
                userControl = (UserControl)e.Source;
                isMoving = true;
            }

            this.startUCPos = userControl.Margin;
            this.startMousePos = e.GetPosition(main);
            userControl.CaptureMouse();
            oriBGColor = userControl.Background;
            userControl.Background = (Brush)(new BrushConverter().ConvertFromString("#FF0A8DD8"));
        }

        private void TextInput_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMoving)
            {
                isMoving = false;
            }
            else if (isDragging)
            {
                isDragging = false;
                userControl.Opacity = 1;
            }
            userControl.ReleaseMouseCapture();
            userControl.Background = oriBGColor;
        }

        private void TextInput_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving || isDragging)
            {
                Point curMousePos = e.GetPosition(main);
                SetUCPosition(
                    startUCPos.Left + curMousePos.X - startMousePos.X,
                    startUCPos.Top + curMousePos.Y - startMousePos.Y);
            }

        }

        private void SetUCPosition(double left, double top)
        {
            MAXTOP = this.template.ActualHeight - userControl.ActualHeight - TemplateMaker.Properties.Settings.Default.MINBOTTOM;
            MAXLEFT = this.template.ActualWidth - userControl.ActualWidth - TemplateMaker.Properties.Settings.Default.MINRIGHT;

            left = left > MINLEFT ? left < MAXLEFT ? left : MAXLEFT : MINLEFT;
            top = top > MINTOP ? top < MAXTOP ? top : MAXTOP : MINTOP;
            userControl.Margin = new Thickness(left, top, userControl.Margin.Right, userControl.Margin.Bottom);

        }

        #endregion
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            template.Children.OfType<UserControl>().ToList<UserControl>().ForEach(x => template.Children.Remove(x));
            this.fields.Clear();
        }

        private void btn_addNewField_Click(object sender, RoutedEventArgs e)
        {
            TextInputFieldProperty tifp = new TextInputFieldProperty();
            tifp.Owner = this;
            var res = tifp.ShowDialog();
            if (res.HasValue && res.Value)
            {
                TextInput _field = new TextInput(false, tifp.GetFieldInfo());
                this.template.Children.Add(_field);

                _field.Margin = new Thickness() { Top = _field.Info.Top, Left = _field.Info.Left };

                fields.Add(_field);
            }
            
        }

        private void btn_saveXmlPath_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.CheckPathExists = true;
            sfd.DefaultExt = "xml";
            sfd.Filter = "xml|*.xml|All File|*.*";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bool? res = sfd.ShowDialog(this);
            if (res.HasValue && res.Value)
            {
                this.tb_xmlPath.Text = sfd.FileName;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_xmlPath.Text))
            {
                return;
            }
            TemplateXml tx = new TemplateXml();
            tx.Version +=1;
            foreach (RadioButton rb in this.wapperlist.Children)
            {
                if (rb.GroupName.Equals("wapper") && rb.IsChecked.Value)
                {
                    tx.Wapper = rb.Content.ToString();
                    break;
                }
            }
            tx.Fields.AddRange(fields.Select(item => item.Info));
            tx.TemplateEntries.Add(docxs);

            TemplateSerializer.Serialize(tx, this.tb_xmlPath.Text);
        }

        private void btn_contentTmplAdd_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.DefaultExt = "docx";
            ofd.Filter = "docx|*.docx|All File|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            bool? res = ofd.ShowDialog(this);
            if (res.HasValue && res.Value)
            {
                string fileName = new FileInfo(ofd.FileName).Name;
                this.cb_contentTemplates.Items.Add(fileName);
                using (var stream = new FileStream(ofd.FileName,FileMode.Open,FileAccess.Read,FileShare.Read))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes,0, bytes.Length);
                    docxs.Add(fileName, Convert.ToBase64String(bytes));
                }
                this.cb_contentTemplates.SelectedValue = fileName;
            }
        }

        private void btn_contentTmplRemove_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.cb_contentTemplates.Text))
            {
                this.cb_contentTemplates.Items.Remove(this.cb_contentTemplates.Text);
                docxs.Remove(this.cb_contentTemplates.Text);
            }
        }

        private void btn_loadDocx_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("#header#", "SJTU");
            dic.Add("#footer#", "page-1");
            dic.Add("#name#", "Saint");
            dic.Add("#age#", "26");
            dic.Add("#test##test#", "hello world");

            ZipFile.ExtractToDirectory(this.tb_docPath.Text, temp);
            IEnumerable<string> files = Directory.EnumerateFiles(temp + "word");
            List<string> words  = new List<string>();
            foreach (var file in files)
            {
                string tem = "";
                using (StreamReader sr = new StreamReader(file))
                {
                    tem = sr.ReadToEnd();
                    foreach (Match match in Regex.Matches(tem, "(?<content>#((##)|[^#])*#)"))
                    {
                        string ori = match.Groups["content"].ToString();
                        string srt = Regex.Replace(ori, "<[^>]*>", "");
                        tem = tem.Replace(ori, dic[srt]);
                        words.Add(srt);
                    }
                }
                using (var sw = new StreamWriter(file))
                {
                    sw.Write(tem);
                }
            }
            if (File.Exists(@"E:\DocumentGenerator\ContentTemplate_test.docx"))
                File.Delete(@"E:\DocumentGenerator\ContentTemplate_test.docx");
            ZipFile.CreateFromDirectory(temp, @"E:\DocumentGenerator\ContentTemplate_test.docx");

            Directory.Delete(temp, true);

        }
        /// <summary>
        /// 检查所有的文档中的Field是否都包含
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_editor_check_Click(object sender, RoutedEventArgs e)
        {
            verify.Owner = this;
            verify.Top = this.Top;
            verify.Left = this.Left + this.Width;
            verify.Height = this.Height;

            var datas = new Dictionary<string, Dictionary<string, bool>>();
            //TODO
            if (docxs.Count == 0)
                return;
            foreach (var item in docxs)
            {
                string filePath = Path.Combine(temp, item.Key);
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] data = Convert.FromBase64String(item.Value);
                    stream.Write(data, 0, data.Length);
                }
                GatherFieldFromFile(filePath).ForEach(field => datas.AddSingleItem(item.Key, field,
                    fields.Any(x => x.Info.Title.Equals(field, StringComparison.OrdinalIgnoreCase))));

                File.Delete(filePath);
            }
            
            verify.TreeViewContent = datas;

            if (verify.InitFromContent())
            {
                verify.Show();
            }
        }

        private List<string> GatherFieldFromFile(string filePath)
        {
            string extractPath = filePath.Substring(0, filePath.LastIndexOf('.'));

            if (Directory.Exists(extractPath))
                Directory.CreateDirectory(extractPath);

            ZipFile.ExtractToDirectory(filePath, extractPath);

            IEnumerable<string> files = Directory.EnumerateFiles(System.IO.Path.Combine(extractPath, "word"));
            List<string> words = new List<string>();
            files.ToList<string>().ForEach(x =>
            {
                string tem = "";
                using (StreamReader sr = new StreamReader(x))
                {
                    tem = sr.ReadToEnd();
                    foreach (Match match in Regex.Matches(tem, "#(?<content>((##)|[^#])*)#"))//TODO 这个是可以自定义的，#
                    {
                        string ori  = Regex.Replace(match.Groups["content"].ToString(), "<[^>]*>", "");
                        words.Add(ori);
                    }
                }
            });
            Directory.Delete(extractPath, true);
            words.Sort();
            return words;
        }

        private bool TransTemplateWithData(string templateFile, Dictionary<string, string> data)
        {

            return false;
        }

        private void main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (verify.IsVisible)
            {
                verify.Top = this.Top;
                verify.Left = this.Left + this.Width;
            }
        }

        private void main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (verify.IsVisible)
            {
                verify.Close();
            }
            if(Directory.Exists(temp))
                Directory.Delete(temp, true);
        }

        private void main_LocationChanged(object sender, EventArgs e)
        {
            if (verify.IsVisible)
            {
                verify.Top = this.Top;
                verify.Left = this.Left + this.Width;
            }
        }

        private void mi_file_new_Click(object sender, RoutedEventArgs e)
        {
            btn_eidtor_clear_Click(sender, e);
        }

        private void mi_file_open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"E:\DocumentGenerator\";
            ofd.DefaultExt = "xml";
            ofd.CheckFileExists = true;
            ofd.Filter = "xml|*.xml|All File|*.*";
            var res = ofd.ShowDialog(this);
            if (res.HasValue && res.Value)
            {
                string fileName = ofd.FileName;
                TemplateXml tx;
                if (TemplateSerializer.Deserialize(out tx, fileName))
                {
                    foreach (RadioButton rb in this.wapperlist.Children)
                    {
                        if (rb.GroupName.Equals("wapper") && rb.Content.Equals(tx.Wapper))
                        {
                            rb.IsChecked = true;
                            break;
                        }
                    }
                    btn_eidtor_clear_Click(sender, e);
                    fields.AddRange(tx.Fields.Select(item => new TextInput(false, item as TextInputField)));
                    foreach (var item in tx.TemplateEntries)
                    {
                        this.cb_contentTemplates.Items.Add(item.Key);
                        docxs[item.Key] = item.Value;
                    }

                    foreach (var item in fields)
                    {
                        this.template.Children.Add(item);
                        item.Margin = new Thickness() { Top = item.Info.Top, Left = item.Info.Left };
                    }
                }

            }
        }

        private void btn_eidtor_clear_Click(object sender, RoutedEventArgs e)
        {
            this.docxs.Clear();
            this.cb_contentTemplates.Items.Clear();
            fields.ForEach(item => template.Children.Remove(item));
            this.fields.Clear();
        }

    }
}
