// -----------------------------------------------------------------------
//  <copyright file="CodeCrackView.xaml.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 18:53</last-date>
// -----------------------------------------------------------------------

using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Messaging;

using Liuliu.CodeCracker.Contexts;
using Liuliu.CodeCracker.Infrastructure;
using Liuliu.CodeCracker.ViewModels;

using OSharp.Utility.Extensions;


namespace Liuliu.CodeCracker.UserControls
{
    /// <summary>
    /// CodeCrackView.xaml 的交互逻辑
    /// </summary>
    public partial class CodeCrackView : UserControl
    {
        public CodeCrackView()
        {
            InitializeComponent();
            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            Messenger.Default.Register<string>(this,
                "CodeCrackView",
                msg =>
                {
                    switch (msg)
                    {
                        case "CrackCode":
                            CrackCode();
                            break;
                    }
                });
        }

        private void CrackCode()
        {
            CodeLoadViewModel loadModel = SoftContext.Locator.Main.CodeLoad;
            if (loadModel.TargetImage == null)
            {
                return;
            }
            CodeCrackViewModel crackModel = SoftContext.Locator.Main.CodeCrack;
            string lang = crackModel.Language;
            if (lang.IsNullOrEmpty())
            {
                lang = "eng";
            }
            string charlist = crackModel.CharList;
            if (charlist.IsNullOrEmpty())
            {
                charlist = "0-9a-zA-Z";
            }
            if (crackModel.CharDict.ContainsKey(charlist))
            {
                charlist = crackModel.CharDict[charlist];
            }
            string tesspath = crackModel.TessPath;
            if (!Directory.Exists(tesspath))
            {
                MessageBox.Show($"字典路径“{tesspath}”不存在");
                return;
            }
            
            if (!File.Exists(Path.Combine(tesspath, $"{lang}.traineddata")))
            {
                MessageBox.Show($"字典路径“{tesspath}”无法找到字库“{lang}.traineddata”，请重新定位，或者到 https://github.com/tesseract-ocr/tessdata/blob/master/eng.traineddata 下载",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            SimpleCodeCracker cracker = new SimpleCodeCracker(lang, charlist, tesspath);
            string code = cracker.CrackCode(loadModel.TargetImage, crackModel.PageSegMode);
            crackModel.CrackResult = code;
        }


        private class SimpleCodeCracker : CodeCrackerBase
        {
            /// <summary>
            /// 初始化一个<see cref="SimpleCodeCracker"/>类型的新实例
            /// </summary>
            public SimpleCodeCracker(string language, string charlist, string tesspath)
                : base(language, charlist, tesspath)
            { }

            protected override Bitmap Binary(Bitmap bmp)
            {
                return bmp;
            }
        }
    }
}