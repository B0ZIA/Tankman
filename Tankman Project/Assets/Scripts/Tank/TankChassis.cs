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
        forwardTrialleft.Play();
        forwardTrialRight.Play();
        buttonTrialleft.Play();
        buttonTrialRight.Play();
    }

    private void OnDisable()
    {
        forwardTrialleft.Stop();
        forwardTrialRight.Stop();
        buttonTrialleft.Stop();
        buttonTrialRight.Stop();
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
