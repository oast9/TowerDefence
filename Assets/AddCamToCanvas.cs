using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCamToCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera Cam;
    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("canv");
        for (int i=0; i<obj.Length; i++)
        {
            obj[i].gameObject.GetComponent<Canvas>().worldCamera = Cam;
        }
    }


}
