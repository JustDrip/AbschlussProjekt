using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pencil : MonoBehaviour
{
    [SerializeField] private Transform pHandle;
    [SerializeField] private Transform pTip;
    [SerializeField] private int pSize;
    private Whiteboard whiteB;
    private Renderer rend;
    private Color[] color;
    private float tHeight;
    private RaycastHit touch;
    private Vector2 touchPos;
    private Vector2 lastTouchPos;
    private bool touchedLastFrame;
    private Quaternion lastTouchRot;
    // Start is called before the first frame update
    void Start()
    {
        rend = pTip.GetComponent<Renderer>();
        color = Enumerable.Repeat(rend.material.color, pSize * pSize).ToArray();
        tHeight = pTip.localScale.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        WriteOnWhiteBoard();
    }

    void WriteOnWhiteBoard()
    {
        // whiteboard is touched in current frame
        if(Physics.Raycast(origin:pTip.position, direction:pTip.transform.up, out touch, (tHeight/10f)))
        {
            if (touch.transform.CompareTag("Whiteboard"))
            {
                if(whiteB== null)
                {
                    whiteB = touch.transform.GetComponent<Whiteboard>();
                }
                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);
                var textX = (int)(touchPos.x * whiteB.textureSize.x - (pSize / 2));
                var textY = (int)(touchPos.y * whiteB.textureSize.y - (pSize / 2));

                //check if touchPos is on Whiteboard

                if(textX<0 || textX>whiteB.textureSize.x || textY<0 || textY > whiteB.textureSize.y)
                {
                    return;
                }

                // check if whiteboard was touched in last frame
                if (touchedLastFrame)
                {
                    whiteB.texture.SetPixels(textX, textY, pSize, pSize, color);
                    // Interpolation between lastTouchPos and touchPos
                    for (float f = 0.01f; f< 1.00f; f += 0.01f)
                    {
                        var lx = (int)Mathf.Lerp(lastTouchPos.x, textX, f);
                        var ly = (int)Mathf.Lerp(lastTouchPos.y, textY, f);
                        whiteB.texture.SetPixels(lx, ly, pSize, pSize, color);
                    }
                    transform.rotation = lastTouchRot;
                    whiteB.texture.Apply();
                }
                // updating last frame touch -position and -rotation
                lastTouchPos = new Vector2(textX, textY);
                lastTouchRot = transform.rotation;
                touchedLastFrame = true;
                return;
            }
        }
        // no whiteboard was touched in current frame
        whiteB = null;
        touchedLastFrame = false;
    }
    void UpdatePos()
    {
        pTip.position = new Vector3(pHandle.position.x, pHandle.position.y, pHandle.position.z);
        pTip.rotation = pHandle.rotation;
    }
}
