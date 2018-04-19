using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class CameraMovement : MonoBehaviour
    {
        private Vector3 _mouseOrigin;
        private bool _rotating;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseOrigin = Input.mousePosition;
                _rotating = true;
            }

            if (!Input.GetMouseButton(0))
                _rotating = false;

            if (_rotating)
            {
                Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - _mouseOrigin);

                transform.RotateAround(transform.position, transform.right, -position.y * 8);
                transform.RotateAround(transform.position, Vector3.up, position.x * 8);
            }

            if (Input.GetKey(KeyCode.W))
                transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
            else if (Input.GetKey(KeyCode.S))
                transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.A))
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.2f);
            else if (Input.GetKey(KeyCode.D))
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.2f);
        }
    }
}
