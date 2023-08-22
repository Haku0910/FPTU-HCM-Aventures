using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AreaNameDisplay : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public Text areaNameText; // Reference to the UI text element to display the area name
    public GameObject ToActive;

    private List<string> areaNames; // List to store the names of the areas from JSON file

    private const string API_Room = "https://649052411e6aa71680cb0654.mockapi.io/area";

    private void Start()
    {
        StartCoroutine(GetRoomFromAPI());
    }

    private void Update()
    {
        // Check if the agent is currently on the NavMesh
        if (agent.isOnNavMesh)
        {
            // Perform a raycast from the agent's position downward
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, Vector3.down, out hit))
            {
                // Check if the hit collider has a name
                if (!string.IsNullOrEmpty(hit.collider.gameObject.name))
                {
                    string areaName = hit.collider.gameObject.name;

                    Debug.Log("area name: " + areaName);

                    // Check if the area name is in the list of area names from JSON
                    if (areaNames != null)
                    {
                        if (areaNames.Contains(areaName))
                        {
                            // Display the area name on the UI text element
                            ToActive.gameObject.SetActive(true);
                            areaNameText.text = areaName;
                            HideAreaNameAfterDelay(3f);
                        }
                        else
                        {
                            // Clear the UI text element if the area name is not in the JSON list
                            areaNameText.text = "";
                        }
                    }
                }
            }
        }
    }
    private IEnumerator HideAreaNameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToActive.gameObject.SetActive(false);
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
            Debug.Log("area names: " + areaNames);
        }
    }
}
