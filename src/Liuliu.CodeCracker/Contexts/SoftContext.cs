// -----------------------------------------------------------------------
//  <copyright file="SoftContext.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 2:28</last-date>
// -----------------------------------------------------------------------

using Liuliu.CodeCracker.ViewModels;

using Microsoft.Practices.ServiceLocation;


namespace Liuliu.CodeCracker.Contexts
{
    public class SoftContext
    {
        public static ViewModelLocator Locator
        {
            get { return ServiceLocator.Current.GetInstance<ViewModelLocator>(); }
        }

        public static MainWindow MainWindow { get; set; }
    }
}