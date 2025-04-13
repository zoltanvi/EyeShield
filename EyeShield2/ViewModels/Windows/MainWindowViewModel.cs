using System.Diagnostics;
using System.Windows.Threading;

namespace EyeShield2.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    private DispatcherTimer _timer;
    private Stopwatch _stopWatch;
    
    private readonly TimeSpan TimerInterval = TimeSpan.FromMilliseconds(10);

    [ObservableProperty]
    private bool _isStarted;

    [ObservableProperty]
    private bool _isCounterPageOpen = true;

    [ObservableProperty]
    private bool _isSettingsPageOpen = false;

    [ObservableProperty]
    private bool _isWork = true;

    [ObservableProperty]
    private bool _isRest = false;

    [ObservableProperty]
    private string _timerDisplay;

    [ObservableProperty]
    private double _workPeriodMinutes = 20;

    [ObservableProperty]
    private double _restPeriodSeconds = 20;

    [ObservableProperty]
    private TimeSpan _timeRemaining;

    [ObservableProperty]
    private double _maxProgressSeconds;

    [ObservableProperty]
    private double _progressSeconds;

    public MainWindowViewModel()
    {
        UpdateWorkPeriod();
        ProgressSeconds = MaxProgressSeconds;

        _stopWatch = new Stopwatch();
        _timer = new DispatcherTimer(DispatcherPriority.DataBind);
        _timer.Interval = TimerInterval;
        _timer.Tick += CountdownTimer_Tick;
    }

    [RelayCommand]
    private async Task StartStop()
    {
        _timer.Stop();
        _stopWatch.Stop();
        UpdateWorkPeriod();

        if (!IsStarted)
        {
            _timer.Start();
            _stopWatch.Restart();

            await StartWork();
        }

        IsStarted = !IsStarted;
    }

    [RelayCommand]
    private async Task OpenSettings()
    {
        IsCounterPageOpen = false;
        IsSettingsPageOpen = true;
    }

    [RelayCommand]
    private async Task OpenCounter()
    {
        IsCounterPageOpen = true;
        IsSettingsPageOpen = false;
    }

    private async void CountdownTimer_Tick(object? sender, EventArgs e)
    {
        if (TimeRemaining.TotalMilliseconds > 0)
        {
            TimeRemaining = TimeSpan.FromMilliseconds((MaxProgressSeconds * 1000) - _stopWatch.ElapsedMilliseconds);
            ProgressSeconds = TimeRemaining.TotalSeconds;
        }
        else
        {
            // Time's up

            _timer.Stop();
            _stopWatch.Stop();

            if (IsWork)
            {
                await StartRest(true);
            }
            else
            {
                await StartWork(true);
            }

            _timer.Start();
            _stopWatch.Restart();
        }
    }

    private async Task StartWork(bool playSound = false)
    {
        IsWork = true;
        IsRest = false;

        if (playSound)
        {
            await PlaySound(1000, 2000, 10, 2);
        }

        UpdateWorkPeriod();
    }

    private async Task StartRest(bool playSound = false)
    {
        IsWork = false;
        IsRest = true;
        
        if (playSound)
        {
            await PlaySound(1000, 500, 30, 2);
        }

        UpdateRestPeriod();
    }

    private async Task PlaySound(int firstFrequency, int secondFrequency, int beepDelay, int beepRepeat)
    {
        for (int i = 0; i < beepRepeat; i++)
        {
            Console.Beep(firstFrequency, beepDelay);
            await Task.Delay(beepDelay);
            Console.Beep(secondFrequency, beepDelay);
        }
    }

    partial void OnWorkPeriodMinutesChanged(double value)
    {
        _timer.Stop();
        _stopWatch.Stop();
        IsStarted = false;

        UpdateWorkPeriod();
    }

    private void UpdateWorkPeriod()
    {
        MaxProgressSeconds = WorkPeriodMinutes * 60;
        ProgressSeconds = MaxProgressSeconds;
        TimeRemaining = TimeSpan.FromSeconds(MaxProgressSeconds);
    }

    partial void OnRestPeriodSecondsChanged(double oldValue, double newValue)
    {
        _timer.Stop();
        _stopWatch.Stop();
        IsStarted = false;
        IsWork = true;
        IsRest = false;

        UpdateWorkPeriod();
    }

    private void UpdateRestPeriod()
    {
        MaxProgressSeconds = RestPeriodSeconds;
        TimeRemaining = TimeSpan.FromSeconds(MaxProgressSeconds);
    }
}
