using System;
using System.Windows;
using System.Windows.Media;

namespace MusicApp;

public class Song
{
    public string? Name { get; set; }
    public string Path { get; set; }
    public bool IsPlayed { get; set; }
    private readonly MediaPlayer _mediaPlayer;

    public Song(string? name, string path)
    {
        Name = name;
        Path = path;
        IsPlayed = false;
        _mediaPlayer = new MediaPlayer();
        _mediaPlayer.Open(new Uri(Path, UriKind.RelativeOrAbsolute));
        _mediaPlayer.MediaFailed += (s, e) => MessageBox.Show(e.ErrorException.Message, "Ошибка");
    }

    public bool StartPlay()
    {
        try
        {
            _mediaPlayer.Play();
            IsPlayed = true;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }
        
        return true;
    }

    public bool PausePlay()
    {
        try
        {
            _mediaPlayer.Pause();
            IsPlayed = false;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public bool StopPlay()
    {
        try
        {
            _mediaPlayer.Stop();
            IsPlayed = false;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }
}