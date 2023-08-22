using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerTouchMovement : MonoBehaviour
{
    [SerializeField] private Vector2 joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatJoystick joystick;
    [SerializeField] private NavMeshAgent player;

    private Finger MovementFinger;
    private Vector2 MovementAmount;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleFingerMove;
        ETouch.Touch.onFingerMove += HandleLoseFinger;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleFingerMove;
        ETouch.Touch.onFingerMove -= HandleLoseFinger;
        EnhancedTouchSupport.Disable();
    }

    private void HandleFingerMove(Finger MoveFinger)
    {
        if (MoveFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = joystickSize.x / 2f;
            ETouch.Touch currentTouch = MovementFinger.currentTouch;

            if (Vector2.Distance(
                currentTouch.screenPosition, joystick.RectTransform.anchoredPosition
                ) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - joystick.RectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joystick.RectTransform.anchoredPosition;
            }

            joystick.Knob.anchoredPosition = knobPosition;
            MovementAmount = knobPosition / maxMovement;
        }
    }
    private void HandleLoseFinger(Finger LostFinger)
    {
        if (LostFinger == MovementFinger)
        {
            MovementFinger = null;
            joystick.Knob.anchoredPosition = Vector2.zero;
            joystick.gameObject.SetActive(false);
            MovementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchFinger)
    {
        if (MovementFinger == null && TouchFinger.screenPosition.x <= Screen.width / 2f)
        {
            MovementFinger = TouchFinger;
            MovementAmount = Vector2.zero;
            joystick.gameObject.SetActive(true);
            joystick.RectTransform.sizeDelta = joystickSize;
            joystick.RectTransform.anchoredPosition = ClampStartPosition(TouchFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPosition)
    {
        if (StartPosition.x < joystickSize.x / 2)
        {
            StartPosition.x = joystickSize.x / 2;
        }

        if (StartPosition.y < joystickSize.y / 2)
        {
            StartPosition.y = joystickSize.y / 2;
        }
        else if (StartPosition.y > Screen.height - joystickSize.y / 2)
        {
            StartPosition.y = Screen.height - joystickSize.y / 2;
        }
        return StartPosition;
    }

    private void Update()
    {
        Vector3 scaleMovement = player.speed * Time.deltaTime * new Vector3(
            MovementAmount.x,
            0,
            MovementAmount.y);
        player.transform.LookAt(player.transform.position + scaleMovement, Vector3.up);
        player.Move(scaleMovement);

    }
}

