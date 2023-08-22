using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Location
{
    public static class ButtonExtension
    {
        public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
        {
            button.onClick.AddListener(delegate ()
            {
                OnClick(param);
            });
        }

        public static void AddEventListener2<T>(this Button button, T param1, Action<T> OnClick, List<LocationData> param2)
        {
            button.onClick.AddListener(delegate ()
            {
                OnClick(param1);
            });
        }
    }
    public class LocationManager : MonoBehaviour
    {
        private const string API_URL = "http://anhkiet-001-site1.htempurl.com/api/Locations";
        private List<LocationData> floorLocations = new List<LocationData>();
        private List<LocationData> floor1Locations = new List<LocationData>();
        private List<LocationData> floor2Locations = new List<LocationData>();
        private List<LocationData> floor3Locations = new List<LocationData>();
        private List<LocationData> specialLocation = new List<LocationData>();
        private List<LocationData> locationNames = new List<LocationData>();

        [SerializeField] private GameObject locate;
        [SerializeField] GameObject LocationCategory;
        [SerializeField] GameObject LocationInacitve;
        [SerializeField] GameObject LocationAcitve;
        [SerializeField] private CharacterMoving characterMoving;

        [SerializeField] private GameObject panel;
        bool isBack = false;


        List<string> floor = new List<string> { "Tầng trệt", "Lầu 1", "Lầu 2", "Lầu 3", "Khu vực đặc biệt" };


        private void Awake()
        {
            StartCoroutine(GetFullLocationName(GetFullLocationName));
        }
        private void Start()
        {
            ClearPanel();
            GameObject buttonTemplate = transform.GetChild(0).gameObject;
            LoadPannel();

            Destroy(buttonTemplate);
        }

        private void LoadFloor(int index)
        {
            switch (index)
            {
                case 0:
                    ShowFloorLocationsList(floorLocations);
                    isBack = true;
                    Debug.Log("show list tầng trệt");
                    break;
                case 1:
                    ShowFloorLocationsList(floor1Locations);
                    isBack = true;
                    Debug.Log("show list tầng 1");
                    break;
                case 2:
                    ShowFloorLocationsList(floor2Locations);
                    isBack = true;
                    Debug.Log("show list tầng 2");
                    break;
                case 3:
                    ShowFloorLocationsList(floor3Locations);
                    isBack = true;
                    Debug.Log("show list tầng 3");
                    break;
                case 4:
                    ShowFloorLocationsList(specialLocation);
                    isBack = true;
                    Debug.Log("show list các khu vực đặc biệt");
                    break;
                default:
                    break;
            }
        }

        private void ShowFloorLocationsList(List<LocationData> listLocation)
        {
            ClearPanel();

            for (int i = 0; i < listLocation.Count; i++)
            {
                if (listLocation[i].status.Equals("ACTIVE"))
                {
                    GameObject g = Instantiate(LocationAcitve, transform);
                    g.transform.GetChild(0).GetComponent<TMP_Text>().text = listLocation[i].locationName.ToString();
                    Action<int> onClickAction = (index) => LocationClick(index, listLocation);
                    g.GetComponent<Button>().AddEventListener2(i, onClickAction, listLocation);
                }
                else
                {
                    GameObject g = Instantiate(LocationInacitve, transform);
                    g.transform.GetChild(0).GetComponent<TMP_Text>().text = listLocation[i].locationName.ToString();
                    Action<int> onClickAction = (index) => LocationClick(index, listLocation);
                    g.GetComponent<Button>().AddEventListener2(i, onClickAction, listLocation);
                }
            }
        }
        private void LocationClick(int index, List<LocationData> listdata)
        {
            Vector3 vector = new Vector3();
            vector.x = float.Parse(listdata[index].x.ToString());
            vector.y = float.Parse(listdata[index].y.ToString());
            vector.z = float.Parse(listdata[index].z.ToString());
            Debug.Log("địa điểm cần tới: " + vector);
            characterMoving.DrawPath(vector);
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

            List<LocationData> temp = new List<LocationData>();

            foreach (LocationData data in locationNames)
            {
                if (data.locationName.Length > 6)
                {
                    string floorChar = data.locationName.Substring(0, 6);
                    // Dựa vào ký tự số, phân loại vào danh sách tương ứng

                    switch (floorChar)
                    {
                        case "Room_0":
                            tempFloorLocations.Add(data);
                            break;
                        case "Room_1":
                            tempFloor1Locations.Add(data);
                            break;
                        case "Room_2":
                            tempFloor2Locations.Add(data);
                            break;
                        case "Room_3":
                            tempFloor3Locations.Add(data);
                            break;
                        default:
                            if (data.locationName.StartsWith("NPC"))
                            {
                                temp.Add(data);
                            }
                            else
                            {
                                tempSpecialLocation.Add(data);
                            }
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

        public void LoadPannel()
        {
            GameObject g;
            for (int i = 0; i < floor.Count; i++)
            {
                g = Instantiate(LocationCategory, transform);
                g.transform.GetChild(0).GetComponent<TMP_Text>().text = floor[i].ToString();

                g.GetComponent<Button>().AddEventListener(i, LoadFloor);
            }
        }

        public void ClearPanel()
        {
            if (panel.transform.childCount > 0)
            {
                for (int i = panel.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject childObject = transform.GetChild(i).gameObject;
                    Destroy(childObject);
                }
            }
        }

        public void onClickBackButton()
        {
            if (isBack)
            {
                ClearPanel();
                LoadPannel();
                isBack = false;
            }
            else
            {
                Close();
            }

        }

        public void Open()
        {

            locate.gameObject.SetActive(true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
            PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        }

        public void Close()
        {
            locate.gameObject.SetActive(false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
        }

    }
}