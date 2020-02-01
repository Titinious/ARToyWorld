using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField]
    public GameObject source;
    [SerializeField]
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.value(gameObject, (_value) =>
        {
            //Vector3 scale = 
            if(source != null && target != null)
                transform.position = Vector3.Lerp(source.transform.position, target.transform.position, _value);

        }, 0.0f, 1.0f, 2.0f).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
