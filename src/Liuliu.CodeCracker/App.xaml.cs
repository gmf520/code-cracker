using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Liuliu.CodeCracker.Contexts;

using OSharp.Utility.Extensions;


namespace Liuliu.MouseClicker
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 初始化一个<see cref="App"/>类型的新实例
        /// </summary>
        public App()
        {
            //注册全局事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            const string msg = "未捕获主线程异常";
            try
            {
                Exception exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        Exception ex = (Exception)exception;
                        HandleException(msg, ex);
                        FatalReport(ex);
                    }));
                }
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            const string msg = "未捕获子线程异常";
            try
            {
                HandleException(msg, e.Exception);
                e.Handled = true;
                FatalReport(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            const string msg = "未捕获异步异常";
            try
            {
                HandleException(msg, e.Exception);
                e.SetObserved();
                FatalReport(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void HandleException(string msg, Exception ex)
        {
            //log error
        }

        private void FatalReport(Exception exception)
        {
            InvalidCastException castException = exception as InvalidCastException;
            if (castException != null && castException.Message.Contains(".Windows.Media.Visual"))
            {
                return;
            }
            
            List<string> lines = new List<string>();
            while (exception != null)
            {
                lines.Add(exception.Message);
                exception = exception.InnerException;
            }
            MessageBox.Show("程序错误", $"错误消息：{lines.ExpandAndToString("\r\n---")}", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
