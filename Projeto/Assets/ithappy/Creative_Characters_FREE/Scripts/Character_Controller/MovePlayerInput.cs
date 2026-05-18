using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CharacterMover))]
    public class MovePlayerInput : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";

        [SerializeField]
        private string m_VerticalAxis = "Vertical";

        [SerializeField]
        private string m_JumpButton = "Jump";

        [SerializeField]
        private KeyCode m_RunKey = KeyCode.LeftShift;

        [Header("Camera")]
        [SerializeField]
        private PlayerCamera m_Camera;

        [SerializeField]
        private string m_MouseX = "Mouse X";

        [SerializeField]
        private string m_MouseY = "Mouse Y";

        [SerializeField]
        private string m_MouseScroll = "Mouse ScrollWheel";

        private CharacterMover m_Mover;

        private Vector2 m_Axis;
        private bool m_IsRun;
        private bool m_IsJump;

        private Vector3 m_Target;
        private Vector2 m_MouseDelta;
        private float m_Scroll;

        // Controle da câmera
        private bool m_CameraLocked = false;

        private void Awake()
        {
            m_Mover = GetComponent<CharacterMover>();

            if (m_Camera == null)
            {
                m_Camera = Camera.main == null
                    ? null
                    : Camera.main.GetComponent<PlayerCamera>();
            }

            if (m_Camera != null)
            {
                m_Camera.SetPlayer(transform);
            }
        }

        private void Update()
        {
            ToggleCamera();

            GatherInput();
            SetInput();
        }

        private void ToggleCamera()
        {
            // Alterna trava/destrava ao clicar botão direito
            if (Input.GetMouseButtonDown(1))
            {
                m_CameraLocked = !m_CameraLocked;

                // Opcional: trava/libera cursor
                Cursor.lockState = m_CameraLocked
                    ? CursorLockMode.Locked
                    : CursorLockMode.None;

                Cursor.visible = !m_CameraLocked;
            }
        }

        public void GatherInput()
        {
            m_Axis = new Vector2(
                Input.GetAxis(m_HorizontalAxis),
                Input.GetAxis(m_VerticalAxis)
            );

            m_IsRun = Input.GetKey(m_RunKey);
            m_IsJump = Input.GetButton(m_JumpButton);

            m_Target = (m_Camera == null)
                ? Vector3.zero
                : m_Camera.Target;

            // Se a câmera estiver travada, não pega movimento do mouse
            if (!m_CameraLocked)
            {
                m_MouseDelta = new Vector2(
                    Input.GetAxis(m_MouseX),
                    Input.GetAxis(m_MouseY)
                );
            }
            else
            {
                m_MouseDelta = Vector2.zero;
            }

            m_Scroll = Input.GetAxis(m_MouseScroll);
        }

        public void BindMover(CharacterMover mover)
        {
            m_Mover = mover;
        }

        public void SetInput()
        {
            if (m_Mover != null)
            {
                m_Mover.SetInput(
                    in m_Axis,
                    in m_Target,
                    in m_IsRun,
                    m_IsJump
                );
            }

            if (m_Camera != null)
            {
                m_Camera.SetInput(in m_MouseDelta, m_Scroll);
            }
        }
    }
}