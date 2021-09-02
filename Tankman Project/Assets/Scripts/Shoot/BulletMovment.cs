using UnityEngine;

public class BulletMovment : MonoBehaviour {

	public int moveSpeed = 30;
    public float time = 0.6f;
    public Transform Explosion;
    public Transform boom;
    public GameObject own;
    public GameObject onShotEffect;


    public void Start()
    {
        Destroy(gameObject, time);
        Destroy(onShotEffect, time/1.5f);
        onShotEffect.transform.parent = FindObjectOfType<TankShot>().startFirePoint;
    }

    void Update ()
    {
		Move();
	}


    void Move()
    {
        transform.Translate(-Vector3.left * Time.deltaTime * moveSpeed);
    }


    void OnTriggerEnter2D(Collider2D coll) 
	{
        if (own == null)
        {
            Debug.LogWarning("Nieznaleziono właściciela pocisku!");
            Destroy(gameObject);
            return;
        }
        if (own.gameObject.tag == TagsManager.GetTag(Tag.RemotePlayerBody))
        {
            if(coll.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
            {
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<TankDeath>().tankCamera.GetComponent<Shake>().CamShake();
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody) && coll.gameObject != own)
            {
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            if (coll.tag == TagsManager.GetTag(Tag.Bot))
            {
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        else if (own.gameObject.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
        {
            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody))
            {
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            if (coll.tag == TagsManager.GetTag(Tag.Bot))
            {
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        else if (own.gameObject.tag == TagsManager.GetTag(Tag.Bot))
        {
            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody) || coll.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
            {
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<TankDeath>().tankCamera.GetComponent<Shake>().CamShake();
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        if (coll.tag == TagsManager.GetTag(Tag.StaticGameObject))
        {
            Instantiate(Explosion, boom.position, transform.rotation);
            Destroy(gameObject);
        }
	}

}
