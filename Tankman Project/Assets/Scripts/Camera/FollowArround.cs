using UnityEngine;
using System.Collections;


/// <summary>
/// to powinna być metoda a nie klasa no ale czyja klasa?
/// </summary>
public class FollowArround  :  MonoBehaviour 
{
    private void Update () 
    {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
	}
}
