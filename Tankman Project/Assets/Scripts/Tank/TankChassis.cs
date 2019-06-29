using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankChassis : MonoBehaviour
{
    [SerializeField]
    private Color normalTrial;
    [SerializeField]
    private Color transparentTrial;

    [SerializeField]
    private ParticleSystem forwardTrialleft;
    [SerializeField]
    private ParticleSystem forwardTrialRight;
    [SerializeField]
    private ParticleSystem buttonTrialleft;
    [SerializeField]
    private ParticleSystem buttonTrialRight;



    private void OnEnable()
    {
        // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
        // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
        // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
        for (int i = 0; i < 4; ++i)
        {
            forwardTrialleft.Play();
            forwardTrialRight.Play();
            buttonTrialleft.Play();
            buttonTrialRight.Play();
        }
    }

    private void OnDisable()
    {
        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for (int i = 0; i < 4; ++i)
        {
            forwardTrialleft.Stop();
            forwardTrialRight.Stop();
            buttonTrialleft.Stop();
            buttonTrialRight.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            //cofanie
            ParticleSystem.MainModule psmain;
            psmain = buttonTrialRight.main;
            psmain.startColor = transparentTrial;
            psmain = buttonTrialleft.main;
            psmain.startColor = transparentTrial;

            psmain = forwardTrialRight.main;
            psmain.startColor = normalTrial;
            psmain = forwardTrialleft.main;
            psmain.startColor = normalTrial;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //jazda!
            ParticleSystem.MainModule psmain;
            psmain = buttonTrialRight.main;
            psmain.startColor = normalTrial;
            psmain = buttonTrialleft.main;
            psmain.startColor = normalTrial;

            psmain = forwardTrialRight.main;
            psmain.startColor = transparentTrial;
            psmain = forwardTrialleft.main;
            psmain.startColor = transparentTrial;
        }
    }
}
