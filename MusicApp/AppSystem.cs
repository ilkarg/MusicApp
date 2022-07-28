using System;
using System.Collections.Generic;
using System.Windows;

namespace MusicApp;

public class AppSystem
{
    public Song CurrentSong;
    public List<Song> SongList;

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