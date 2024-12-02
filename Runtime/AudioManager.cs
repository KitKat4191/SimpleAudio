
using UnityEngine;
using System.Collections.Generic;
using KitKat.SimpleAudio.Runtime;

[AddComponentMenu("SimpleAudio/AudioManager")]
public class AudioManager : MonoBehaviour
{
    [Header("Audio Configuration")]
    [SerializeField] private Sound[] sounds;

    
    private readonly Dictionary<string, Sound> _sounds = new();
    
    
    #region API

    private static AudioManager _instance;
    
    /// <summary>
    /// Plays a sound.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    /// <returns>If the operation was successful.</returns>
    public static bool Play(string soundName) => _instance.PlayInstance(soundName);
    
    /// <summary>
    /// Stops a sound.
    /// </summary>
    /// <param name="soundName">The name of the sound to stop.</param>
    /// <returns>If the operation was successful.</returns>
    public static bool Stop(string soundName) => _instance.StopPlayInstance(soundName);
    
    #endregion // API
    
    #region INTERNAL

    private void OnValidate()
    {
        if (sounds == null) return;
        if (sounds.Length == 0) return;
        
        foreach (Sound sound in sounds)
            sound.OnValidate();
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this);
            return;
        }
        
        _instance = this;
        
        DontDestroyOnLoad(gameObject);

        foreach (var sound in sounds)
        {
            var newSource = gameObject.AddComponent<AudioSource>();
            sound.SetAudioSource(newSource);
            _sounds.Add(sound.Name, sound);
        }
    }

    private bool PlayInstance(string soundName)
    {
        if (!_sounds.TryGetValue(soundName, out var sound))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return false;
        }

        sound.Play();
        return true;
    }

    private bool StopPlayInstance(string soundName)
    {
        if (!_sounds.TryGetValue(soundName, out var sound))
        {
            Debug.LogWarning($"Sound: {soundName} not found!");
            return false;
        }

        sound.Stop();
        return true;
    }
    
    #endregion // INTERNAL
}