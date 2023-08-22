using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    private const string API_Target = "https://649052411e6aa71680cb0654.mockapi.io/users";
    private const string API_Room = "https://649052411e6aa71680cb0654.mockapi.io/area";

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private FixedJoystick fixedJoystick;

    private bool isRunning = false;
    private bool isMouseLocked = true;
    private bool isLineVisible = false;
    private bool isAuto = false;
    private bool isAgentEnter = false;
    private bool isWalkBack = false;

    Text textButton;

    public Button togglePathButton;
    public Dropdown positionDropdown;
    public Dropdown showAreaDropdown;
    public LineRenderer lineRenderer;
    //public Canvas mainCanvas;
    public Text ShowFootstep;


    float horizontalInput = 0;
    float verticalInput = 0;

    List<Vector3> objectPositions = new List<Vector3>();
    private List<string> areaNames;
    Vector3 movement;

    private Animator animator;
    private NavMeshAgent agent;

    private bool lockCamera = true;

    float xRotation;

    private void Start()
    {
        GetApi();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();

        agent.stoppingDistance = 2f;
        agent.speed = moveSpeed;

        textButton = togglePathButton.GetComponentInChildren<Text>();


        // Load the JSON file and retrieve the area names
        /*string filePath = "Assets/Scripts/Json/Area.json";
        string jsonString = File.ReadAllText(filePath);
        areaNames = JsonConvert.DeserializeObject<List<string>>(jsonString);*/

        //mainCanvas.gameObject.SetActive(true);

        lineRenderer.enabled = false;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;

        /*        objectPositions = new List<Vector3>()
                        {
                            new Vector3(0, 0, 0),
                            new Vector3(-52f, 4f, 25f),
                            new Vector3(68f, 0f, 25f),
                        };*/



    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        horizontalInput = fixedJoystick.Horizontal;
        verticalInput = fixedJoystick.Vertical;

        if (!isAgentEnter && isMouseLocked)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotationSpeed);
        }

        /*float mouseX = 0;
        float mouseY = 0;*/

        /*if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
        {
            if (Touchscreen.current.touches.Count > 1 && Touchscreen.current.touches[1].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[1].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[1].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }
        }
        else
        {
            if (Touchscreen.current.touches.Count > 0 && Touchscreen.current.touches[0].isInProgress)
            {
                if (EventSystem.current.IsPointerOverGameObject(Touchscreen.current.touches[0].touchId.ReadValue()))
                    return;

                Vector2 touchDeltaPosition = Touchscreen.current.touches[0].delta.ReadValue();
                mouseX = touchDeltaPosition.x;
                mouseY = touchDeltaPosition.y;
            }
        }

        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * rotationSpeed);*/


        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isMouseLocked = !isMouseLocked;
            Debug.Log("mouse lock" + isMouseLocked);

            if (isMouseLocked)
            {
                //lock mouse
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                agent.isStopped = false;
            }
            else
            {
                //unlock mouse
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                animator.SetBool("isWalk", false);
                animator.SetBool("isRunning", false);
            }
        }


        float currentMoveSpeed = isRunning ? moveSpeed * sprintSpeedMultiplier : moveSpeed;



        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f; // Di chuyển theo trục dọc dương (lên)
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f; // Di chuyển theo trục dọc âm (xuống)
        }

        if (Input.GetKey(KeyCode.A) || horizontalInput < 0)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkBack", false);
            animator.SetBool("isTurnLeft", true);
            animator.SetBool("isTurnRight", false);

        }
        else if (Input.GetKey(KeyCode.D) || horizontalInput > 0)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkBack", false);
            animator.SetBool("isTurnLeft", false);
            animator.SetBool("isTurnRight", true);

        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkBack", false);
            animator.SetBool("isTurnLeft", false);
            animator.SetBool("isTurnRight", false);
        }

        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.Translate(movement * currentMoveSpeed * Time.deltaTime);

        if (movement.magnitude > 0 && verticalInput > 0)
        {
            if (isRunning)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalk", false);
            }
            else
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRunning", false);
            }
        }
        else if (movement.magnitude > 0 && verticalInput < 0)
        {
            if (isRunning)
            {
                animator.SetBool("isWalkBack", true);
                animator.SetBool("isRunning", false);
            }
            else
            {
                animator.SetBool("isWalkBack", true);
                animator.SetBool("isWalk", false);
            }
        }
        else if (isAuto)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalkBack", false);
        }

        if (isAuto)
        {
            if (agent.remainingDistance != 0)
            {
                // Kiểm tra nếu agent đã đến điểm đích và dừng di chuyển
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    isAuto = false;
                    agent.ResetPath();
                    lineRenderer.enabled = false;
                    Debug.Log("đã đến");
                }
            }
        }
    }

    public void DrawPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
        {
            Vector3[] corners = path.corners;
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners);

            ActiveAgent();
            lineRenderer.enabled = true;
            isLineVisible = true;
            isAuto = true;

            agent.SetDestination(targetPosition);
            showAreaDropdown.ClearOptions();
            CalculateObjectsOnPath(targetPosition);
        }
    }

    private void OnDropdownValueChanged(Dropdown dropdown)
    {
        int vitri = dropdown.value;
        Vector3 valueSelectedPostion = objectPositions[vitri];
        DrawPath(valueSelectedPostion);

        togglePathButton.gameObject.SetActive(true);
        textButton.text = "Tắt chỉ đường";
        togglePathButton.onClick.AddListener(TogglePathVisibility);
    }

    private void TogglePathVisibility()
    {
        lineRenderer.enabled = false;
        agent.ResetPath();
        isAuto = false;
        togglePathButton.gameObject.SetActive(false);
        showAreaDropdown.gameObject.SetActive(false);
    }

    public void ActiveAgent()
    {
        agent.isStopped = false;
        isAgentEnter = false;
    }

    public void InactiveAgent()
    {
        agent.isStopped = true;
        isAgentEnter = true;
    }
    public void CalculateObjectsOnPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        HashSet<string> objectNamesSet = new HashSet<string>();
        showAreaDropdown.gameObject.SetActive(true);
        if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
        {
            Vector3[] corners = path.corners;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Vector3 startPoint = corners[i];
                Vector3 endPoint = corners[i + 1];

                // Tìm các đối tượng nằm trên đoạn đường từ startPoint đến endPoint
                Collider[] colliders = Physics.OverlapCapsule(startPoint, endPoint, agent.radius);

                foreach (Collider collider in colliders)
                {
                    // Xử lý đối tượng (ví dụ: lưu lại, hiển thị, ...)
                    GameObject obj = collider.gameObject;
                    string objName = obj.name;
                    if (!objectNamesSet.Contains(objName))
                    {
                        objectNamesSet.Add(objName);
                    }
                }
            }
        }
        int count = objectNamesSet.Count;
        List<string> element = new List<string>(objectNamesSet);
        List<string> options = new List<String>();
        options.Insert(0, "Đây là những chỗ bạn cần đi");
        String textToShow = "";
        foreach (String ele in element)
        {
            if (areaNames.Contains(ele))
            {
                Debug.Log(ele);
                options.Add(ele);
                textToShow += ele + "\n";
            }
        }
        ShowFootstep.text = textToShow;

        showAreaDropdown.AddOptions(options);
        showAreaDropdown.onValueChanged.RemoveAllListeners();
    }

    public void SetMovingFromJoytick(float valueHorizontal, float valueVertical)
    {
        horizontalInput = valueHorizontal;
        verticalInput = valueVertical;
    }

    public void GetApi()
    {
        StartCoroutine(GetDataFromAPI());
        StartCoroutine(GetRoomFromAPI());
    }

    private IEnumerator GetDataFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(API_Target);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error retrieving data from API: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            ProcessJSONData(json);
        }
    }

    private void ProcessJSONData(string json)
    {
        Vector3[] vectorArray = Newtonsoft.Json.JsonConvert.DeserializeObject<Vector3[]>(json);
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (Vector3 vector in vectorArray)
        {
            objectPositions.Add(vector);
            Debug.Log("vector in character:" + vector);
        }
        for (int i = 0; i < objectPositions.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = "Target " + (i + 1);
            options.Add(option);
        }

        positionDropdown.ClearOptions();
        positionDropdown.AddOptions(options);

        positionDropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(positionDropdown); });
    }
    private IEnumerator GetRoomFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(API_Room);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error retrieving data from API: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            areaNames = JsonConvert.DeserializeObject<List<string>>(json);
        }
    }
}
