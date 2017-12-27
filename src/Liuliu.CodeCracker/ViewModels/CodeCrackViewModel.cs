// -----------------------------------------------------------------------
//  <copyright file="CodeCrackViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 13:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Liuliu.CodeCracker.Infrastructure;

using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class CodeCrackViewModel : ViewModelExBase
    {
        private string _language;
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value, () => Language); }
        }

        private string _charlist;
        public string CharList
        {
            get { return _charlist; }
            set { SetProperty(ref _charlist, value, () => CharList); }
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
        public string CrackResult
        {
            get { return _crackResult; }
            set { SetProperty(ref _crackResult, value, () => CrackResult); }
        }
    }
}