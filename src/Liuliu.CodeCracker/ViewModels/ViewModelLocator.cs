using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;


namespace Liuliu.CodeCracker.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(()=>SimpleIoc.Default);
            SimpleIoc.Default.Register<ViewModelLocator>();
            RegisterViewModels();
        }
        
        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MainCommandViewModel>();
            SimpleIoc.Default.Register<CodeLoadViewModel>();
            SimpleIoc.Default.Register<CodeCrackViewModel>();
            SimpleIoc.Default.Register<CodeFilterViewModel>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public MainCommandViewModel MainCommand
        {
            get { return ServiceLocator.Current.GetInstance<MainCommandViewModel>(); }
        }
    }
}
