using UnityEngine;

public class EngineAudio
{
    private AudioSource audioSource;// Reference to the audio source used to play engine sounds.
    private AudioClip engineIdling;// Audio to play when the tank isn't moving.
    private AudioClip engineDriving;// Audio to play when the tank is moving.
    private float pitchRange;// The amount by which the pitch of the engine noises can vary.

    public EngineAudio(AudioSource audioSource, AudioClip engineIdling, AudioClip engineDriving, float pitchRange)
    {
        this.audioSource = audioSource;
        this.engineIdling = engineIdling;
        this.engineDriving = engineDriving;
        this.pitchRange = pitchRange;
    }

    public virtual void SetAudio(float speedValue, float turnValue)
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(speedValue) < 0.1f && Mathf.Abs(turnValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (audioSource.clip == engineDriving)
            {
                // ... change the clip to idling and play it.
                audioSource.clip = engineIdling;
                audioSource.pitch = pitchRange;
                audioSource.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (audioSource.clip == engineIdling)
            {
                // ... change the clip to driving and play.
                audioSource.clip = engineDriving;
                audioSource.pitch = pitchRange;
                audioSource.Play();
            }
        }
    }
}