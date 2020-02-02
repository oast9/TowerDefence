using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

public class PlaneScan : MonoBehaviour
{
    [SerializeField]
    public GameObject placePrefab;
    private Vector2 touchPosition = default;
    private ARRaycastManager aRRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private void Awake() => aRRaycastManager = GetComponent<ARRaycastManager>();

    void Update()
    {
        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;
        if (touch.phase == TouchPhase.Began)
        {
            if (aRRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.All))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(placePrefab, hitPose.position, Quaternion.identity);
                gameObject.GetComponent<ARPointCloudManager>().SetTrackablesActive(false);
                gameObject.GetComponent<ARPlaneManager>().SetTrackablesActive(false);
                gameObject.GetComponent<AddCamToCanvas>().enabled = true;
                enabled = false;
            }
        }
    }
}
