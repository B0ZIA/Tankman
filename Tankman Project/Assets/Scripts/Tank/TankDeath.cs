using UnityEngine;

/// <summary>
/// Scritp responsoible for player progres in game. 
/// Don't turn off this script in PlayerSetup
/// </summary>
public class TankDeath : Photon.MonoBehaviour
{
    [Header("Inne skrypty:")]
    public CameraClamp cameraFollow;
    public PlayerSetup playerSetup;
    public TriggerSth triggerSth;
 
    public GameObject stan;
    public Camera tankCamera;
    public GameObject gameOver;

    //Do śmierci gracza
    [Header("Do śmierci gracza:")]
    public GameObject camOryginalTarget;
    public GameObject camDeadTarget;
    public GameObject[] objectToDisableOnDeath;
    public Behaviour[] componentToDisableOnDeath;

    public delegate void ShakeEnemyCamera();
    public event ShakeEnemyCamera onPlayerDead;
    public TankPeriscope tankPeriscope;



    /// <summary>
    /// Śmierć gracza (tylko lokalna i nie pełna)
    /// </summary>
    public void OnDead()
    {
        for (int i = 0; i < objectToDisableOnDeath.Length; i++)
        {
            objectToDisableOnDeath[i].SetActive(false);
        }
        for (int i = 0; i < componentToDisableOnDeath.Length; i++)
        {
            componentToDisableOnDeath[i].enabled = false;
        }
        GetComponent<TankEvolution>().TurretGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        ShopManager.Instance.ResetUpdate();
        cameraFollow.target = camDeadTarget.transform;
        GetComponent<TankRPC>().OnDeathRPC(true);
        gameOver.SetActive(true);

        if (onPlayerDead != null)
            onPlayerDead();
    }

    /// <summary>
    /// Kontynuacja rozgrywki lokalnego gracza
    /// </summary>
    public void ContinueGame()
    {
        UstawPozycje();
        for (int i = 0; i < objectToDisableOnDeath.Length; i++)
        {
            objectToDisableOnDeath[i].SetActive(true);
        }
        for (int i = 0; i < componentToDisableOnDeath.Length; i++)
        {
            componentToDisableOnDeath[i].enabled = true;
        }
        GetComponent<TankEvolution>().TurretGameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        cameraFollow.target = camOryginalTarget.transform;
        GetComponent<TankRPC>().OnDeathRPC(false);
        gameOver.SetActive(false);
        GetComponent<TankShot>().isReloadnig = false;
        GetComponent<TankShot>().realReloadTime = 0f;
        GetComponent<TankShot>().SetShootingOpportunity(true);
        GetComponent<PlayerGO>().myPlayer.currentHp = 600f;
        GetComponent<TankEvolution>().SetStartTank();
        HUDManager.Instance.StartRefresh();
        TankWaterCollision.Instance.Setup();
        TechTree.Instance.TankSwitchTierButton(1);
        PlayersManager.Instance.UpdateMyPlayer(photonView.viewID);
    }

    /// <summary>
    /// Looks for player respawns and moves the player in a random of them
    /// </summary>
    void UstawPozycje()
    {
        Vector3 pos = GameManager.Instance.GetRandomGameObject(TagsManager.GetTag(Tag.PlayerSpawn)).transform.position;
        GetComponent<TankEvolution>().TankGameObject.transform.position = pos;
    }

    public void ShakeCamera()
    {
        photonView.RPC("ShakeMyCameraRPC", GetComponent<PlayerGO>().myPlayer.pp, null);
    }

    [PunRPC]
    public void ShakeMyCameraRPC()
    {
        tankCamera.GetComponent<Shake>().CamShake();
    }
}