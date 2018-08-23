using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    [HideInInspector]
    public float s_volume;
    [HideInInspector]
    public float m_volume;

    public AudioSource musicPlayer;
    public AudioSource soundPlayer;

    [Range(0f, 1f)]
    public float musicVolume;
    [Range(0f, 1f)]
    public float soundVolume;

    protected static AudioManager manager;

    public enum sType
    {sound,music}


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

        if (!System.IO.Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/");
        }


        string path = Application.persistentDataPath + "/SaveData/Configuration.sv";
        if (!System.IO.File.Exists(path))
        {
            System.IO.File.Create(path).Close();
            s_volume = 1;
            m_volume = 0.5f;
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

        soundPlayer.volume = soundVolume * s_volume;
        musicPlayer.volume = musicVolume * m_volume;


        DontDestroyOnLoad(gameObject);
    }

   

    public void RefreshVolume(float sound, float music)
    {
       
        s_volume = sound;
        m_volume = music;

        soundPlayer.volume = soundVolume * s_volume;
        musicPlayer.volume = musicVolume * m_volume;

        string[] str = new string[2];
        str[0] = s_volume.ToString();
        str[1] = m_volume.ToString();
        string path = Application.persistentDataPath + "/SaveData/Configuration.sv";

        System.IO.File.Create(path).Close();
        System.IO.File.WriteAllLines(path, str);

    }



    public void Play(string name, sType type)
    {
        Sound sound = Array.Find(sounds, x => x.name == name);

        switch (type)
        {
            case sType.music:
                musicPlayer.Stop();
                musicPlayer.clip = sound.clip;
                musicPlayer.Play();
                break;

            case sType.sound:
                soundPlayer.PlayOneShot(sound.clip);
                break;
        }
     
    }
}
