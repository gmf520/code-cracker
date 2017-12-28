// -----------------------------------------------------------------------
//  <copyright file="MainViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 2:12</last-date>
// -----------------------------------------------------------------------

using Microsoft.Practices.ServiceLocation;

using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class MainViewModel : ViewModelExBase
    {
        private string _title = "柳柳验证码助手";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value, () => Title); }
        }

        private string _statusbar;
        public string Statusbar
        {
            get { return _statusbar; }
            set { SetProperty(ref _statusbar, value, () => Statusbar); }
        }
        
        private CodeLoadViewModel _codeLoad= ServiceLocator.Current.GetInstance<CodeLoadViewModel>();
        public CodeLoadViewModel CodeLoad
        {
            get { return _codeLoad; }
            set { SetProperty(ref _codeLoad, value, () => CodeLoad); }
        }

        private CodeFilterViewModel _codeFilter= ServiceLocator.Current.GetInstance<CodeFilterViewModel>();
        public CodeFilterViewModel CodeFilter
        {
            get { return _codeFilter; }
            set { SetProperty(ref _codeFilter, value, () => CodeFilter); }
        }

        private CodeCrackViewModel _codeCrack = ServiceLocator.Current.GetInstance<CodeCrackViewModel>();
        public CodeCrackViewModel CodeCrack
        {
            get { return _codeCrack; }
            set { SetProperty(ref _codeCrack, value, () => CodeCrack); }
        }
    }
}