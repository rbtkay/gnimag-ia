using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{

    public int state;
    // Use this for initialization
    void Start()
    {
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (state != 0)
        {
            return;
        }
        if (GameManagerScript.instance.turn == 1)
        {
            GameManagerScript.instance.turn = 2;
            GetComponent<MeshRenderer>().material.color = Color.red;
            state = 1;
        }
    }
}
