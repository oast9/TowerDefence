using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class PlaneScan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //ARKitSessionSubsystem
    }

    // Update is called once per frame
    void Update()
    {
        SetMap();
    }

    private void SetMap()
    {
        // GameObject session = GameObject.FindGameObjectWithTag("session");
        // ARPlaneManager manager = session.GetComponent<ARPlaneManager>();
        // GameObject map = manager.planePrefab;
        // Debug.Log(map.transform.localScale.x + " " + map.transform.localScale.y + " " + map.transform.localScale.z);
    }
}
