using System;
using System.Windows;

namespace MusicApp;

public class AppSystem
{
    public Song? CurrentSong;

    public bool LoadAllMusic()
    {
        try
        {
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }

    public bool AddSong()
    {
        try
        {
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка");
            return false;
        }

        return true;
    }
}