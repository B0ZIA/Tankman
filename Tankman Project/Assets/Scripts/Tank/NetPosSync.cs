using UnityEngine;

/*
 * ###################################
 * #        by Jakub Główczyk        #
 * #            [#][#][#]            #
 * ###################################
 */

/// <summary>
/// Script Responsible for Synchronization Position Players
/// </summary>
public class NetPosSync : Photon.MonoBehaviour
{
    [SerializeField] [Range(10,30)]
    private int networkSendRate = 15;

    [SerializeField]
    private Transform[] targets;
    private Vector3[] myVector;
    private Quaternion[] myQuaternion;



    void Awake()
    {
        if (photonView == null)
            Debug.LogError("no PhotonView! Please add PhotonView component to this object!");

        myVector = new Vector3[targets.Length];
        myQuaternion = new Quaternion[targets.Length];
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].position = Vector3.Lerp(targets[i].position, myVector[i], networkSendRate * Time.deltaTime);
                targets[i].rotation = Quaternion.Slerp(targets[i].rotation, myQuaternion[i], networkSendRate * Time.deltaTime);
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView.isMine)
        {
            // Gracz należy do mnie, wysyłam innym moją pozycję
            for (int i = 0; i < targets.Length; i++)
            {
                stream.SendNext(targets[i].position);
                stream.SendNext(targets[i].rotation);
            }
        }
        else
        {
            // Gracz nie należy do mnie, pobieram pozycje i ustawiam ją
            for (int i = 0; i < targets.Length; i++)
            {
                myVector[i] = (Vector3)stream.ReceiveNext();
                myQuaternion[i] = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
