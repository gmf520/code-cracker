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

using Microsoft.Win32;

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
            set { SetProperty(ref _localPath, value, () => LocalPath); }
        }

        private string _codeUrl;
        public string CodeUrl
        {
            get { return _codeUrl; }
            set { SetProperty(ref _codeUrl, value, () => CodeUrl); }
        }

        private Bitmap _processImage;
        public Bitmap ProcessImage
        {
            get { return _processImage; }
            set { SetProperty(ref _processImage, value, () => ProcessImage); }
        }

        private Bitmap _sourceImage;
        public Bitmap SourceImage
        {
            get { return _sourceImage; }
            set { SetProperty(ref _sourceImage, value, () => SourceImage); }
        }

        private Bitmap _targetImage;
        public Bitmap TargetImage
        {
            get { return _targetImage; }
            set { SetProperty(ref _targetImage, value, () => TargetImage); }
        }

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
                        if (File.Exists(LocalPath))
                        {
                            Bitmap bmp = new Bitmap(LocalPath);
                            ProcessImage = SourceImage = TargetImage = bmp;
                        }
                    };
                    dialog.ShowDialog();
                });
            }
        }

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
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                Bitmap bmp = new Bitmap(ms);
                                ProcessImage = SourceImage = TargetImage = bmp;
                            }
                        }
                    };
                    client.DownloadDataAsync(new Uri(CodeUrl));
                });
            }
        }
    }
}