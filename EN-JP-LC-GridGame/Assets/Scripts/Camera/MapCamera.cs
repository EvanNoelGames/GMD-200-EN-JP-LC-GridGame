using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private MapManager map;

    private Camera cam;

    private Vector3 mapMidPoint;

    private float maxZoomOut;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        
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
}
