using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject m_glassObject;
    private bool m_zoom = false;
    private float m_initialZoom;
    private Vector3 m_initialPos;

    public Collider collisionMask;
    public float zoomFov = 3f;
    public float zoomSpeed = 5f;
    
	void Start ()
    {
        m_glassObject = GameObject.Find("Glass");
        m_initialZoom = GetComponent<Camera>().fieldOfView;
        m_initialPos = transform.position;
	}
	
	void Update ()
    {
        Camera cam = GetComponent<Camera>();

        if (m_zoom)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFov, Time.deltaTime * zoomSpeed);

            Vector3 targetPos = m_glassObject.transform.position;
            targetPos.y += m_glassObject.GetComponent<MeshRenderer>().bounds.extents.y * 1.5f;
            targetPos.z = transform.position.z;
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * zoomSpeed);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, m_initialZoom, Time.deltaTime * zoomSpeed);
            transform.position = Vector3.Slerp(transform.position, m_initialPos, Time.deltaTime * zoomSpeed);
        }

        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = (cam.fieldOfView - zoomFov) / 1f;
    }

    public Vector3 mousePoint()
    {
        Vector3 mpos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mpos.x, mpos.y, 0));

        RaycastHit info;
        collisionMask.Raycast(ray, out info, 100f);

        return info.point;
    }

    public void setZoom(bool zoom)
    {
        m_zoom = zoom;
    }

    public bool isZoom()
    {
        return m_zoom/* && GetComponent<Camera>().fieldOfView <= zoomFov + 10f*/;
    }
}
