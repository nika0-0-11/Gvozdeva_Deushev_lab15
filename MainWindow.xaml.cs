using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Lab15;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int TotalPairs = 10;

    private readonly List<string> _originalEmojis = new List<string>() {
        "👽","👽", "🐴","🐴",
        "👾","👾", "🤖","🤖",
        "🐷","🐷", "🦁","🦁",
        "🐯","🐯", "🐑","🐑",
        "🐒","🐒", "🦈","🦈",
    };

    DispatcherTimer _timer = new DispatcherTimer();
    int _tenthsOfSecondsElapsed;
    int _matchesFound;
    private Random _random = new();
    TextBlock _lastTextBlockClicked;
    bool _findingMatch = false;

    public MainWindow()
    {
        InitializeComponent();
        InitializeTimer();
        SetUpGame();
    }

    private void SetUpGame() {
        AssignRandomEmojis();
        _timer.Start();
        _tenthsOfSecondsElapsed = 0;
        _matchesFound = 0;
    }

    private void TextBlock(object sender, MouseButtonEventArgs e) {
        if (!(sender is TextBlock clickedTextBlock && clickedTextBlock.Visibility == Visibility.Visible))
            return;

        if (!_findingMatch) {
            clickedTextBlock.Visibility = Visibility.Hidden;
            _lastTextBlockClicked = clickedTextBlock;
            _findingMatch = true;
            return;
        }

        if (clickedTextBlock == _lastTextBlockClicked)
            return;

        if (clickedTextBlock.Text == _lastTextBlockClicked.Text) {
            clickedTextBlock.Visibility = Visibility.Hidden;
            _matchesFound++;
        } else {
            _lastTextBlockClicked.Visibility = Visibility.Hidden;
        }

        _findingMatch = false;
    } 

    private void Timer_Tick(object sender, EventArgs e) {
        _tenthsOfSecondsElapsed++;
        timeTextBlock.Text = $"{_tenthsOfSecondsElapsed / 10F:0.0s}";
        if (_matchesFound == TotalPairs) {
            _timer.Stop();
            timeTextBlock.Text += " - Play again?";
        }
    }

    private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
        if (_matchesFound == TotalPairs) {
            SetUpGame();
        }
    }
    private void InitializeTimer() {
        _timer.Interval = TimeSpan.FromSeconds(.1);
        _timer.Tick += Timer_Tick;
    }

    private void AssignRandomEmojis() {
        var emojis = new List<string>(_originalEmojis);

        foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>()) {
            if (textBlock.Name != "timeTextBlock") {
                textBlock.Visibility = Visibility.Visible;
                int index = _random.Next(emojis.Count);
                string nextEmoji = emojis[index];
                textBlock.Text = nextEmoji;
                emojis.RemoveAt(index);
            }
        }

    }
}