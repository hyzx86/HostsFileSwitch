using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostsFileSwitch
{
    public partial class Form1 : Form
    {

        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
               
                AppLoad();
                if (args[0].ToLower() == "e")
                {
                    ChangeState("<<");
                }
                else if (args[0].ToLower() == "d")
                {
                    ChangeState(">>");
                }

                Application.Exit(); 
            }
        
        }

        const string HOSTSFILEPATH = @"C:\Windows\System32\drivers\etc\hosts";


        private void Form1_Load(object sender, EventArgs e)
        {
            AppLoad();

        }

        private void AppLoad()
        {
            string fileContent = File.ReadAllText(HOSTSFILEPATH);
            txtFileContent.Text = fileContent;

            var contentList = fileContent.Split(new string[] { "\r\n" },
                StringSplitOptions.RemoveEmptyEntries).Reverse();
            //排除掉以# 号开头的数据

            var enabledList = contentList.Where(o => !o.StartsWith("#"));
            //排除掉另外一部分数据
            var disabledList = contentList.Except(enabledList);
            lbEnabled.Items.Clear();
            lbDisabled.Items.Clear();
            lbEnabled.Items.AddRange(enabledList.ToArray());
            lbDisabled.Items.AddRange(disabledList.ToArray());
        }

        private void Save(string[] allList)
        {
            try
            {
                if (File.Exists(HOSTSFILEPATH))
                {
                    File.Delete(HOSTSFILEPATH);
                }

                File.AppendAllLines(HOSTSFILEPATH, allList.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Move_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var oprName = btn.Text;

            ChangeState(oprName);
        }

        private void ChangeState(string oprName)
        {
            switch (oprName)
            {
                //禁用所有
                case ">>":
                    var swapList = lbEnabled.Items.Cast<string>();
                    var targetList = swapList.Select(o => "#" + o);
                    lbDisabled.Items.AddRange(targetList.ToArray());
                    lbEnabled.Items.Clear();

                    break;
                //禁用选中
                case ">":
                    swapList = lbEnabled.SelectedItems.Cast<string>();
                    targetList = swapList.Select(o => "#" + o);
                    lbDisabled.Items.AddRange(targetList.ToArray());
                    for (int i = 0; i < lbEnabled.SelectedItems.Count; i++)
                    {
                        lbEnabled.Items.Remove(lbEnabled.SelectedItems[i]);
                    }
                    break;

                //启用全部
                case "<<":
                    swapList = lbDisabled.Items.Cast<string>();
                    targetList = swapList.Select(o => o.TrimStart(" #".ToCharArray()));
                    lbEnabled.Items.AddRange(targetList.ToArray());
                    lbDisabled.Items.Clear();

                    break;
                //启用选中
                case "<":
                    swapList = lbDisabled.SelectedItems.Cast<string>();
                    targetList = swapList.Select(o => o.TrimStart(" #".ToCharArray()));
                    lbEnabled.Items.AddRange(targetList.ToArray());
                    lbDisabled.Items.Remove(lbDisabled.SelectedItems);
                    for (int i = 0; i < lbDisabled.SelectedItems.Count; i++)
                    {
                        lbDisabled.Items.Remove(lbDisabled.SelectedItems[i]);
                    }

                    break;

                default:
                    break;
            }



            var lsEnabled = lbEnabled.Items.Cast<string>();
            var lsDisabled = lbDisabled.Items.Cast<string>();

            var allList = new List<string>();


            allList.AddRange(lsEnabled);
            allList.AddRange(lsDisabled);
            allList.Reverse();

            Save(allList.ToArray());
            AppLoad();
        }

        private void Lb_DoubleClick(object sender, EventArgs e)
        {
            if (sender is ListBox)
            {
                var lb = (ListBox)sender;
                if (lb.Name == "lbEnabled")
                {
                    ChangeState(">");
                }
                else
                {
                    ChangeState("<");
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Save(txtFileContent.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            AppLoad();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.cnblogs.com/Qbit/p/11080198.html");
        }
    }
}
