using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject m_glassObject;
    private Transform m_playerObject;
    private float m_initialDistance;
    private bool m_zoom = false;
    private float m_initialZoom;
    private Vector3 m_initialPos;
    private Vector3 m_shakeDirection;
    private Vector3 m_targetShakeDirection;
    private float m_shakeCounter = 0.5f;

    public Collider collisionMask;
    public float zoomFov = 3f;
    public float zoomSpeed = 5f;
    
	void Start ()
    {
        m_glassObject = GameObject.Find("Glass");
        m_playerObject = GameObject.Find("Player").transform;
        m_initialDistance = m_playerObject.position.x;
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
        audio.volume = 1f - (cam.fieldOfView - zoomFov) / (m_initialZoom - zoomFov);


        if ((m_shakeCounter -= Time.deltaTime) <= 0f)
        {
            m_shakeCounter = 0.5f;
            float advance = 1f - (m_playerObject.position.x - m_glassObject.transform.position.x) / (m_initialDistance - m_glassObject.transform.position.x);
            m_targetShakeDirection = Random.insideUnitSphere * advance;
            m_targetShakeDirection *= 0.25f;
            m_targetShakeDirection.z = 0f;
        }
        if (!m_zoom)
        {
            m_shakeDirection = Vector3.Slerp(m_shakeDirection, m_targetShakeDirection, Time.deltaTime * 20f);
            transform.localPosition = Vector3.Slerp(transform.localPosition, m_shakeDirection, Time.deltaTime * 2.5f);
        }
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
