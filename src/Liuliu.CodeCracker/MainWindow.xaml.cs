// -----------------------------------------------------------------------
//  <copyright file="MainWindow.xaml.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 2:28</last-date>
// -----------------------------------------------------------------------

using System.Windows;

using Liuliu.CodeCracker.Contexts;
using Liuliu.CodeCracker.ViewModels;


namespace Liuliu.CodeCracker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            SoftContext.MainWindow = this;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel main = SoftContext.Locator.Main;
            main.Statusbar = "准备就绪";
        }
    }
}