using System.Windows;

namespace MusicApp
{
    public partial class MainWindow : Window
    {
        private Song? _song;
        private AppSystem? _appSystem;
        
        public MainWindow()
        {
            InitializeComponent();
            _appSystem = new AppSystem();
        }

        private void PauseSoundButtonClick(object sender, RoutedEventArgs e) => 
            _song?.PausePlay();

        private void PlaySoundButtonClick(object sender, RoutedEventArgs e) => 
            _song?.StartPlay();

        private void AddSoundButtonClick(object sender, RoutedEventArgs e) =>
            _song = new Song("Бара Бара Бере Бере", "./Music/test.mp3");

        private void SoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e) =>
            _song?.SetVolume(VolumeSlider.Value);
    }
}