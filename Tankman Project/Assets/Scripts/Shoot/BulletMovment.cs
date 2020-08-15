using UnityEngine;

/// <summary>
/// Sktypt odpowiedzialny za poruszanie się i inne funkcje każdego pocisku
/// </summary>
public class BulletMovment : MonoBehaviour {

	public int moveSpeed = 30;
    public float time = 0.4f;
    public Transform Explosion;
    public Transform boom;
    public GameObject own;


    public void Start()
    {
        Destroy(gameObject, time);
    }

    void Update ()
    {
		Move();
	}


    void Move()
    {
        transform.Translate(-Vector3.left * Time.deltaTime * moveSpeed);
    }


    //Jestem pociskiem
    //O to mój krutki żywot:
    void OnTriggerEnter2D(Collider2D coll) 
	{
        if (own == null)
        {
            Debug.LogWarning("Nieznaleziono właściciela pocisku!");
            Destroy(gameObject);
            return;
        }
        //Jeśli wystrzelił mnie nie lokalny gracz..
        if (own.gameObject.tag == TagsManager.GetTag(Tag.RemotePlayerBody))
        {
            //i trafiłem lokalnego gracza...
            if(coll.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
            {
                //to lokalny gracz potrząsa swoją kamerą
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<TankDeath>().tankCamera.GetComponent<Shake>().CamShake();
                //i robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //aczkolwiek mogłem trafić w ogóle innego gracza (nie siebie)
            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody) && coll.gameObject != own)
            {
                //więc zrobię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //lub trafiłem Bota...
            if (coll.tag == TagsManager.GetTag(Tag.Bot))
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Jeśli wystrzelił mnie lokalny gracz...
        else if (own.gameObject.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
        {
            //own.GetComponent<GameOver>().tankCamera.GetComponent<Shake>().CamShake();

            //i trafiłem nie lokalnego gracza...
            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody))
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //lub trafiłem Bota...
            if (coll.tag == TagsManager.GetTag(Tag.Bot))
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Jeśli wystrzelił mnie BOT...
        else if (own.gameObject.tag == TagsManager.GetTag(Tag.Bot))
        {
            //i trafiłem jakiegoś gracza...
            if (coll.tag == TagsManager.GetTag(Tag.RemotePlayerBody) || coll.tag == TagsManager.GetTag(Tag.LocalPlayerBody))
            {
                //jeśli tak lokalny gracz potrząsa swoją kamerą
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<TankDeath>().tankCamera.GetComponent<Shake>().CamShake();
                //i zrobi boom!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Nie ważnie kto mnie wystrzelił
        //ale trafiłem chyba w kamień, a może to dom?...
        if (coll.tag == TagsManager.GetTag(Tag.StaticGameObject))
        {
            //więc robię BOOM!
            Instantiate(Explosion, boom.position, transform.rotation);
            Destroy(gameObject);
        }
	}

}
