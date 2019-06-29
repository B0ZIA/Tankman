using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The script responsible for regeneration HP
/// </summary>
public class TankHealth : Photon.MonoBehaviour
{
    public static TankHealth Instance { get; private set; }

    [SerializeField]
    [Range(1f, 180f)] private float czasDoRozpoczeciaRegeneracji = 30f;
    public float CzasDoRozpoczeciaRegeneracji { get { return czasDoRozpoczeciaRegeneracji; } }

    [SerializeField]
    [Range(1f,10f)] private float szybkoscRegeneracji = 3f;
    public float tempTime;

    public float MaxHP
    {
        get { return TankEvolution.Instance.MaxHp; }
    }

    Player player;

    [SerializeField]
    private Slider sliderHpOnTank;

    float tempHp;

    bool czyPotrzebaRegeneracji = true;
    bool czekajNaRegeneracje = true;
    bool przerwijRegeneracje = false;
    bool a = false;



    public void Awake()
    {
        if (!photonView.isMine)
            enabled = false;
        else if (Instance == null)
            Instance = this;
        else
            enabled = false;
    }

    void Start ()
    {
        player = GetComponent<PlayerGO>().myPlayer;
        tempHp = player.hp;
	}

	void Update ()
    {
        //Jeśli gracz PONOWNIE oberwie to przerywam regeneracje
        if (a)
        {
            if (tempHp != player.hp)
            {
                //Debug.Log("Gracz oberwał ponownie, przerywam regeneracje...");
                photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
                czekajNaRegeneracje = false;
                przerwijRegeneracje = true;
                tempHp = player.hp;
            }
        }

        //Pętla zamyka się tutaj jeśli masz mniej HP niż maxHP
        if (czyPotrzebaRegeneracji == false)
            return;


        //Jesli nie mam maksymalnej ilosci HP
        if (player.hp != MaxHP)
        {
            //Debug.Log("OHO! trzeba czekać na regenerecje");
            photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
            czyPotrzebaRegeneracji = false;     //Zamykam pętle powyżej
            przerwijRegeneracje = false;
            czekajNaRegeneracje = true;
            tempHp = player.hp;
            a = true;
            StartCoroutine(CzekanieNaRegeneracje());
        }
	}

    IEnumerator CzekanieNaRegeneracje()
    {
        for(int i = 0; i < czasDoRozpoczeciaRegeneracji; i++)
        {
            tempTime = czasDoRozpoczeciaRegeneracji-i;
            yield return new WaitForSecondsRealtime(1f);
            if (czekajNaRegeneracje == false)
            {
                //Przerywam pętle
                i = (int)czasDoRozpoczeciaRegeneracji;
                //Debug.Log("...podczas oczekiwania na regeneracje !");
                photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
            }
        }
        if (czekajNaRegeneracje)
        {
            StartCoroutine(Regeneracja());
        }
        else
        {
            czekajNaRegeneracje = true;
            czyPotrzebaRegeneracji = true;
            przerwijRegeneracje = false;
            a = false;
        }
    }

    IEnumerator Regeneracja()
    {
        //Debug.Log("Rozpoczynam Regeneracje !");
        float ihp = player.hp;
        float imaxHp = MaxHP;

        while(ihp < imaxHp)
        {
            yield return new WaitForSecondsRealtime((11f - szybkoscRegeneracji) / 10);
            a = false;
            player.hp += 10f;
            ihp = player.hp;
            imaxHp = MaxHP;
            tempHp = ihp;
            photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
            if (przerwijRegeneracje)
            {
                //Przerywam pętle
                //Debug.Log("...podczas regeneracji !");
                photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
                ihp = imaxHp;
            }
            a = true;
        }
        //Debug.Log("Kończe regeneracje ponieważ mam już pełne HP!");
        if (player.hp > MaxHP)
          player.hp = MaxHP;
        photonView.RPC("SetHpRPC", PhotonTargets.Others, GetComponent<PlayerGO>().myPlayer.hp, MaxHP);
        tempHp = MaxHP;
        czyPotrzebaRegeneracji = true;
        przerwijRegeneracje = false;
        czekajNaRegeneracje = true;
        a = false;
    }

    [PunRPC]
    void SetHpRPC(float HP, float MAXHP)
    {
        GetComponent<PlayerGO>().myPlayer.hp = HP;
        sliderHpOnTank.value = HP / MAXHP;
    }
}
