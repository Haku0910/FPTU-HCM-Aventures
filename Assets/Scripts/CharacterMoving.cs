using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMoving : MonoBehaviourPun
{
    private const string API_URL = "http://anhkiet-001-site1.htempurl.com/api/Locations";

    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    private float rotationSpeed = 50f;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private FloatingJoystick fixedTouchField;
    [SerializeField] private PlatformDetector platformDetector;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera currentCamera;
    [SerializeField] private GameObject sprintButton;
    public float maxVerticalRotation = 45f; // Góc quay tối đa lên trên
    public float minVerticalRotation = -45f;
    private float currentVerticalRotation = 0f;

    private bool checkPlatform;
    private bool isMine = false;
    private bool isRunning = false;
    private bool isMouseLocked = true;
    private bool isLineVisible = false;
    private bool isAuto = false;
    private bool isRotating = false;

    public LineRenderer lineRenderer;
    public ButtonManager buttonManager;

    [SerializeField] private float verticalRotation = 0f;
    [SerializeField] private float speedY = 2f;
    float horizontalInput = 0;
    float verticalInput = 0;

    private List<string> areaNames;
    Vector3 movement;

    private Animator animator;
    private NavMeshAgent agent;


    private void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        else
        {
            //GetApi();

            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            lineRenderer = GetComponent<LineRenderer>();

            agent.stoppingDistance = 2f;
            agent.speed = moveSpeed;


            lineRenderer.enabled = false;
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            lineRenderer.positionCount = 0;
            checkPlatform = platformDetector.PlatformCheck();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            MoveAction();
        }
    }

    private void MoveAction()
    {

        /*if (checkPlatform)
        {
            
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }*/
        /* old code here
        horizontalInput = fixedJoystick.Horizontal;
        verticalInput = fixedJoystick.Vertical;

        // Xử lý di chuyển người chơi
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Xử lý tốc độ chạy
        isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentMoveSpeed = isRunning ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        transform.Translate(movement * currentMoveSpeed * Time.deltaTime);


        // Kiểm tra số lượng touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Xử lý xoay camera bằng cảm ứng
            if (touch.position.x >= Screen.width * 0.5f)
            {
                // Tính toán độ xoay dựa trên sự chuyển động cảm ứng
                float rotationInput = fixedTouchField.TouchDist.x;
                transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);

                float verticalRotationInput = fixedTouchField.TouchDist.y;
                float newVerticalRotation = currentVerticalRotation + verticalRotationInput * -rotationSpeed * Time.deltaTime;

                // Giới hạn góc quay trong khoảng tối đa và tối thiểu
                newVerticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalRotation, maxVerticalRotation);

                // Tính toán sự thay đổi góc quay
                float rotationChange = newVerticalRotation - currentVerticalRotation;
                currentVerticalRotation = newVerticalRotation;

                // Áp dụng sự thay đổi góc quay vào camera
                mainCamera.transform.localRotation = Quaternion.Euler(newVerticalRotation, 0f, 0f);
            }
        }

        */

        horizontalInput = fixedJoystick.Horizontal;
        verticalInput = fixedJoystick.Vertical;

        // Xử lý di chuyển người chơi
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Xử lý tốc độ chạy
        isRunning = Input.GetKey(KeyCode.LeftShift);

        float currentMoveSpeed = isRunning ? moveSpeed * sprintSpeedMultiplier : moveSpeed;

        // Di chuyển
        transform.Translate(movement * currentMoveSpeed * Time.deltaTime);


        float rotationInput = fixedTouchField.Horizontal;
        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);

        if (rotationInput > 0)
        {
            animator.SetBool("isTurnRight", true);
            animator.SetBool("isTurnLeft", false);
        }
        else if (rotationInput < 0)
        {
            animator.SetBool("isTurnRight", false);
            animator.SetBool("isTurnLeft", true);
        }
        else
        {
            animator.SetBool("isTurnRight", false);
            animator.SetBool("isTurnLeft", false);
        }

        float verticalRotationInput = fixedTouchField.Vertical;
        float newVerticalRotation = currentVerticalRotation + verticalRotationInput * -rotationSpeed * Time.deltaTime;

        // Giới hạn góc quay trong khoảng tối đa và tối thiểu
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalRotation, maxVerticalRotation);

        // Tính toán sự thay đổi góc quay
        float rotationChange = newVerticalRotation - currentVerticalRotation;
        currentVerticalRotation = newVerticalRotation;

        // Áp dụng sự thay đổi góc quay vào camera
        mainCamera.transform.localRotation = Quaternion.Euler(newVerticalRotation, 0f, 0f);



        /*if (checkPlatform)
        {
        }
        else
        {
            // Xoay đối tượng người chơi dựa trên đầu vào của chuột
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }*/

        animator.SetFloat("horizontal", horizontalInput);
        animator.SetFloat("vertical", verticalInput);

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
            animator.SetFloat("vertical", 1);
        }
        else
        {
            animator.SetFloat("vertical", 0);
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
        }
    }

    public void ActiveAgent()
    {
        agent.isStopped = false;
    }

    public void InactiveAgent()
    {
        agent.isStopped = true;
    }
    public void CalculateObjectsOnPath(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        HashSet<string> objectNamesSet = new HashSet<string>();
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
                options.Add(ele);
                textToShow += ele + "\n";
            }
        }
    }

    public void SetSprinting(bool isRunning)
    {
        this.isRunning = isRunning;
    }
}
