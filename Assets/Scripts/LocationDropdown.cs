using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LocationDropdown : MonoBehaviour
{
    private const string API_URL = "http://anhkiet-001-site1.htempurl.com/api/Locations";
    public Dropdown positionDropdown;
    public Dropdown locationRoomDropdown;
    public Button backButton;

    [SerializeField] private float moveSpeed = 7;

    // Các danh sách vị trí tương ứng với từng tầng
    private List<LocationData> floorLocations = new List<LocationData>();
    private List<LocationData> floor1Locations = new List<LocationData>();
    private List<LocationData> floor2Locations = new List<LocationData>();
    private List<LocationData> floor3Locations = new List<LocationData>();
    private List<LocationData> specialLocation = new List<LocationData>();
    private List<LocationData> locationNames = new List<LocationData>();

    Dropdown.OptionData floor = new Dropdown.OptionData("Tầng trệt");
    Dropdown.OptionData floor1 = new Dropdown.OptionData("Tầng 1");
    Dropdown.OptionData floor2 = new Dropdown.OptionData("Tầng 2");
    Dropdown.OptionData floor3 = new Dropdown.OptionData("Tầng 3");
    Dropdown.OptionData specialPlace = new Dropdown.OptionData("Khu vực khác");

    public CharacterMoving characterMoving;

    private void Awake()
    {
        StartCoroutine(GetFullLocationName(GetFullLocationName));
    }

    private void Start()
    {

        positionDropdown.gameObject.SetActive(true);
        locationRoomDropdown.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        positionDropdown.ClearOptions();

        Dropdown.OptionData defaultOption = new Dropdown.OptionData("**Các tầng có thể đi**");
        positionDropdown.options.Add(defaultOption);

        positionDropdown.options.Add(floor);
        positionDropdown.options.Add(floor1);
        positionDropdown.options.Add(floor2);
        positionDropdown.options.Add(floor3);
        positionDropdown.options.Add(specialPlace);

        positionDropdown.onValueChanged.AddListener(OnPositionValueChange);
        positionDropdown.RefreshShownValue();
    }

    private void OnPositionValueChange(int index)
    {

        switch (index)
        {
            case 0:
                break;
            case 1:
                ShowFloorLocationsList(floorLocations);
                Debug.Log("show list tầng trệt");
                break;
            case 2:
                ShowFloorLocationsList(floor1Locations);
                Debug.Log("show list tầng 1");
                break;
            case 3:
                ShowFloorLocationsList(floor2Locations);
                Debug.Log("show list tầng 2");
                break;
            case 4:
                ShowFloorLocationsList(floor3Locations);
                Debug.Log("show list tầng 3");
                break;
            case 5:
                ShowFloorLocationsList(specialLocation);
                Debug.Log("show list các khu vực đặc biệt");
                break;
            default:
                break;
        }
        backButton.gameObject.SetActive(true);
        positionDropdown.gameObject.SetActive(false);
        locationRoomDropdown.gameObject.SetActive(true);
    }

    private void ShowFloorLocationsList(List<LocationData> locations)
    {
        // Xóa tất cả các tùy chọn hiện có trong dropdown
        locationRoomDropdown.ClearOptions();

        // Thêm các vị trí từ danh sách cụ thể vào dropdown
        foreach (LocationData location in locations)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(location.locationName);
            locationRoomDropdown.options.Add(option);
        }

        locationRoomDropdown.onValueChanged.AddListener((index) => OnfloorValueChange(index, locations));

        // Cập nhật dropdown
        locationRoomDropdown.RefreshShownValue();
    }

    private void OnfloorValueChange(int index, List<LocationData> locations)
    {
        if (index >= 0 && index < locations.Count)
        {
            string selectedLocationName = locations[index].locationName;
            Debug.Log("room đã chọn:" + selectedLocationName);
            for (int i = 0; i < locations.Count; i++)
            {
                if (selectedLocationName.Equals(locations[i].locationName))
                {
                    Vector3 xyz = ShowLocationRoom(i, locations);
                    Debug.Log("tọa đồ đã chọn:" + xyz);
                    characterMoving.DrawPath(xyz);
                }
            }
        }
    }


    private Vector3 ShowLocationRoom(int index, List<LocationData> locations)
    {
        Vector3 vector3 = new Vector3();
        vector3.x = float.Parse(locations[index].x.ToString());
        vector3.y = float.Parse(locations[index].y.ToString());
        vector3.z = float.Parse(locations[index].z.ToString());
        return vector3;
    }

    private IEnumerator GetFullLocationName(Action<List<LocationData>> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL))
        {
            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Parse JSON response to extract "data" array
                LocationListDataWrapper wrapper = JsonUtility.FromJson<LocationListDataWrapper>(response);
                locationNames = wrapper.data;
                // Call the callback function with the location names list
                callback?.Invoke(locationNames);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
    void GetFullLocationName(List<LocationData> locationDataList)
    {
        List<LocationData> tempFloorLocations = new List<LocationData>();
        List<LocationData> tempFloor1Locations = new List<LocationData>();
        List<LocationData> tempFloor2Locations = new List<LocationData>();
        List<LocationData> tempFloor3Locations = new List<LocationData>();
        List<LocationData> tempSpecialLocation = new List<LocationData>();


        foreach (LocationData data in locationNames)
        {
            if (data.locationName.Length > 6)
            {
                char floorChar = data.locationName[5];
                // Dựa vào ký tự số, phân loại vào danh sách tương ứng

                switch (floorChar)
                {
                    case '0':
                        tempFloorLocations.Add(data);
                        break;
                    case '1':
                        tempFloor1Locations.Add(data);
                        break;
                    case '2':
                        tempFloor2Locations.Add(data);
                        break;
                    case '3':
                        tempFloor3Locations.Add(data);
                        break;
                    default:
                        tempSpecialLocation.Add(data);
                        break;
                }
            }
            else
            {
                tempSpecialLocation.Add(data);
            }
        }

        // Sau khi vòng lặp kết thúc, thêm các phần tử từ danh sách tạm thời vào danh sách chính
        floorLocations.AddRange(tempFloorLocations);
        floor1Locations.AddRange(tempFloor1Locations);
        floor2Locations.AddRange(tempFloor2Locations);
        floor3Locations.AddRange(tempFloor3Locations);
        specialLocation.AddRange(tempSpecialLocation);

        floorLocations.Sort((a, b) => a.locationName.CompareTo(b.locationName));
        floor1Locations.Sort((a, b) => a.locationName.CompareTo(b.locationName));
        floor2Locations.Sort((a, b) => a.locationName.CompareTo(b.locationName));
        floor3Locations.Sort((a, b) => a.locationName.CompareTo(b.locationName));
        specialLocation.Sort((a, b) => a.locationName.CompareTo(b.locationName));


        // Tiếp tục xử lý sau khi đã thêm các phần tử vào danh sách chính
        // ...

        // Clear danh sách tạm thời
        tempFloorLocations.Clear();
        tempFloor1Locations.Clear();
        tempFloor2Locations.Clear();
        tempFloor3Locations.Clear();
        tempSpecialLocation.Clear();
    }

    public void OnBackButtonClick()
    {
        positionDropdown.gameObject.SetActive(true);
        locationRoomDropdown.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}