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
        if (own.gameObject.tag == Tag.REMOTEPLAYERBODY)
        {
            //i trafiłem lokalnego gracza...
            if(coll.tag == Tag.LOCALPLAYERBODY)
            {
                //to lokalny gracz potrząsa swoją kamerą
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<GameOver>().tankCamera.GetComponent<Shake>().CamShake();
                //i robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //aczkolwiek mogłem trafić w ogóle innego gracza (nie siebie)
            if (coll.tag == Tag.REMOTEPLAYERBODY && coll.gameObject != own)
            {
                //więc zrobię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //lub trafiłem Bota...
            if (coll.tag == Tag.BOT)
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Jeśli wystrzelił mnie lokalny gracz...
        else if (own.gameObject.tag == Tag.LOCALPLAYERBODY)
        {
            //own.GetComponent<GameOver>().tankCamera.GetComponent<Shake>().CamShake();

            //i trafiłem nie lokalnego gracza...
            if (coll.tag == Tag.REMOTEPLAYERBODY)
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }

            //lub trafiłem Bota...
            if (coll.tag == Tag.BOT)
            {
                //więc robię BOOM!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Jeśli wystrzelił mnie BOT...
        else if (own.gameObject.tag == Tag.BOT)
        {
            //i trafiłem jakiegoś gracza...
            if (coll.tag == Tag.REMOTEPLAYERBODY || coll.tag == Tag.LOCALPLAYERBODY)
            {
                //jeśli tak lokalny gracz potrząsa swoją kamerą
                coll.GetComponent<TankObject>().PlayerGO.GetComponent<GameOver>().tankCamera.GetComponent<Shake>().CamShake();
                //i zrobi boom!
                Instantiate(Explosion, boom.position, transform.rotation);
                Destroy(gameObject);
            }
        }


        //Nie ważnie kto mnie wystrzelił
        //ale trafiłem chyba w kamień, a może to dom?...
        if (coll.tag == Tag.STATICGAMEOBJECT)
        {
            //więc robię BOOM!
            Instantiate(Explosion, boom.position, transform.rotation);
            Destroy(gameObject);
        }
	}

}
