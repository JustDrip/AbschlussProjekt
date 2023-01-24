using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckResult : MonoBehaviour
{
    //Variablen
    [SerializeField] private GameObject whiteBoard;
    [SerializeField] private GameObject tip;
    [SerializeField] private Material correct;
    public Texture2D letter;
    public float quote;


    public void check()
    {
        float treffer = 0f;
        //1024 Pixel in 16 Schritten abprüfen -> 64*64 Pixel werden geprüft
        float gesamtPixel = 64*64;
            //whiteBoard.GetComponent<Whiteboard>().textureSize.x * whiteBoard.GetComponent<Whiteboard>().textureSize.y;
        Debug.Log(gesamtPixel);
        Texture2D submission = whiteBoard.GetComponent<Whiteboard>().texture;
            for (int i = 0; i < whiteBoard.GetComponent<Whiteboard>().textureSize.y; i+=16)
            {
                for (int j = 0; j < whiteBoard.GetComponent<Whiteboard>().textureSize.y; j+=16)
                {
                
                    if (submission.GetPixel(i, j) == letter.GetPixel(i, j))
                    {
                        treffer++;
                    }
                }
            }
            float result = ((float)treffer / (float)gesamtPixel);
            Debug.Log(result);
            if (result >= quote)
            {
                tip.GetComponent<MeshRenderer>().material = correct;

            }

    }
}
