using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;
    public float minZoomDist;
    public float maxZoomDist;
    private Camera camera;
    void Awake()
    {
        camera = Camera.main;
    }
    void Update()
    {
        Move();
        Zoom();
    }
    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * (-zInput) - transform.right * xInput;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, camera.transform.position);
        camera.transform.position += camera.transform.forward * scrollInput * zoomSpeed;
    }

}
