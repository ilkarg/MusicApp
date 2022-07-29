using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MusicApp;

public class Song
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public Dictionary<string, int> Duration { get; set; }
    public double Volume { get; set; }
    public bool IsPlayed { get; set; }
    private readonly MediaPlayer _mediaPlayer;

    public Song(string? name, string? path)
    {
        try
        {
            Name = name;
            Path = path;
            IsPlayed = false;
            Duration = new Dictionary<string, int>();
            Duration.Add("Hours", 0);
            Duration.Add("Minutes", 0);
            Duration.Add("Seconds", 0);
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(new Uri(Path, UriKind.RelativeOrAbsolute));
            _mediaPlayer.MediaFailed += (s, e) => MessageBox.Show(e.ErrorException.Message, "Ошибка");
            _mediaPlayer.MediaOpened += (s, e) =>
            {
                Duration["Hours"] = _mediaPlayer.NaturalDuration.TimeSpan.Hours;
                Duration["Minutes"] = _mediaPlayer.NaturalDuration.TimeSpan.Minutes;
                Duration["Seconds"] = _mediaPlayer.NaturalDuration.TimeSpan.Seconds;

                Volume = _mediaPlayer.Volume;
            };
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
        }
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

    public bool SetVolume(double volume)
    {
        try
        {
            _mediaPlayer.Volume = volume;
            Volume = volume;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public double GetVolume()
    {
        double volume = 0.0;
        try
        {
            volume = _mediaPlayer.Volume;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return 0.0;
        }

        return volume;
    }
}