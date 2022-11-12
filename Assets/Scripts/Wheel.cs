using System;
using System.IO;
using System.Net;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public enum WheelOwner
{
    General,
    Paul,
    Test
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
    [SerializeField] private GameObject arrow;
    
    [SerializeField] private WheelOwner wheelOwner;
    
    private WheelItem[] wheelItems;

    private bool launched = false;

    private float arrowRotationSpeed;
    private float arrowRotationZ;
    private float angularDrag;
    
    // Start is called before the first frame update
    void Start()
    {
        // Remove constraint on maximum angular velocity
        arrow.GetComponent<Rigidbody>().maxAngularVelocity = 1000.0f;
        
        RetrieveData();
        CreateWheel();
    }

    void CreateWheel()
    {
        float startingAngle = 0f;
        Vector3 startingPos = new Vector3(-3.0f, 0.0f, 0.0f);
        Vector3 endingPos = new Vector3(-4.5f, 1.0f, 0.0f);
        
        // Create panels 
        
        
        // Check if some angles are more than 30 degrees
        CheckForThirtyDegreesAngle();

        // Fill wheel
        foreach (var item in wheelItems)
        {
            // Instantiate mark
            GameObject spawnedMark = Instantiate(markPrefab, 
                new Vector3(0.0f, -0.01f, -0.10f), 
                new Quaternion(.0f, -180.0f, 0.0f, 0.0f));
            // Instantiate color background
            GameObject spawnedTriangle = Instantiate(trianglePrefab,
                Vector3.zero, 
                Quaternion.identity);
            
            // Update mark position and color
            float itemAngle = PercentageToDegreeAngle(item.itemPercentage);
            spawnedMark.GetComponent<Transform>().Rotate(Vector3.forward, startingAngle + itemAngle);
        
            // Get mark end position after its rotation
            endingPos = spawnedMark.transform.GetChild(0).gameObject.GetComponent<Transform>().position;

            // Change material colors
            GameObject spawnedMarkChild = spawnedMark.transform.GetChild(0).gameObject;
            spawnedMarkChild.GetComponent<Renderer>().material.color = item.itemColor;
            spawnedMarkChild = spawnedMark.transform.GetChild(1).gameObject;
            spawnedMarkChild.GetComponent<Renderer>().material.color = item.itemColor;
        
            // Update color background position and color
            DrawTriangle triangleScript = spawnedTriangle.GetComponent<DrawTriangle>();
            triangleScript.UpdateTriangle(startingPos, endingPos, item.itemColor);
        
            // Store mark end position
            startingPos = endingPos;
            
            startingAngle += itemAngle;
        }
    }

    void CheckForThirtyDegreesAngle()
    {
        for (int i = 0; i < wheelItems.Length; i++)
        {
            float currentPercentage = wheelItems[i].itemPercentage;
            
            if (currentPercentage > 30.0f)
            {
                float nbToCreate = currentPercentage / 30.0f;
                int nbToCreateInt = Convert.ToInt32(nbToCreate);
                
                Debug.Log(nbToCreate + " " + nbToCreateInt);

                for (int j = 0; j < nbToCreateInt; j++)
                {
                    WheelItem newItem = wheelItems[i];
                    newItem.itemPercentage = 30.0f;
                    ArrayUtility.Insert(ref wheelItems, i, newItem);
                }

                WheelItem lastItem = wheelItems[i];
                lastItem.itemPercentage = currentPercentage - (30 * nbToCreateInt);
                ArrayUtility.Insert(ref wheelItems, i, lastItem);
                
                // Remove wheelItem[i]
                ArrayUtility.Remove(ref wheelItems, wheelItems[i + nbToCreateInt + 1]);
            }
        }
    }
    
    float PercentageToDegreeAngle(float percentage)
    {
        return percentage * 360f / 100f;
    }
    
    public void LaunchWheel()
    {
        if (launched)
        {
            // Stop arrow
            launched = false;
        }
        else
        {
            // Start arrow rotation
            launched = true;
            
            // Randomize arrow values
            arrowRotationSpeed = Random.Range(6.0f, 10.0f);
            arrowRotationZ = Random.Range(2.0f, 4.0f);
            angularDrag = Random.Range(0.5f, 2.0f);

            arrow.GetComponent<Rigidbody>().angularDrag = angularDrag;
        }
    }

    private void Update()
    {
        if (launched)
        {
            arrow.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.back * arrowRotationSpeed);
        }
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
        
        // if no save exists yet, create a new file at saves location
        if (!File.Exists(Application.dataPath + "wheelsPatterns.json"))
        {
            File.Create(Application.dataPath + "wheelsPatterns.json");
        }

        File.WriteAllText(Application.dataPath + "wheelsPatterns.json", json);
    }
    
    void RetrieveData()
    {
        string jsonFile="";
        //if no save exists, take default resource file
        if (!File.Exists(Application.dataPath + "wheelsPatterns.json"))
        {
            TextAsset file  = Resources.Load<TextAsset>("wheelsPatterns");
            jsonFile = file.text;
        }
        else
        {
            jsonFile = File.ReadAllText(Application.dataPath + "wheelsPatterns.json");
        }
        AllWheels allWheels = JsonUtility.FromJson<AllWheels>(jsonFile);
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