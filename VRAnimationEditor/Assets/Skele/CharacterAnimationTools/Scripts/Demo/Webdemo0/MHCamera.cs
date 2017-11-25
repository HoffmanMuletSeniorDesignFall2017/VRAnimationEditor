
using UnityEngine;
using System;

namespace MH
{
    /// <summary>
    /// used WSADQE and mouse to control cam movement
    /// </summary>
    public class MHCamera : MonoBehaviour
    {

        public float m_fXRotMul = 1.1f;
        public float m_fYRotMul = 1.1f;

        public float m_fMovSpd = 15f; // per second	

        [Tooltip("if true, will lock the cursor")]
        public bool m_shouldLock = true;

        [Tooltip("if true, the movemvent is executed in world coordinate system")]
        public bool m_moveInWorldSys = false;

        private Transform m_tr;

        // Use this for initialization
        void Start()
        {
            m_tr = transform;
            _LockCursor(true);
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                _LockCursor(false);
            }
            else
            {
                _LockCursor(true);

                float XRotDelta = -Input.GetAxis("Mouse Y") * m_fXRotMul;
                float YRotDelta = Input.GetAxis("Mouse X") * m_fYRotMul;

                Vector3 euler = m_tr.eulerAngles;
                float xRot = euler.x;
                float yRot = euler.y;
                xRot += XRotDelta;
                yRot = Mathf.Repeat(yRot + YRotDelta, 360f);
                transform.eulerAngles = new Vector3(xRot, yRot, 0);
            }

            Vector3 mov = Vector3.zero;
            mov.x = Input.GetAxis("Horizontal") * Time.deltaTime * m_fMovSpd;
            mov.z = Input.GetAxis("Vertical") * Time.deltaTime * m_fMovSpd;

            if (Input.GetKey(KeyCode.E))
            {
                mov.y = Time.deltaTime * m_fMovSpd;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                mov.y = -Time.deltaTime * m_fMovSpd;
            }

            if( m_moveInWorldSys )
            {
                m_tr.Translate(mov, Space.World);
            }
            else
            {
                m_tr.Translate(mov, Space.Self);
            }
        }

        private void _LockCursor(bool bLock)
        {
#if ! (UNITY_5_3_OR_NEWER || UNITY_5_3)
            Screen.lockCursor = m_shouldLock && bLock;
#else
            Cursor.lockState = (m_shouldLock && bLock) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !(m_shouldLock && bLock);
#endif
        }
    }

}