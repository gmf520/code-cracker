// -----------------------------------------------------------------------
//  <copyright file="CodeFilterViewModel.cs" company="柳柳软件">
//      Copyright (c) 2017 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-27 15:10</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Newtonsoft.Json;

using OSharp.Utility.Wpf;


namespace Liuliu.CodeCracker.ViewModels
{
    public class CodeFilterViewModel : ViewModelExBase
    {
        public CodeFilterViewModel()
        {
            _filterNames = new[]
            {
                "Binaryzation",
                "DeepFore",
                "ClearNoiseRound",
                "ClearNoiseArea",
                "ClearBorder",
                "AddBorder",
                "ClearGray",
                "ToValid"
            };
            _filterItems = new ObservableCollection<CodeFilterItemViewModel>(new List<CodeFilterItemViewModel>
            {
                new CodeFilterItemViewModel() { FilterName = "ToGrayArray2D" }
            });
        }

        private string[] _filterNames;
        public string[] FilterNames
        {
            get { return _filterNames; }
            set { SetProperty(ref _filterNames, value, () => FilterNames); }
        }

        private ObservableCollection<CodeFilterItemViewModel> _filterItems;
        public ObservableCollection<CodeFilterItemViewModel> FilterItems
        {
            get { return _filterItems; }
            set { SetProperty(ref _filterItems, value, () => FilterItems); }
        }

        private string _filterCode;
        public string FilterCode
        {
            get { return _filterCode; }
            set { SetProperty(ref _filterCode, value, () => FilterCode); }
        }

        public ICommand SelectionChangedCommand
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(e =>
                {
                    if (e.OriginalSource is TextBlock || e.OriginalSource is Border)
                    {
                        return;
                    }
                    DataGrid grid = e.Source as DataGrid;
                    if (grid == null)
                    {
                        return;
                    }
                    CodeFilterItemViewModel filter = grid.SelectedValue as CodeFilterItemViewModel;
                    if (filter == null)
                    {
                        return;
                    }
                    Messenger.Default.Send(filter, "ShowFilterSettingView");
                });
            }
        }

        public override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            Messenger.Default.Send("UpdateImage", "CodeFilter");
        }
    }


    public class CodeFilterItemViewModel : ViewModelExBase
    {
        private bool _enabled = true;
        public bool Enabled
        {
            get { return _enabled; }
            set { SetProperty(ref _enabled, value, () => Enabled); }
        }

        private string _filterName;
        [Required(ErrorMessage = "必须选一个滤镜")]
        public string FilterName
        {
            get { return _filterName; }
            set { SetProperty(ref _filterName, value, () => FilterName); }
        }

        private string _filterArgs;
        public string FilterArgs
        {
            get { return _filterArgs; }
            set { SetProperty(ref _filterArgs, value, () => FilterArgs); }
        }

        [JsonIgnore]
        public ICommand FilterNameChangedCommand
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(e =>
                {
                    if (e.AddedItems.Count == 0)
                    {
                        return;
                    }
                    string name = e.AddedItems[0] as string;
                    if (name == null)
                    {
                        return;
                    }
                    Messenger.Default.Send(name, "CodeFilterInit");
                    Messenger.Default.Send("UpdateImage", "CodeFilterView");
                    Messenger.Default.Send("CrackCode", "CodeCrackView");
                });
            }
        }

        public override void RaisePropertyChanged(string propertyName)
        {
            base.RaisePropertyChanged(propertyName);
            if (propertyName == "FilterArgs")
            {
                Messenger.Default.Send("UpdateImage", "CodeFilterView");
                Messenger.Default.Send("CrackCode", "CodeCrackView");
            }
        }
    }
}