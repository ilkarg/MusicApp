using System;
using System.Windows;

namespace MusicApp
{
    public partial class MainWindow : Window
    {
        private Song? _song;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PauseSoundButtonClick(object sender, RoutedEventArgs e) => 
            _song?.PausePlay();

        private void PlaySoundButtonClick(object sender, RoutedEventArgs e) => 
            _song?.StartPlay();

        private void StopSoundButtonClick(object sender, RoutedEventArgs e) => 
            _song?.StopPlay();

        private void AddSoundButtonClick(object sender, RoutedEventArgs e) =>
            _song = new Song("Бара Бара Бере Бере", new Uri("./Music/test.mp3", UriKind.RelativeOrAbsolute));
    }
}