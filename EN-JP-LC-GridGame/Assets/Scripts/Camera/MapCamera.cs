using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private MapManager map;

    private Camera cam;

    private Vector3 mapMidPoint;

    private float maxZoomOut;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 difference;
    private Vector3 origin;

    private bool drag;


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (map.DoneLoading)
        {
            HandleMapMovement();
        }
    }

    public void SetupMap()
    {
        mapMidPoint = map.GetAverageTilePosition();
        mapMidPoint.z = -10;

        maxZoomOut = GetFullZoomOut();
        ResetView();
    }

    public void ResetView()
    {
        transform.position = mapMidPoint;
        cam.orthographicSize = maxZoomOut;
    }

    private float GetFullZoomOut()
    {
        Vector3 topRight = new Vector3(Mathf.Abs(map.MaxTileX), Mathf.Abs(map.MaxTileY), 0);
        Vector3 botLeft = new Vector3(Mathf.Abs(map.MinTileX), Mathf.Abs(map.MinTileY), 0);

        if (topRight.magnitude > botLeft.magnitude)
        {
            return topRight.magnitude;
        }
        else
        {
            return botLeft.magnitude;
        }
    }

    private void HandleMapMovement()
    {
        if (Input.GetMouseButton(0))
        {
            difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (drag == false)
            {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag == true)
        {
            cam.transform.position = origin - difference;
        }

        // reset camera to origin and zoom out
        if (Input.GetMouseButton(1))
        {
            cam.transform.position = mapMidPoint;
            cam.orthographicSize = maxZoomOut;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            cam.orthographicSize -= 0.5f;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            cam.orthographicSize += 0.5f;
        }

        // don't zoom too far in
        if (cam.orthographicSize < 1)
        {
            cam.orthographicSize = 1;
        }
    }
}
