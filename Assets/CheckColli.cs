using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColli : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Rock2")
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }

        if (other.gameObject.name == "Rock4A")
        {
            gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

}
