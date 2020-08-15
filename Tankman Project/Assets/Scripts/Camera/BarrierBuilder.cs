using UnityEngine;

public class BarrierBuilder : Photon.MonoBehaviour
{
    [SerializeField]
    private GameObject canBuildPanel;
    [SerializeField]
    private GameObject canNotBuildPanel;
    [SerializeField]
    private GameObject musicPrefabInstancedWhileBuild;
    [SerializeField]
    private GameObject barrierPrefab;

    private bool canBuild = false;
    private bool allowShooting = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RotateBarrierPlan();

        if (Input.GetKeyDown(KeyCode.Escape))
            Destroy(gameObject);

        if (!allowShooting)
            GameManager.LocalPlayer.gameObject.GetComponent<TankShot>().SetShootingOpportunity(false);
    }

    private void RotateBarrierPlan()
    {
        transform.Rotate(new Vector3(0, 0, 90));
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == TagsManager.GetTag(Tag.StaticGameObject)
            || collision.tag == TagsManager.GetTag(Tag.RepairedBarrier)
            || collision.tag == TagsManager.GetTag(Tag.DestroyedBarrier))
        {
            canNotBuildPanel.SetActive(true);
            canBuildPanel.SetActive(false);
            canBuild = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagsManager.GetTag(Tag.StaticGameObject)
            || collision.tag == TagsManager.GetTag(Tag.RepairedBarrier)
            || collision.tag == TagsManager.GetTag(Tag.DestroyedBarrier))
        {
            canBuildPanel.SetActive(true);
            canNotBuildPanel.SetActive(false);
            canBuild = true;
        }
    }

    public void BuildBarrier()
    {
        allowShooting = true;
        if (canBuild)
        {
            GameManager.Instance.photonView.RPC("SpawnSceneObjectRPC", PhotonTargets.MasterClient, barrierPrefab.name, transform.position, transform.rotation);
            GameManager.LocalPlayer.gameObject.GetComponent<PlayerGO>().myPlayer.Zasoby -= 1;
            GameManager.LocalPlayer.gameObject.GetComponent<TankShot>().SetShootingOpportunity(true,0.2f);
            Instantiate(musicPrefabInstancedWhileBuild, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
