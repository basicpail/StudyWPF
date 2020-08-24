﻿using Prism.Ioc;
using Prism.Regions;
using System.ComponentModel;
using System.Windows;

namespace FirstPrismApp.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        IContainerExtension _container;
        IRegionManager _regionManager;
        IRegion _region;

        public MainView(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();

            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(SubView));
            _container = container;
            _regionManager = regionManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var view = _container.Resolve<SubView>();
            //var region = _regionManager.Regions["ContentRegion"];
            //region.Add(view);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
        }


    }
}
