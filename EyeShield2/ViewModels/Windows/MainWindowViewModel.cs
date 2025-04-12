using System.Windows.Threading;

namespace EyeShield2.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    private DispatcherTimer _countdownTimer;
    private bool _started;

    private readonly TimeSpan WorkingTimeInterval = TimeSpan.FromSeconds((20 * 60) - 1);
    private readonly TimeSpan EyeRelaxInterval = TimeSpan.FromSeconds(3);
    private readonly TimeSpan TimerInterval = TimeSpan.FromMilliseconds(10);

    [ObservableProperty]
    private bool _isWork = true;

    [ObservableProperty]
    private bool _isRest = false;

    [ObservableProperty]
    private string _timerDisplay;

    [ObservableProperty]
    private TimeSpan _timeRemaining;

    [ObservableProperty]
    private string _buttonText = "Start";

    [ObservableProperty]
    private double _maxProgress = 100;

    [ObservableProperty]
    private double _progress = 100;

    public MainWindowViewModel()
    {
        TimeRemaining = WorkingTimeInterval;
        MaxProgress = WorkingTimeInterval.TotalSeconds;
        Progress = MaxProgress;

        _countdownTimer = new DispatcherTimer(DispatcherPriority.DataBind);
        _countdownTimer.Interval = TimerInterval;
        _countdownTimer.Tick += CountdownTimer_Tick;
    }

    [RelayCommand]
    private async Task StartStop()
    {
        _countdownTimer.Stop();

        if (!_started)
        {
            _countdownTimer.Start();
            ButtonText = "Stop";

            await StartWork();
        }
        else
        {
            ButtonText = "Start";
        }

        _started = !_started;
    }

    private async void CountdownTimer_Tick(object? sender, EventArgs e)
    {
        if (TimeRemaining.TotalMilliseconds > 0)
        {
            TimeRemaining = TimeRemaining.Subtract(TimerInterval);
            Progress = TimeRemaining.TotalSeconds;
        }
        else
        {
            // Time's up

            _countdownTimer.Stop();

            if (IsWork)
            {
                await StartRest(true);
            }
            else
            {
                await StartWork(true);
            }

            _countdownTimer.Start();
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

        TimeRemaining = WorkingTimeInterval;
        MaxProgress = WorkingTimeInterval.TotalSeconds;

    }

    private async Task StartRest(bool playSound = false)
    {
        IsWork = false;
        IsRest = true;
        
        if (playSound)
        {
            await PlaySound(1000, 500, 30, 2);
        }

        TimeRemaining = EyeRelaxInterval;
        MaxProgress = EyeRelaxInterval.TotalSeconds;
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

    partial void OnTimeRemainingChanged(TimeSpan oldValue, TimeSpan newValue)
    {
        TimerDisplay = newValue.TotalSeconds switch
        {
            > 60 => ((int)newValue.TotalMinutes + 1).ToString() + " minutes remaining",
            <= 60 and > 5 => newValue.ToString(@"mm\:ss"),
            <= 5 and > -10 => newValue.ToString(@"mm\:ss\:ff"),
            _ => "No time left"
        };
    }
}
