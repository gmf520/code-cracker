// -----------------------------------------------------------------------
//  <copyright file="CodeLoadViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 11:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Microsoft.Win32;

using Newtonsoft.Json;

using OSharp.Utility.Extensions;
using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class CodeLoadViewModel : ViewModelExBase
    {
        private bool _fromLocal = true;
        public bool FromLocal
        {
            get { return _fromLocal; }
            set { SetProperty(ref _fromLocal, value, () => FromLocal); }
        }

        private string _localPath;
        public string LocalPath
        {
            get { return _localPath; }
            set
            {
                SetProperty(ref _localPath, value, () => LocalPath);
                if (value != null && File.Exists(value))
                {
                    byte[] bytes = File.ReadAllBytes(value);
                    LoadImage(bytes);
                }
            }
        }

        private string _codeUrl;
        public string CodeUrl
        {
            get { return _codeUrl; }
            set { SetProperty(ref _codeUrl, value, () => CodeUrl); }
        }

        private Bitmap _processImage;
        [JsonIgnore]
        public Bitmap ProcessImage
        {
            get { return _processImage; }
            set { SetProperty(ref _processImage, value, () => ProcessImage); }
        }

        private Bitmap _sourceImage;
        [JsonIgnore]
        public Bitmap SourceImage
        {
            get { return _sourceImage; }
            set { SetProperty(ref _sourceImage, value, () => SourceImage); }
        }

        private Bitmap _targetImage;
        [JsonIgnore]
        public Bitmap TargetImage
        {
            get { return _targetImage; }
            set { SetProperty(ref _targetImage, value, () => TargetImage); }
        }

        [JsonIgnore]
        public ICommand CodeBrowseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    OpenFileDialog dialog = new OpenFileDialog()
                    {
                        Filter = "图像文件(*.bmp,*.jpg,*,png,*,tif)|*.bmp;*.jpg;*.png;*.tif|所有文件(*.*)|*.*",
                        FileName = LocalPath
                    };
                    dialog.FileOk += (sender, e) =>
                    {
                        LocalPath = dialog.FileName;
                    };
                    dialog.ShowDialog();
                });
            }
        }

        [JsonIgnore]
        public ICommand RemoteRefreshCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (CodeUrl == null || !CodeUrl.IsUrl())
                    {
                        MessageBox.Show("网络验证码的URL不能为空");
                        return;
                    }
                    WebClient client = new WebClient();
                    client.DownloadDataCompleted += (sender, e) =>
                    {
                        byte[] bytes = e.Result;
                        if (bytes != null)
                        {
                            LoadImage(bytes);
                        }
                    };
                    client.DownloadDataAsync(new Uri(CodeUrl));
                });
            }
        }

        private void LoadImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Bitmap bmp = new Bitmap(ms);
                ProcessImage = SourceImage = TargetImage = bmp;
            }

            Messenger.Default.Send("UpdateImage", "CodeFilterView");
            Messenger.Default.Send("CrackCode", "CodeCrackView");
        }
    }
}