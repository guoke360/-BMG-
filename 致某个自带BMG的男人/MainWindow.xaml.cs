using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 致某个自带BMG的男人
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> s = new List<string>();
        byte[] bs;
        public List<string> str8 = new List<string>();
        private bool? flag = null;
        private bool? flag1;
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        StringBuilder sb = new StringBuilder();
        byte[] byData = new byte[100];
        char[] charData = new char[1000];
        private string[] strm = null;
        public MainWindow()
        {
            try
            {
                FileStream fs = new FileStream("歌曲列表.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.Default);

                string line = sr.ReadLine();
                string text = System.IO.File.ReadAllText("歌曲列表.txt", Encoding.Default);
                Console.WriteLine(text);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            flag = null; flag1 = null;
            str8 = new List<string>();
            ////slidvolume.Value = 0.3; me.Volume = 0.3;
            txttataltime.Content = me.Position.ToString();
            txtruntime.Content = me.Position.ToString();
            listBox1.Items.Clear();
            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            byte[] bt = new byte[fs1.Length];
            fs1.Read(bt, 0, bt.Length);
            string str = Encoding.Default.GetString(bt);
            string[] str4 = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            s = str4.Distinct().ToList();
            for (int i = 0; i <= s.Count - 1; i++)
            {
                if (s[i].LastIndexOf("\\") >= 0)
                {
                    listBox1.Items.Add(s[i].Substring(s[i].LastIndexOf("\\") + 1));
                    str8.Add(s[i].Substring(0, s[i].LastIndexOf("\\")));
                }
            }
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Enabled = true;
            t.Tick += new EventHandler(t_Tick);
            t.Interval = 1100; t.Start();
        }
        private void btnfor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (flag1 == null)
                {
                    listBox1.SelectedIndex--;
                }
                if (flag1 == true)
                {
                    Random rnd = new Random();
                    listBox1.SelectedIndex = rnd.Next(0, listBox1.Items.Count - 1);
                }
                if (flag1 == false)
                {
                }
                if (listBox1.SelectedIndex < 0) { (listBox1.SelectedIndex) = (listBox1.Items.Count - 1); }
                musicplay();
            }
            catch { System.Windows.Forms.MessageBox.Show("请选择歌曲"); }
        }
        private void btnnext_Click(object sender, RoutedEventArgs e)
        {
            if (flag1 == null)
            {
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                {
                    listBox1.SelectedIndex++;
                    musicplay();
                    return;
                }
                if (listBox1.SelectedIndex >= listBox1.Items.Count - 1)
                {
                    (listBox1.SelectedIndex) = 0;
                }
            }
            if (flag1 == true)
            {
                Random rnd = new Random();
                listBox1.SelectedIndex = rnd.Next(0, listBox1.Items.Count - 1);
            }
            if (flag1 == false)
            {
            }
            musicplay();
        }
        private void btnplayhold_Click(object sender, RoutedEventArgs e)
        {
            if (flag == true)
            {
                btnplayhold.Content = "暂停";
                me.Pause();
                flag = false; return;
            }
            else
            {
                if (flag == false)
                {
                    me.Play();
                    btnplayhold.Content = "播放";
                    flag = true;
                    return;
                }
                else
                {
                    btnplayhold.Content = "播放";
                    musicplay();
                    return;
                }
            }
        }
        private void btnopen_Click(object sender, RoutedEventArgs e)
        {
            fbd.ShowDialog();
            byte[] bs;
            string[] str = { "*.mp3", "*.mp4" };
            foreach (string s in str)
            {
                if (fbd.SelectedPath == "")
                {
                    System.Windows.Forms.MessageBox.Show("请选择正确的文件夹");
                    return;
                }
                strm = Directory.GetFiles(fbd.SelectedPath, s);//需要单个数据条给str8赋值
                str8.Add(Convert.ToString(fbd.SelectedPath));

     
                for (int i = 0; i <= strm.Length - 1; i++)
                {
                    sb.Append(strm[i]);
                    sb.Append("\r\n");
                    listBox1.Items.Add(strm[i].Substring(strm[i].LastIndexOf("\\") + 1));
                }
            }
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            bs = Encoding.Default.GetBytes(sb.ToString());
            fs.Write(bs, 0, bs.Length);
            fs.Flush();
            fs.Close();
            if (listBox1.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("请选择正确的文件夹");
            }
        }
        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void listBox1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            musicplay();
        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = null;
        }
        private void btngo_Click_1(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex + 1 != 0)
            {
                TimeSpan ts = new TimeSpan(0, 0, 3);
                me.Position = me.Position.Add(ts);
            }
        }
        private void btnback_Click_1(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex + 1 != 0)
            {
                TimeSpan ts = new TimeSpan(0, 0, 3);
                me.Position = me.Position - ts;
            }
        }
        private void slidvolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            me.ScrubbingEnabled = true;
            me.Volume = slidvolume.Value;
        }
        private void musicplay()
        {
            Thread.Sleep(1000); me.Close();
            if (this.listBox1.SelectedItem == null)
            {
                return;
            }
            if (listBox1.SelectedItem.ToString() == null)
            {
                System.Windows.Forms.MessageBox.Show("请选择歌曲");
                return;
            }
            for (int i = 0; i <= str8.Count - 1; i++)
            {
                if (str8[i] != "")
                {
                    if (File.Exists(str8[i] + "\\" + listBox1.SelectedItem.ToString()) == true)
                    {
                        lable1.Content = "当前播放" + listBox1.SelectedItem.ToString();
                        lable1.Width = listBox1.SelectedItem.ToString().Length * 20;
                        me.Source = new Uri(str8[i] + "\\" + listBox1.SelectedItem.ToString().ToString(), UriKind.RelativeOrAbsolute);
                        me.Play(); btnplayhold.Content = "播放";
                        flag = true; return;
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(0, 0, 0);
                        me.Position = ts;
                        lable1.Content = "都是假的，不存在的";
                        lable1.Width = listBox1.SelectedItem.ToString().Length * 40;
                        txttataltime.Content = "00:00:00";
                    }
                }
            }
        }
        private void t_Tick(object sender, EventArgs e)
        {
            string str = me.NaturalDuration.ToString();
            if (me.Position == me.NaturalDuration)
            {
                me.Close();
                string[] str3 = str.Split(':');
                double a = Convert.ToDouble(str3[0]);
                double b = Convert.ToDouble(str3[1]);
                double c = Convert.ToDouble(str3[2]);
                //slider1.Maximum = (a * 3600 + b * 60 + c);
            }
            if (str != "Automatic")
            {
                string[] str3 = str.Split(':');
                double a = Convert.ToDouble(str3[0]);
                double b = Convert.ToDouble(str3[1]);
                double c = Convert.ToDouble(str3[2]);
                //slider1.Maximum = (a * 3600 + b * 60 + c);
                txttataltime.Content = me.NaturalDuration.ToString();
            }
            if (str == "Automatic")
            {
                str = me.Position.ToString();
            }
            //slider1.Minimum = 0;
            txtruntime.Content = me.Position.ToString();
            string s = me.Position.ToString();
            string[] str2 = s.Split(':');
            double d = Convert.ToDouble(str2[0]);
            double g = Convert.ToDouble(str2[1]);
            double f = Convert.ToDouble(str2[2]);
            //slider1.Value = (d * 3600 + g * 60 + f);
        }
        private void btnplaymode_Click(object sender, RoutedEventArgs e)
        {
            if (flag1 == null)
            {
                btnplaymode.Content = "随机播放"; flag1 = true; return;
            }
            if (flag1 == true) { btnplaymode.Content = "单曲循环"; flag1 = false; return; }
            if (flag1 == false) { btnplaymode.Content = "列表循环"; flag1 = null; return; }
        }
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            me.ScrubbingEnabled = true;
           // me.Position = TimeSpan.FromSeconds(slider1.Value);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofp = new Microsoft.Win32.OpenFileDialog();
            ofp.Multiselect = true; ofp.ShowDialog();
            if (ofp.FileName == "")
            {
                System.Windows.Forms.MessageBox.Show("请?选?择?正y确¨¡¤的Ì?文?件t");
                return;
            }
            for (int i = 0; i <= ofp.FileNames.Count() - 1; i++)
            {
                sb.Append(ofp.FileNames[i]);
                sb.Append("\r\n");
                if (ofp.SafeFileNames[i] != "")
                {
                    listBox1.Items.Add(ofp.SafeFileNames[i]);
                    str8.Add(ofp.FileNames[i].Substring(0, ofp.FileNames[i].LastIndexOf("\\") + 1));
                }
            }
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            bs = Encoding.Default.GetBytes(sb.ToString());
            fs.Write(bs, 0, bs.Length);
            fs.Flush();
            fs.Close();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                byte[] bt = new byte[fs1.Length];
                fs1.Read(bt, 0, bt.Length);
                fs1.Dispose();
                string str = Encoding.Default.GetString(bt);
                string[] s = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                List<string> list = new List<string>(s);
                if (listBox1.SelectedItem != null)
                {
                    for (int i = 0; i < s.Length - 1; i++)
                    {
                        if (s[i].Contains(listBox1.SelectedItem.ToString()))
                        {
                            list.RemoveAt(i);
                        }
                    }
                }
                s = (string[])list.ToArray();
                FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt");
                file.Refresh();
                file.Delete();
                List<string> li = new List<string>();
                li = s.Distinct().ToList();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i <= li.Count - 1; i++)
                {
                    sb.Append(li[i]);
                    sb.Append("\r\n");
                }
                FileStream fs2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                bs = Encoding.Default.GetBytes(sb.ToString());
                fs2.Write(bs, 0, bs.Length);
                fs2.Flush(); fs2.Close();
            }
            catch
            {

            }
            listBox1.Items.Remove(listBox1.SelectedItem);
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            me.Stop();
            listBox1.Items.Clear();
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\歌曲列表.txt");
            file.Refresh();
            file.Delete();
        }
        private void me_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (flag1 == null)
            {
                if (listBox1.SelectedIndex == listBox1.Items.Count - 1)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    listBox1.SelectedIndex++;
                }
                musicplay();
            }
            if (flag1 == true)
            {
                Random rnd = new Random();
                listBox1.SelectedIndex = rnd.Next(0, listBox1.Items.Count - 1);
                musicplay();
            }
            if (flag1 == false)
            {
                musicplay();
            }
        }
    }
}
