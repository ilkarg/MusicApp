using System;
using System.Windows;

namespace MusicApp
{
    public partial class MainWindow : Window
    {
        private readonly AppSystem _appSystem;
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _appSystem = new AppSystem();
                _appSystem.LoadAllMusic();
                _appSystem.SongList.Add(new Song("Тестовая песня", "./Music/test.mp3"));
                _appSystem.CurrentSong = _appSystem.SongList[0];
                _appSystem.CurrentSong.SetVolume(1.0);
                VolumeSlider.Value = _appSystem.CurrentSong.Volume * 100;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.StackTrace}", "Ошибка");
            }
        }

        private void PauseSoundButtonClick(object sender, RoutedEventArgs e) => 
            _appSystem.CurrentSong.PausePlay();

        private void PlaySoundButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _appSystem.CurrentSong.StartPlay();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.StackTrace, "Ошибка");
            }
        }

        private void AddSoundButtonClick(object sender, RoutedEventArgs e) =>
            _appSystem.AddSong();

        private void SoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e) =>
            _appSystem.CurrentSong.SetVolume(VolumeSlider.Value / 100);
    }
}