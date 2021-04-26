using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CameraController : MonoBehaviour
    {
        //camera moviment
        public Transform target = null;
        private Vector3 offset;

        //camera zoom
        private float currentZoom = 10f;
        private float pitch = 2f;
        private float zoomSpeed = 4f;
        private float minZoom = 5f;
        private float maxZoom = 15f;


        //camera rotation
        public float yawSpeed = 100f;
        private float currentYaw = 0f;


        private void Start()
        {
            //adjust camera
            offset.y = -1;
            offset.z = -1.2f;
        }

        private void Update()
        {
            //camera zoom
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            //camera rotation - get horizontal button
            currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                //camera moviment
                transform.position = target.position - offset * currentZoom;
                transform.LookAt(target.position + Vector3.up * pitch);

                transform.RotateAround(target.position, Vector3.up, currentYaw);
            }
        
        }
    }

}