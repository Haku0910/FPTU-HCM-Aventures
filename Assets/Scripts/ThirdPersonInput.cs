using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson.PunDemos;

public class ThirdPersonInput : MonoBehaviour
{

    public FixedJoystick joystick;
    protected ThirdPersonUserControl control;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<ThirdPersonUserControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("H: " + joystick.input.x + " + V: " + joystick.input.y);
        control.Hinput = joystick.input.x;
        control.Vinput = joystick.input.y;
    }
}
