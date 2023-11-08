using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Created a struct to map strings to lists of audio files so that
// they can be edited directly from the inspector. You cannot do this using dictionaries.
[Serializable]
public struct SFXMap
{
    public SFXManager.SFXCategory SFXCategory;
    public List<AudioClip> SFXClips;
}

public class SFXManager : MonoBehaviour
{
    public enum SFXCategory
    {
        DoorOpen,
        ChatMessage,
        InvalidInteraction,
        OxygenLoss,
        PickupSound,
        PowerLoss,
        SurvivorDown,
        MovingDebris,
        SingleKeyClick,
        VentStep,
        HealthRestore,
        ElevatorStart,
        ElevatorContinuous,
        ElevatorEnd
    }
    
    public static SFXManager s;
    
    private static Dictionary<SFXCategory, List<AudioClip>> sfxDictionary = new Dictionary<SFXCategory, List<AudioClip>>();
    private AudioSource audioSource;
    private System.Random random;

    [SerializeField]
    private bool debugSound = false;
    
    [SerializeField] private List<SFXMap> maps;

    void Awake()
    {
        // singleton code
        if (s == null)
        {
            s = this;
        }
        else if (s != this) 
        {
            s = this;
            Debug.LogWarning("Multiple SFXManager scripts in scene."); 
        }
        
        foreach (SFXMap map in maps)
        {
            if (!sfxDictionary.ContainsKey(map.SFXCategory))
            {
                sfxDictionary.Add(map.SFXCategory, map.SFXClips);
            }
        }

        random = new System.Random();

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource attached to GameManager.");
        }
    }

    public void PlaySoundAtRandom(SFXCategory category)
    {
        int upperBound = sfxDictionary[category].Count - 1;
        PlaySound(category, random.Next(0, upperBound));
    }

    public void PlaySound(SFXCategory category, int index = 0)
    {
        // Get rid of this later if it screws something up. 
        if (sfxDictionary.ContainsKey(category) && sfxDictionary[category].Count > index
        && (!audioSource.isPlaying || !sfxDictionary[category].Contains(audioSource.clip)))
        {
            audioSource.PlayOneShot(sfxDictionary[category][index]);
            if (debugSound)
            {
                Debug.Log("Played sound effect: " + sfxDictionary[category][index]);
            }
            
        }
    }
    
    public void StopSound(SFXCategory category, int index = 0)
    {
        /*if (IsPlaying(category, index))
        {
            audioSource.Stop();
            if (debugSound)
            {
                Debug.Log("Stopped sound effect: " + sfxDictionary[category][index]);
            }
            
        }*/
        audioSource.Stop();
        if (debugSound)
        {
            Debug.Log("Stopped sound effect: " + sfxDictionary[category][index]);
        }
    }

    public bool IsPlaying(SFXCategory category, int index = 0)
    {
        return sfxDictionary.ContainsKey(category) && sfxDictionary[category].Count > index
                                                   && audioSource.isPlaying &&
                                                       sfxDictionary[category].Contains(audioSource.clip);
    }
}