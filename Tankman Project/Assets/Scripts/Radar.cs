using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{

    public bool TargetPrzedemna;
    public BOTEngine botMovement;

    public float shootDistance = 7.75f;
    public GameObject maxPoint;
    public float degreesPerSec = 360f;
    public LayerMask whatToHit;


}
//    void Update()
//    {
//        return;
//        float tempRot = transform.localRotation.eulerAngles.z;


//        float rotAmount = degreesPerSec * Time.deltaTime;
//        float curRot = transform.localRotation.eulerAngles.z;
//        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));

//        Debug.Log(rotAmount + "  " + curRot);

//        if (transform.localRotation.z < 0.7)
//        {
//            coSkanuje = Skanuje.SkanujePrzod;
//            RaycastHit2D hit = MakeRaycast(Color.cyan, whatToHit, shootDistance);
//            if (hit.collider != null)
//            {
//                if (hit.collider.gameObject == botMovement.target)
//                {
//                    switch (coSkanuje)
//                    {
//                        case Skanuje.SkanujePrzod:
//                            TargetPrzedemna = true;
//                            break;
//                        case Skanuje.SkanujeTyl:
//                            TargetPrzedemna = false;
//                            break;
//                    }
//                }
//            }
//        }
//        else
//        {
//            coSkanuje = Skanuje.SkanujeTyl;
//            RaycastHit2D hit = MakeRaycast(Color.red, whatToHit, shootDistance);
//            if (hit.collider != null)
//            {
//                if (hit.collider.gameObject == botMovement.target)
//                {
//                    switch (coSkanuje)
//                    {
//                        case Skanuje.SkanujePrzod:
//                            TargetPrzedemna = true;
//                            break;
//                        case Skanuje.SkanujeTyl:
//                            TargetPrzedemna = false;
//                            break;
//                    }
//                }
//            }
//        }
//    }
//}

//    public MamTarget Scan(GameObject wantedObject, LayerMask whatToHit)
//    {
//        float curRot = transform.localRotation.eulerAngles.z;
//        float rotAmount = degreesPerSec * Time.deltaTime;
//        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));

//        if (transform.localRotation.z < 0.7)
//        {
//            coSkanuje = Skanuje.SkanujePrzod;
//            RaycastHit2D hit = MakeRaycast(Color.cyan, whatToHit, shootDistance);
//            if (hit.collider != null)
//            {
//                if (hit.collider.gameObject == botMovement.target)
//                {
//                    return MamTarget.ZPrzodu;
//                }
//            }
//            return MamTarget.NieZnalazłem;
//        }
//        else
//        {
//            coSkanuje = Skanuje.SkanujeTyl;
//            RaycastHit2D hit = MakeRaycast(Color.red, whatToHit, shootDistance);
//            if (hit.collider != null)
//            {
//                if (hit.collider.gameObject == botMovement.target)
//                {
//                    return MamTarget.ZTyłu;
//                }
//            }
//            return MamTarget.NieZnalazłem;
//        }
//    }

//    public RaycastHit2D MakeRaycast(Color color, LayerMask myWhatToHit,float myShootDistance)
//    {
//        Vector2 mousePosition = new Vector2(maxPoint.transform.position.x, maxPoint.transform.position.y);
//        Vector2 firePointPosition = new Vector2(transform.position.x, transform.position.y);
//        Debug.DrawLine(firePointPosition, mousePosition, color);
//        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, shootDistance, myWhatToHit);
//        return hit;
//    }

//    public enum Skanuje
//    {
//        SkanujePrzod,
//        SkanujeTyl,
//        Skanuje360
//    }
//}

//public enum MamTarget
//{
//    ZPrzodu,
//    ZTyłu,
//    NieZnalazłem
//}
