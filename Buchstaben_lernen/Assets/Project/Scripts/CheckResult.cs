using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckResult : MonoBehaviour
{
    //Variablen
    [SerializeField] private GameObject whiteBoard;
    [SerializeField] private GameObject letter_object;
    [SerializeField] private Material correctResult;
    [SerializeField] private Text btn_text;
    public Texture2D letter;
    public float quote;
    private int level = 2;


    public void check()
    {
        float treffer = 0f;
        //1024 Pixel in 16 Schritten abprüfen -> 64*64 Pixel werden geprüft
        float gesamtPixel = 64*64;
            //whiteBoard.GetComponent<Whiteboard>().textureSize.x * whiteBoard.GetComponent<Whiteboard>().textureSize.y; -> Falls alle Pixel geprüft werden sollen
        Debug.Log(gesamtPixel);
        Texture2D submission = whiteBoard.GetComponent<Whiteboard>().texture;
            for (int i = 0; i < whiteBoard.GetComponent<Whiteboard>().textureSize.y; i+=16)
            {
                for (int j = 0; j < whiteBoard.GetComponent<Whiteboard>().textureSize.y; j+=16)
                {
                    Color subColor = submission.GetPixel(i, j);
                    Color letColor = letter.GetPixel(i, j);
                    if ((subColor.r == letColor.r) && (subColor.b == letColor.b) && (subColor.g == letColor.g))
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
                whiteBoard.GetComponent<MeshRenderer>().material = correctResult;
                btn_text.text = "Richtig!";
                StartCoroutine(loadNextScene());

            }
    }

    public IEnumerator loadNextScene()
    {
        yield return new WaitForSeconds(10);
        if (SceneManager.GetActiveScene().buildIndex < level - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
