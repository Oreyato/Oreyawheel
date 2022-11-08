using System.IO;
using System.Net;
using UnityEngine;

[System.Serializable]
public enum WheelOwner
{
    General,
    Paul
}
[System.Serializable]
public struct AllWheels
{
    public WheelItems[] allWheels;
}
[System.Serializable]
public struct WheelItems
{
    public WheelOwner wheelOwner;
    public WheelItem[] wheelItems;
}

[System.Serializable]
public struct WheelItem {
    public string itemName;
    [Range(0.0f, 100.0f)]
    public float itemPercentage;

    public Color itemColor;
}

public class Wheel : MonoBehaviour
{
    [SerializeField] private GameObject markPrefab;
    [SerializeField] private GameObject trianglePrefab;

    [SerializeField] private WheelOwner wheelOwner;
    
    private WheelItem[] wheelItems;

    // Start is called before the first frame update
    void Start()
    {
        RetrieveData();
        CreateWheel();
    }

    void CreateWheel()
    {
        float startingAngle = 0f;
        
        foreach (var item in wheelItems)
        { 
            GameObject spawnedMark = Instantiate(markPrefab, 
                new Vector3(0.0f, -0.01f, 0.0f), 
                new Quaternion(.0f, -180.0f, 0.0f, 0.0f));

            GameObject spawnedTriangle = Instantiate(trianglePrefab,
                new Vector3(0.0f, -0.01f, 0.0f), 
                Quaternion.identity);

            DrawTriangle triangleScript = spawnedTriangle.GetComponent<DrawTriangle>();

            triangleScript.UpdateTriangle(
                new Vector3(-3.0f, 0.5f, .0f), 
                item.itemColor);

            float itemAngle = PercentageToDegreeAngle(item.itemPercentage);
            spawnedMark.GetComponent<Transform>().Rotate(Vector3.forward, startingAngle + itemAngle);
            GameObject spawnedMarkChild = spawnedMark.transform.GetChild(0).gameObject;
            spawnedMarkChild.GetComponent<Renderer>().material.color = item.itemColor;
            spawnedMarkChild = spawnedMark.transform.GetChild(1).gameObject;
            spawnedMarkChild.GetComponent<Renderer>().material.color = item.itemColor;

            startingAngle += itemAngle;
        }
    }

    float PercentageToDegreeAngle(float percentage)
    {
        return percentage * 360f / 100f;
    }
    
    void LaunchWheel()
    {
        
    }

    void ReturnWheelResult()
    {
        
    }

    void SaveData()
    {
        // Guys wheels
        WheelItems generalWheel = new WheelItems
        {
            wheelOwner = WheelOwner.General,
            wheelItems = wheelItems
        };
        // Own wheel
        WheelItems paulWheel = new WheelItems
        {
            wheelOwner = WheelOwner.Paul,
            wheelItems = wheelItems
        };

        WheelItems[] allWheels = { generalWheel, paulWheel };
        
        AllWheels wheels = new AllWheels
        {
            allWheels = allWheels
        };

        string json = JsonUtility.ToJson(wheels);
        
        File.WriteAllText("S:/UnityProjects/PERSONAL_PROJECTS/Oreyawheel/Assets/Scripts/wheelsPatterns.json", json);
    }
    
    void RetrieveData()
    {
        string jsonPath = File.ReadAllText("S:/UnityProjects/PERSONAL_PROJECTS/Oreyawheel/Assets/Scripts/wheelsPatterns.json");
        
        AllWheels allWheels = JsonUtility.FromJson<AllWheels>(jsonPath);
        WheelItems[] wheels = allWheels.allWheels;

        foreach (var wheel in wheels)
        {
            if (wheel.wheelOwner == wheelOwner)
            {
                wheelItems = wheel.wheelItems;
            }
        }
    }
}