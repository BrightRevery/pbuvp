﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Pages;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    /// <summary>
    ///     登录ViewModel。
    /// </summary>
    public class LoginViewModel : ViewModelBase {
        /// <summary>
        ///     登录错误信息。
        /// </summary>
        public const string LoginErrorMessage =
            "Sorry!!!\n\nAn error occurred when we tried to sign you in.\nPlease screenshot this dialog and send it to your teacher.\n\nError:\n";

        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     根导航服务。
        /// </summary>
        private readonly IRootNavigationService _rootNavigationService;

        /// <summary>
        ///     登录命令。
        /// </summary>
        private RelayCommand _loginCommand;

        /// <summary>
        ///     正在登录。
        /// </summary>
        private bool _signingIn;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="identityService">身份服务。</param>
        /// <param name="rootNavigationService">根导航服务。</param>
        /// <param name="dialogService">对话框服务。</param>
        public LoginViewModel(IIdentityService identityService,
            IRootNavigationService rootNavigationService,
            IDialogService dialogService) {
            _identityService = identityService;
            _rootNavigationService = rootNavigationService;
            _dialogService = dialogService;
        }

        /// <summary>
        ///     正在登录。
        /// </summary>
        public bool SigningIn {
            get => _signingIn;
            set => Set(nameof(SigningIn), ref _signingIn, value);
        }

        /// <summary>
        ///     登录命令。
        /// </summary>
        public RelayCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new RelayCommand(async () => {
                SigningIn = true;
                _loginCommand.RaiseCanExecuteChanged();
                var serviceResult = await _identityService.LoginAsync();
                SigningIn = false;
                _loginCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.OK:
                        _rootNavigationService.Navigate(typeof(MyUvpPage), null,
                            NavigationTransition.EntranceNavigationTransition);
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            LoginErrorMessage + serviceResult.Message);
                        break;
                }
            }, () => !SigningIn));
    }
}