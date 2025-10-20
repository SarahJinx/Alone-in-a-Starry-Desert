using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StreamReader srPosition = new StreamReader(Application.persistentDataPath + "/saveposition.text");

        float x = float.Parse(srPosition.ReadLine());
        float y = float.Parse(srPosition.ReadLine());
        float z = float.Parse(srPosition.ReadLine());
        transform.position = new Vector3(x, y, z);

        srPosition.Close();
    }

    private void OnDisable()
    {
        FileStream fsPosition = File.Create(Application.persistentDataPath + "/saveposition.text");
        StreamWriter swPosition = new StreamWriter(fsPosition);

        swPosition.WriteLine(transform.position.x);
        swPosition.WriteLine(transform.position.y);
        swPosition.WriteLine(transform.position.z);

        swPosition.Close();
        fsPosition.Close();
    }
}