using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using DirectTVRemote.Models;
using System.Diagnostics;
namespace DirectTVRemote.ViewModels {
    public class MainPageViewModel : ViewModelBase {
        public MainPageViewModel() {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) {
                Value = "Designtime value";
            }
        }
 
        string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState) {
            if (suspensionState.Any()) {
                Value = suspensionState[nameof(Value)]?.ToString();
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending) {
            if (suspending) {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args) {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public async void GotoDetailsPage() {
            var myDTV = new DirectTvAPI("http://192.168.1.73:8080");

            var results = await myDTV.TuneToChannel("36");
            Debug.WriteLine(results);
        }
        public async void StbPowerToggle() {
            var myDTV = new DirectTvAPI("http://192.168.1.73:8080");
            var results = await myDTV.KeyPress("poweron");
            Debug.WriteLine(results);
        }
        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

