using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Pencil : MonoBehaviour
{
    private Interactable inter;
    public SteamVR_Action_Boolean switchPMode;
    public SteamVR_Action_Boolean submitResult;
    private int mode;
    [SerializeField] private Material correct;
    [SerializeField] private Material tipColor2;
    [SerializeField] private Material tipColor1;
    [SerializeField] private GameObject tip;
    [SerializeField] private Transform pHandle;
    [SerializeField] private Transform pTip;
    [SerializeField] private int pSize;
    [SerializeField] private GameObject whiteBoard;
    private Whiteboard whiteB;
    private Renderer rend;
    private Color[] color;
    private float tHeight;
    private RaycastHit touch;
    private Vector2 touchPos;
    private Vector2 lastTouchPos;
    private bool touchedLastFrame;
    private Quaternion lastTouchRot;
    public Texture2D letter;
    public float quote;
    // Start is called before the first frame update
    void Start()
    {
        rend = pTip.GetComponent<Renderer>();
        color = Enumerable.Repeat(rend.material.color, pSize * pSize).ToArray();
        tHeight = pTip.localScale.y;
        inter = GetComponent<Interactable>();
        mode = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        WriteOnWhiteBoard();
        if (inter.attachedToHand != null)
        {
            //pencil is grabbed
            switchMode();
            check();
        }
        
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
    void switchMode()
    {
        bool modeSwitch = false;
        SteamVR_Input_Sources source = inter.attachedToHand.handType;
        if (switchPMode[source].stateDown)
        {
            if(mode==0)
            {
                tip.GetComponent<MeshRenderer>().material = tipColor2;
                mode = 1;
                modeSwitch = true;
            }
            else
            {
                tip.GetComponent<MeshRenderer>().material = tipColor1;
                mode = 0;
                modeSwitch = true;
            }
        }

        if (modeSwitch)
        {
            color = Enumerable.Repeat(rend.material.color, pSize * pSize).ToArray();
            modeSwitch = false;
        }

    }

    void check()
    {

        float treffer = 0;
        Debug.Log(treffer);
        float gesamtPixel = whiteBoard.GetComponent<Whiteboard>().textureSize.x * whiteBoard.GetComponent<Whiteboard>().textureSize.y;
        Debug.Log(gesamtPixel);
        SteamVR_Input_Sources source = inter.attachedToHand.handType;
        if (submitResult[source].stateDown)
        {
            Texture2D submission = whiteBoard.GetComponent<Whiteboard>().texture;
            for(int i=0; i<whiteBoard.GetComponent<Whiteboard>().textureSize.y; i++)
            {
                for(int j=0; j < whiteBoard.GetComponent<Whiteboard>().textureSize.y; j++)
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

}
