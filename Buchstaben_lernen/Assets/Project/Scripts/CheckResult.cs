using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckResult : MonoBehaviour
{
    //Variablen
    [SerializeField] private GameObject whiteBoard;
    [SerializeField] private GameObject letter_object;
    [SerializeField] private Material correct;
    public Texture2D letter;
    public float quote;
    private int level = 2;


    public void check()
    {
        float treffer = 0f;
        //1024 Pixel in 16 Schritten abpr�fen -> 64*64 Pixel werden gepr�ft
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
                letter_object.SetActive(true);
                
                StartCoroutine(loadNextScene());

            }
    }

    public IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(5);
        if (SceneManager.GetActiveScene().buildIndex < level - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
