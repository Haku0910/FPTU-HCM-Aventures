using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject questionNPCPrefab;
    public GameObject checkinNPCPrefab;
    public GameObject shoppingNPCPrefab;

    public void CreateQuestionNPC(float x, float y, float z, Quaternion rotation, string Status, TaskDto taskType, string major, string location)
    {
        Vector3 position = new Vector3(x, y, z);
        GameObject npc = Instantiate(questionNPCPrefab, position, rotation);
        // L?y transform c?a con ??u ti�n trong prefab
        Transform firstChild = npc.transform.GetChild(0);

        // ??t v? tr� c?a con ??u ti�n theo v? tr� x, y, z ?� ch? ??nh
        firstChild.position = position;
        npc.GetComponent<NPCQuestionAndAnswer>().SetTaskDto(taskType);
        npc.GetComponent<NPCQuestionAndAnswer>().SetMajor(major);
        npc.GetComponent<NPCQuestionAndAnswer>().SetLocation(location);
        npc.GetComponent<NPCQuestionAndAnswer>().SetStatus(Status);
    }

    public void CreateCheckinNPC(float x, float y, float z, Quaternion rotation, string Status, TaskDto taskType, string major, string location)
    {
        Vector3 position = new Vector3(x, y, z);
        GameObject npc = Instantiate(checkinNPCPrefab, position, rotation);
        Transform firstChild = npc.transform.GetChild(0);

        // ??t v? tr� c?a con ??u ti�n theo v? tr� x, y, z ?� ch? ??nh
        firstChild.position = position;
        npc.GetComponent<CheckInNpc>().SetTaskDto(taskType);
        npc.GetComponent<CheckInNpc>().SetMajor(major);
        npc.GetComponent<CheckInNpc>().SetLocation(location);
        npc.GetComponent<CheckInNpc>().SetStatus(Status);
    }

    /*public void CreateShoppingNPC(Vector3 position, Quaternion rotation, string taskType, string major, Location location)
    {
        GameObject npc = Instantiate(shoppingNPCPrefab, position, rotation);
        npc.GetComponent<NPC>().SetTaskType(taskType);
        npc.GetComponent<NPC>().SetMajor(major);
        npc.GetComponent<NPC>().SetLocation(location);
    }*/
}
