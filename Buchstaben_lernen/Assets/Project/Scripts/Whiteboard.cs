using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize= new Vector2(1200, 1200);



    // Start is called before the first frame update
    void Start()
    {
        var rend = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        rend.material.mainTexture = texture;
    }
   
}
