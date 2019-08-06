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
        StartLeaveTrial();
    }

    private void OnDisable()
    {
        StopLeaveTrial();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.S))
            SetLeavingFrontTrial();
        else if (Input.GetKey(KeyCode.W))
            SetLeavingBackTrial();
    }

    private void StartLeaveTrial()
    {
        forwardTrialleft.Play();
        forwardTrialRight.Play();
        buttonTrialleft.Play();
        buttonTrialRight.Play();
    }

    private void StopLeaveTrial()
    {
        forwardTrialleft.Stop();
        forwardTrialRight.Stop();
        buttonTrialleft.Stop();
        buttonTrialRight.Stop();
    }

    private void SetLeavingFrontTrial()
    {
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

    private void SetLeavingBackTrial()
    {
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
