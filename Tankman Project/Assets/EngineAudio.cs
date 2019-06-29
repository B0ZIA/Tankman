using UnityEngine;

public class EngineAudio
{
    private AudioSource AudioSource;// Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    private AudioClip EngineIdling;// Audio to play when the tank isn't moving.
    private AudioClip EngineDriving;// Audio to play when the tank is moving.
    private float PitchRange;// The amount by which the pitch of the engine noises can vary.

    public EngineAudio(AudioSource audioSource, AudioClip engineIdling, AudioClip engineDriving, float pitchRange)
    {
        AudioSource = audioSource;
        EngineIdling = engineIdling;
        EngineDriving = engineDriving;
        PitchRange = pitchRange;
    }

    public virtual void SetAudio(float speedValue, float turnValue)
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(speedValue) < 0.1f && Mathf.Abs(turnValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (AudioSource.clip == EngineDriving)
            {
                // ... change the clip to idling and play it.
                AudioSource.clip = EngineIdling;
                AudioSource.pitch = PitchRange;
                AudioSource.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (AudioSource.clip == EngineIdling)
            {
                // ... change the clip to driving and play.
                AudioSource.clip = EngineDriving;
                AudioSource.pitch = PitchRange;
                AudioSource.Play();
            }
        }
    }
}