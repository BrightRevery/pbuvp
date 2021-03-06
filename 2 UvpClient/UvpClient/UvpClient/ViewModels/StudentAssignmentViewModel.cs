﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UvpClient.Models;
using UvpClient.Services;

namespace UvpClient.ViewModels {
    public class StudentAssignmentViewModel : ViewModelBase {
        /// <summary>
        ///     对话框服务。
        /// </summary>
        private readonly IDialogService _dialogService;

        /// <summary>
        ///     身份服务。
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     个人作业服务。
        /// </summary>
        private readonly IStudentAssignmentService _studentAssignmentService;

        /// <summary>
        ///     刷新命令。
        /// </summary>
        private RelayCommand _refreshCommand;

        /// <summary>
        ///     正在刷新。
        /// </summary>
        private bool _refreshing;

        /// <summary>
        ///     个人作业。
        /// </summary>
        private StudentAssignment _studentAssignment;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="dialogService">对话框服务。</param>
        /// <param name="identityService">身份服务。</param>
        /// <param name="studentAssignmentService">个人作业服务。</param>
        public StudentAssignmentViewModel(IDialogService dialogService,
            IIdentityService identityService,
            IStudentAssignmentService studentAssignmentService) {
            _dialogService = dialogService;
            _identityService = identityService;
            _studentAssignmentService = studentAssignmentService;
        }

        /// <summary>
        ///     正在刷新。
        /// </summary>
        public bool Refreshing {
            get => _refreshing;
            set => Set(nameof(Refreshing), ref _refreshing, value);
        }

        /// <summary>
        ///     个人作业id。
        /// </summary>
        public int StudentAssignmentId { get; set; }

        /// <summary>
        ///     个人作业。
        /// </summary>
        public StudentAssignment StudentAssignment {
            get => _studentAssignment;
            set =>
                Set(nameof(StudentAssignment), ref _studentAssignment, value);
        }

        /// <summary>
        ///     刷新命令。
        /// </summary>
        public RelayCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => {
                Refreshing = true;
                _refreshCommand.RaiseCanExecuteChanged();
                var serviceResult =
                    await _studentAssignmentService.GetAsync(
                        StudentAssignmentId);
                Refreshing = false;
                _refreshCommand.RaiseCanExecuteChanged();

                switch (serviceResult.Status) {
                    case ServiceResultStatus.Unauthorized:
                    case ServiceResultStatus.Forbidden:
                        break;
                    case ServiceResultStatus.OK:
                        StudentAssignment = serviceResult.Result;
                        break;
                    default:
                        await _dialogService.ShowAsync(
                            App.HttpClientErrorMessage + serviceResult.Message);
                        break;
                }
            }));
    }
}