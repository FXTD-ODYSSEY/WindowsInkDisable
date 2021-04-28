using System;
using System.IO;
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
using System.Windows.Navigation;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string isSoftwraeInstall(string software)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey uninstallNode = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("DisplayName");
                if (displayName != null)
                {
                    if (displayName.ToString().ToLower().Contains(software.ToLower()))
                    {
                        //MessageBox.Show(subKey.GetValue("InstallLocation").ToString());
                        return subKey.GetValue("InstallLocation").ToString();
                    }
                }
            }
            return "";
        }

        private void Config_BTN_Click(object sender, RoutedEventArgs e)
        {
            string location = isSoftwraeInstall("PhotoShop");

            if (location.Length != 0)
            {
                string psName = Path.GetFileName(location);
                string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string settingsPath = Path.Combine(ApplicationData,"Adobe", psName, $"{psName} Settings");
                if (!Directory.Exists(settingsPath))
                {
                    MessageBox.Show($"PhotoShop Settings 目录不存在\n{settingsPath}", "配置失败");
                }
                else
                {
                    string conifg = Path.Combine(settingsPath, "PSUserConfig.txt");
                    string content = "";
                    content += "# Use WinTab\n";
                    content += "UseSystemStylus 0\n";
                    content += "WarnRunningScripts 0\n";
                    File.WriteAllText(conifg, content);
                    MessageBox.Show($"PSUserConfig.txt 配置成功\n 请重启 PhotoShop", "配置成功");
                }
            }
            else
            {
                MessageBox.Show("未能找到安装的 PhotoShop", "配置失败");

            }

        }
        private void EnableInk_BTN_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.CreateSubKey(@"SOFTWARE\Policies\Microsoft\WindowsInkWorkspace");
            hkSoftWare.SetValue("AllowWindowsInkWorkspace", "1", RegistryValueKind.DWord);
            hklm.Close();
            hkSoftWare.Close();
            MessageBox.Show("启用 WindwosInk", "配置成功");

        }
        private void DisableInk_BTN_Click(object sender, RoutedEventArgs e)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.CreateSubKey(@"SOFTWARE\Policies\Microsoft\WindowsInkWorkspace");
            hkSoftWare.SetValue("AllowWindowsInkWorkspace", "0", RegistryValueKind.DWord);
            hklm.Close();
            hkSoftWare.Close();
            MessageBox.Show("禁用 WindwosInk", "配置成功");
        }
    }
}
