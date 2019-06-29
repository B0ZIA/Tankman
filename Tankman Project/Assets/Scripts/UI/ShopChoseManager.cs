using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopChoseManager : MonoBehaviour {

    public  bool nation;
    public bool moro;
    string tempTag;
    int tags;

    public Sprite nation_Russia;
    public Sprite nation_Germany;
    public Sprite nation_America;

    public Sprite moro_forest;
    public Sprite moro_desert;

    void Start()
    {
        tempTag = gameObject.tag;
        int.TryParse(tempTag, out tags);

        if(nation)
        {
            switch (tags)
            {
                case 0:
                    GetComponent<SpriteRenderer>().sprite = nation_Russia;
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().sprite = nation_Germany;
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().sprite = nation_America;
                    break;
            }
        }
    }
}
