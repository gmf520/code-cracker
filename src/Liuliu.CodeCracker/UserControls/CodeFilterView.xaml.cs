// -----------------------------------------------------------------------
//  <copyright file="CodeFilterView.xaml.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 15:54</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Messaging;

using Liuliu.CodeCracker.Contexts;
using Liuliu.CodeCracker.ViewModels;

using MahApps.Metro.Controls;

using OSharp.Utility.Extensions;


namespace Liuliu.CodeCracker.UserControls
{
    /// <summary>
    /// ImageFilterView.xaml 的交互逻辑
    /// </summary>
    public partial class CodeFilterView : UserControl
    {
        private string _code;

        public CodeFilterView()
        {
            InitializeComponent();
            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            Messenger.Default.Register<string>(this,"CodeFilter",
                msg =>
                {
                    switch (msg)
                    {
                        case "UpdateImage":
                            UpdateImage();
                            break;
                    }
                });
            Messenger.Default.Register<string>(this, "CodeFilterInit", name => CodeFilterInit(name));
        }

        private void CodeFilterInit(string name)
        {
            CodeFilterItemViewModel filter = CodeFilterDataGrid.SelectedValue as CodeFilterItemViewModel;
            if (filter == null)
            {
                return;
            }
            filter.FilterName = name;
            filter.FilterArgs = null;
            ShowFilterSettingView(filter);
        }

        private void ShowFilterSettingView(CodeFilterItemViewModel filter)
        {
            string name = filter.FilterName;
            WrapPanel panel = FilterArgsPanel;
            panel.Children.Clear();
            string args = filter.FilterArgs;
            string[] strs;
            switch (name)
            {
                case "ToGrayArray2D":
                    filter.FilterArgs = null;
                    break;
                case "Binaryzation":
                case "DeepFore":
                    AddNumericUpDown(panel, "灰度", filter.FilterArgs.CastTo(200));
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "200";
                    }
                    break;
                case "ClearNoiseRound":
                    strs = (args ?? "200,3").Split(',');
                    AddNumericUpDown(panel, "灰度", strs[0].CastTo(200));
                    AddNumericUpDown(panel, "点数", strs[1].CastTo(3), 1, 4);
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "200,3";
                    }
                    break;
                case "ClearNoiseArea":
                    strs = (args ?? "200,8").Split(',');
                    AddNumericUpDown(panel, "灰度", strs[0].CastTo(200));
                    AddNumericUpDown(panel, "面积", strs[1].CastTo(8), 1, 1000);
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "200,8";
                    }
                    break;
                case "ClearBorder":
                    AddNumericUpDown(panel, "边宽", (args ?? "1").CastTo(1), 1, 100);
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "1";
                    }
                    break;
                case "AddBorder":
                    strs = (args ?? "1,255").Split(',');
                    AddNumericUpDown(panel, "边宽", strs[0].CastTo(1), 1, 100);
                    AddNumericUpDown(panel, "灰度", strs[1].CastTo(255), 1, 100);
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "1,255";
                    }
                    break;
                case "ClearGray":
                    strs = (args ?? "255,255").Split(',');
                    AddNumericUpDown(panel, "小值", strs[0].CastTo(255));
                    AddNumericUpDown(panel, "大值", strs[1].CastTo(255));
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "255,255";
                    }
                    break;
                case "ToValid":
                    AddNumericUpDown(panel, "灰度", (args ?? "200").CastTo(1), 1, 100);
                    if (filter.FilterArgs == null)
                    {
                        filter.FilterArgs = "200";
                    }
                    break;
            }
        }

        private void AddNumericUpDown(Panel panel, string text, int value, int min = 0, int max = 255)
        {
            panel.Children.Add(new Label() { Content = $"{text}：" });
            NumericUpDown upDown = new NumericUpDown() { Value = value, Minimum = min, Maximum = max };
            upDown.ValueChanged += UpDown_ValueChanged;
            panel.Children.Add(upDown);
        }

        private void UpDown_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double?> e)
        {
            object row = CodeFilterDataGrid.SelectedItem;
            if (!(row is CodeFilterItemViewModel))
            {
                return;
            }
            //更新当前选择的
            CodeFilterItemViewModel filter = (CodeFilterItemViewModel)row;
            NumericUpDown[] upDowns = FilterArgsPanel.Children.Cast<UIElement>().OfType<NumericUpDown>().ToArray();
            filter.FilterArgs = upDowns.Length == 0 ? null : upDowns.Select(m => m.Value).ExpandAndToString();
        }

        private void UpdateImage()
        {
            _code = null;
            MainViewModel main = SoftContext.Locator.Main;
            CodeLoadViewModel load = main.CodeLoad;
            if (load.SourceImage==null)
            {
                return;
            }
            byte[,] bytes = load.SourceImage.ToGrayArray2D();
            _code += "return bmp.ToGrayArray2D()";
            foreach (CodeFilterItemViewModel filter in main.CodeFilter.FilterItems.Where(m=>m.Enabled))
            {
                bytes = CodeFilter(bytes, filter);
            }
            _code += ".ToBitmap();";
            main.CodeFilter.FilterCode = _code;
            main.CodeLoad.TargetImage = main.CodeLoad.ProcessImage = bytes.ToBitmap();
        }

        private byte[,] CodeFilter(byte[,] bytes, CodeFilterItemViewModel filter)
        {
            if (filter.FilterArgs == null)
            {
                return bytes;
            }
            int[] args = filter.FilterArgs.Split(',').Select(m => m.CastTo<int>()).ToArray();
            switch (filter.FilterName)
            {
                case "Binaryzation":
                    _code += $".Binaryzation({args[0]})";
                    return bytes.Binaryzation((byte)args[0]);
                case "DeepFore":
                    _code += $".DeepFore({args[0]})";
                    return bytes.DeepFore((byte)args[0]);
                case "ClearNoiseRound":
                    _code += $".ClearNoiseRound({args[0]}, {args[1]})";
                    return bytes.ClearNoiseRound((byte)args[0], args[1]);
                case "ClearNoiseArea":
                    _code += $".ClearNoiseArea({args[0]}, {args[1]})";
                    return bytes.ClearNoiseArea((byte)args[0], args[1]);
                case "ClearBorder":
                    _code += $".ClearBorder({args[0]})";
                    return bytes.ClearBorder(args[0]);
                case "AddBorder":
                    _code += $".AddBorder({args[0]}, {args[1]})";
                    return bytes.AddBorder(args[0], (byte)args[1]);
                case "ClearGray":
                    _code += $".ClearGray({args[0]},{args[1]})";
                    return bytes.ClearGray((byte)args[0], (byte)args[1]);
                case "ToValid":
                    _code += $".ToValid({args[0]})";
                    return bytes.ToValid((byte)args[0]);
            }
            return bytes;
        }
    }
}