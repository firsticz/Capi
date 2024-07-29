using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using KeyboardHookLite;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly KeyboardHook _keyboardHook;
        private readonly DispatcherTimer _resetTimer;
        private readonly string _initialImagePath = "image/01.png";

        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => DragMove();
            this.Focusable = true;
            this.Focus();
            this.Loaded += (s, e) => this.Focus();
            this.Activate();

            MyImage.Source = new BitmapImage(new Uri(_initialImagePath, UriKind.Relative));

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyboardPressed += OnKeyPress;

            _resetTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1)
            };
            _resetTimer.Tick += (s, e) => ResetImage();

            PositionWindowInBottomRight();
        }

        private void PositionWindowInBottomRight()
        {
            var screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            var screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

            var windowWidth = this.Width;
            var windowHeight = this.Height;

            this.Left = screenWidth - windowWidth + 200;
            this.Top = screenHeight - windowHeight;

            Debug.WriteLine($"Window positioned at: Left={Left}, Top={Top}");
        }

        private void OnKeyPress(object sender, KeyboardHookEventArgs e)
        {
            if (e.InputEvent.Key == Key.A)
            {
                string imagePath = "image/02.png";
                Dispatcher.Invoke(() => MyImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative)));
                StartResetTimer();
            }
            // Add more cases as needed
        }

        private void StartResetTimer()
        {
            _resetTimer.Stop(); // Stop any previous timer
            _resetTimer.Start(); // Start a new timer
        }

        private void ResetImage()
        {
            _resetTimer.Stop(); // Stop the timer
            Dispatcher.Invoke(() => MyImage.Source = new BitmapImage(new Uri(_initialImagePath, UriKind.Relative)));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _keyboardHook?.Dispose();
        }
    }
}