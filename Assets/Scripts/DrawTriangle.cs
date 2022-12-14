using UnityEngine;
using System.Collections;

public class DrawTriangle : MonoBehaviour {

    Mesh m;
    MeshFilter mf;

    private Vector3 originPos;
    private Vector3 startingPos;
    private Vector3 endingPos;
    [SerializeField] private Color color;

    // Use this for initialization
    void Awake ()
    {
        mf = GetComponent<MeshFilter>();
        m = new Mesh();
        mf.mesh = m;
        
        startingPos = new Vector3(-3.0f, 0.0f, 0.0f);
        endingPos = new Vector3(-4.5f, 1.0f, 0.0f);
        
        Draw();
        
        // Change material color
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void UpdateTriangle(Vector3 startingPosP, Vector3 endingPosP, Color colorP)
    {
        startingPos = startingPosP;
        endingPos = endingPosP;
        color = colorP;
        
        Draw();
        
        // Change material color
        gameObject.GetComponent<Renderer>().material.color = color;
    }
    
    //This draws a triangle
    void Draw()
    {
        //We need two arrays one to hold the vertices and one to hold the triangles
        Vector3[] VerticesArray = new Vector3[3];
        int[] trianglesArray = new int[3];

        //lets add 3 vertices in the 3d space
        VerticesArray[0] = originPos;
        VerticesArray[1] = endingPos;
        VerticesArray[2] = startingPos;

        //define the order in which the vertices in the VerticesArray should be used to draw the triangle
        trianglesArray[0] = 0;
        trianglesArray[1] = 1;
        trianglesArray[2] = 2;

        //add these two triangles to the mesh
        m.vertices = VerticesArray;
        m.triangles = trianglesArray;
    }
}