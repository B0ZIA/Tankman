using UnityEngine;
using System.Collections.Generic;

public class Barrier
{
    private Sprite repairedTexture;
    private Sprite destroyedTexture;
    private int[] tankTiersWhichCanDestroy;



    public Barrier(Sprite repairedTexture, Sprite destroyedTexture, int[] tankTiersWhichCanDestroy)
    {
        this.repairedTexture = repairedTexture;
        this.destroyedTexture = destroyedTexture;
        this.tankTiersWhichCanDestroy = tankTiersWhichCanDestroy;
    }
}
