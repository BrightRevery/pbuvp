﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using GalaSoft.MvvmLight.Threading;

namespace UvpClient.Services {
    /// <summary>
    ///     根导航服务接口。
    /// </summary>
    public class RootNavigationService : IRootNavigationService {
        /// <summary>
        ///     导航。
        /// </summary>
        public void Navigate(Type sourcePageType) {
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                ((Frame) Window.Current.Content).Navigate(sourcePageType);
            });
        }

        /// <summary>
        ///     导航。
        /// </summary>
        public void Navigate(Type sourcePageType, object parameter) {
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                ((Frame) Window.Current.Content).Navigate(sourcePageType,
                    parameter);
            });
        }

        /// <summary>
        ///     导航。
        /// </summary>
        public void Navigate(Type sourcePageType, object parameter,
            NavigationTransition navigationTransition) {
            DispatcherHelper.CheckBeginInvokeOnUI(() => {
                NavigationTransitionInfo navigationTransitionInfo;
                switch (navigationTransition) {
                    case NavigationTransition.EntranceNavigationTransition:
                        navigationTransitionInfo =
                            new EntranceNavigationTransitionInfo();
                        break;
                    default:
                        navigationTransitionInfo =
                            new SuppressNavigationTransitionInfo();
                        break;
                }

                ((Frame) Window.Current.Content).Navigate(sourcePageType,
                    parameter, navigationTransitionInfo);
            });
        }
    }
}