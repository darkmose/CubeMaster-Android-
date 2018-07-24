using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public float s_volume;
    [HideInInspector]
    public float m_volume;
    AudioSource current;

    protected static AudioManager manager;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        string path = Application.persistentDataPath + "/SaveData/Configuration.sv";
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path).Close();
            s_volume = 1;
            m_volume = 1;
            string[] strs = new string[2];
            strs[0] = s_volume.ToString();
            strs[1] = m_volume.ToString();
            System.IO.File.WriteAllLines(path, strs);
        }
        else
        {
            string[] str = System.IO.File.ReadAllLines(path);
            s_volume = Convert.ToSingle(str[0]);
            m_volume = Convert.ToSingle(str[1]);
        } 

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;

            if (s.type == Sound.Type.Music)
            {
                s.source.volume = s.volume * m_volume/100;
            }
            else
            {
                s.source.volume = s.volume * s_volume/100;
            }
        }
    }

   

    public void RefreshVolume(float sound, float music)
    {
       
        s_volume = sound;
        m_volume = music;

        foreach (Sound s in sounds)
        {
            if (s.type == Sound.Type.Music)
            {
                s.source.volume = s.volume * m_volume/100;
            }
            else
            {
                s.source.volume = s.volume * s_volume/100;
            }            
        }

        string[] str = new string[2];
        str[0] = s_volume.ToString();
        str[1] = m_volume.ToString();
        string path = Application.persistentDataPath + "/SaveData/Configuration.sv";
        System.IO.File.WriteAllLines(path, str);

    }


    public void Play(string name, bool record)
    {
        if (current && record)
        {
            current.Stop();
        }

        Sound sound = Array.Find(sounds, x => x.name == name);
        
        if (record)
        {
            current = sound.source;
        }

        sound.source.Play();
    }
}
