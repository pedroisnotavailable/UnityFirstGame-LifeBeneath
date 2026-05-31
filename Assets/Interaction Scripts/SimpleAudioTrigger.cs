using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class SimpleAudioTrigger : PlayerActivatable 
{
    public AudioClip audioClip;
    public AudioMixerGroup outputAudioMixerGroup;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 1f)] public float spatialBlend = 1f;
    public float minDistance = 1f;
    public float maxDistance = 25f;

    override protected void OnActivate()
    {        
        if (audioClip != null)
        {
            PlayAudioAndDestroy();
        }
    }

    private void PlayAudioAndDestroy()
    {
        GameObject audioObject = new GameObject("One Shot Audio");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = outputAudioMixerGroup;
        audioSource.volume = volume;
        audioSource.spatialBlend = spatialBlend;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;

        audioSource.Play();

        Destroy(audioObject, audioClip.length);
    }
}
