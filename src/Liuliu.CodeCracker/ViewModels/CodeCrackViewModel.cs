// -----------------------------------------------------------------------
//  <copyright file="CodeCrackViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 13:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Liuliu.CodeCracker.Infrastructure;

using Newtonsoft.Json;

using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class CodeCrackViewModel : ViewModelExBase
    {
        public string[] Languages
        {
            get { return new[] { "eng" }; }
        }

        private string _language = "eng";
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value, () => Language); }
        }

        public IDictionary<string,string> CharDict
        {
            get
            {
                return new Dictionary<string, string>()
                {
                    {"0-9","0123456789" },
                    {"a-z","abcdefghijklmnopqrstuvwxyz" },
                    {"A-Z","ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                    {"a-zA-Z","abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                    {"0-9a-z","0123456789abcdefghijklmnopqrstuvwxyz" },
                    {"0-9A-Z","0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                    {"0-9a-zA-Z","0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" }
                };
            }
        }

        private string _charlist = "0-9a-zA-Z";
        public string CharList
        {
            get { return _charlist; }
            set { SetProperty(ref _charlist, value, () => CharList); }
        }

        private string _tessPath = "tessdata";
        public string TessPath
        {
            get { return _tessPath; }
            set { SetProperty(ref _tessPath, value, () => TessPath); }
        }

        public string[] PageSegModes
        {
            get
            {
                Type type = typeof(PageSegMode);
                string[] names = Enum.GetNames(type).Select((name, index) => $"{index}.{name}").ToArray();
                return names;
            }
        }

        private PageSegMode _pageSegMode = PageSegMode.SingleBlockVertText;
        public PageSegMode PageSegMode
        {
            get { return _pageSegMode; }
            set { SetProperty(ref _pageSegMode, value, () => PageSegMode); }
        }

        private string _crackResult;
        [JsonIgnore]
        public string CrackResult
        {
            get { return _crackResult; }
            set { SetProperty(ref _crackResult, value, () => CrackResult); }
        }

        [JsonIgnore]
        public ICommand TessPathBrowseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog()
                    {
                        SelectedPath = TessPath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    };
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    string folder = dialog.SelectedPath;
                    if (Directory.Exists(folder))
                    {
                        TessPath = folder;
                    }
                });
            }
        }

        public ICommand TessdataDownloadCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Process.Start("https://github.com/tesseract-ocr/tessdata/blob/master/eng.traineddata");
                });
            }
        }

        public override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            Messenger.Default.Send("CrackCode", "CodeCrackView");
        }
    }
}