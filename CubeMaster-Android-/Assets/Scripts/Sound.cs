using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public enum Type
    {
        Music,
        Sounds
    }
    public Type type;
    public string name;
    [Range(0,100)]
    public int volume;
    [Range(0f,3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
