using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoldenBook.Helpers
{
    public class ActivityIndicatorDisposable : IDisposable
    {
        private bool _showIndicator;
        private ActivityIndicator _indicator;
        private Task _indicatorDelay;

        public ActivityIndicatorDisposable(ActivityIndicator indicator, bool showIndicator)
        {
            _indicator = indicator;
            _showIndicator = showIndicator;

            if (showIndicator)
            {
                _indicatorDelay = Task.Delay(2000);
                SetIndicatorActivity(true);
            }
            else
            {
                _indicatorDelay = Task.FromResult(0);
            }
        }

        private void SetIndicatorActivity(bool isActive)
        {
            _indicator.IsVisible = isActive;
            _indicator.IsRunning = isActive;
        }

        public void Dispose()
        {
            if (_showIndicator)
            {
                _indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
    }
}
