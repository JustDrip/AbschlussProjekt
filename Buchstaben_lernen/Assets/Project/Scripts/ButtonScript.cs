using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    //Variablen
    [SerializeField] private float schwelle;
    [SerializeField] private float totZone;

    private bool btn_pressed;
    private Vector3 startPos;
    private ConfigurableJoint btn_joint;

    public UnityEvent onBtnPressed;
    public UnityEvent onBtnReleased;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        btn_joint = GetComponent<ConfigurableJoint>();
        btn_pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!btn_pressed && getValue() + schwelle >= 1)
        {
            Pressed();
        }
        if(btn_pressed && getValue() - schwelle <= 0)
        {
            Release();
        }
    }

    float getValue()
    {
        var distance = Vector3.Distance(startPos, transform.localPosition) / btn_joint.linearLimit.limit;

        if (Mathf.Abs(distance) < totZone)
        {
            distance = 0;
        }
        return Mathf.Clamp(distance, min: -1f, max:1f);
    }
    void Pressed()
    {
        btn_pressed = true;
        onBtnPressed.Invoke();
        Debug.Log("Button_Pressed");
    }
    void Release()
    {
        btn_pressed = false;
        onBtnReleased.Invoke();
        Debug.Log("Button_Released");
    }
}
