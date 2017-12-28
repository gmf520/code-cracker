// -----------------------------------------------------------------------
//  <copyright file="MainCommandViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-28 13:39</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;

using Liuliu.CodeCracker.Contexts;

using Microsoft.Win32;

using OSharp.Utility.Extensions;
using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class MainCommandViewModel : ViewModelExBase
    {
        private string _configFile, _configPath = "configs";

        public ICommand BrowseConfigCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    OpenFileDialog dialog = new OpenFileDialog()
                    {
                        Filter = "配置文件(*.json)|*.json"
                    };
                    dialog.FileOk += (sender, e) =>
                    {
                        _configFile = dialog.FileName;
                        string[] lines = File.ReadAllLines(_configFile).Where(m => !m.IsNullOrWhiteSpace()).ToArray();
                        if (lines.Length != 3)
                        {
                            MessageBox.Show("配置文件格式错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        MainViewModel main = SoftContext.Locator.Main;
                        try
                        {
                            CodeLoadViewModel codeLoad = lines[0].FromJsonString<CodeLoadViewModel>();
                            CodeFilterViewModel codeFilter = lines[1].FromJsonString<CodeFilterViewModel>();
                            codeFilter.FilterItems.RemoveAt(0);
                            CodeCrackViewModel codeCrack = lines[2].FromJsonString<CodeCrackViewModel>();
                            main.CodeLoad = codeLoad;
                            main.CodeFilter = codeFilter;
                            main.CodeCrack = codeCrack;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"配置文件格式错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        main.Statusbar = $"配置文件“{Path.GetFileName(_configFile)}”加载成功";
                    };
                    dialog.ShowDialog();
                });
            }
        }

        public ICommand SaveConfigCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MainViewModel main = SoftContext.Locator.Main;
                    List<string> lines = new List<string>
                    {
                        main.CodeLoad.ToJsonString(),
                        main.CodeFilter.ToJsonString(),
                        main.CodeCrack.ToJsonString()
                    };
                    if (_configFile == null)
                    {
                        if (!Directory.Exists(_configPath))
                        {
                            Directory.CreateDirectory(_configPath);
                        }
                        _configFile = $"config-{DateTime.Now.ToString("HHmmss")}.json";
                    }

                    SaveFileDialog dialog = new SaveFileDialog()
                    {
                        Filter = "配置文件(*.json)|*.json",
                        FileName = _configFile,
                        AddExtension = true
                    };
                    dialog.FileOk += (sender, e) =>
                    {
                        File.WriteAllLines(_configFile, lines);
                        main.Statusbar = $"配置文件“{Path.GetFileName(_configFile)}”保存成功";
                    };
                    dialog.ShowDialog();
                });
            }
        }
    }
}