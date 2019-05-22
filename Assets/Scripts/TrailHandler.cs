using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(TrailRenderer))]
public class TrailHandler : MonoBehaviour
{

    TrailRenderer trail;
    public List<LineRenderer> m_Array;

    [HideInInspector]
    public GameObject father;
    [TextArea]
    public string str;
    public string path;
    [Space]
    [Header("User Settings")]
    public KeyCode writeJSonInput;
    [Space]
    public bool createCurve;
    bool temp;
    public Transform spawnpoint;
    [Space]
    public Color trailColor;
    public float lineWidth = 1f;
    [Space]    
    public bool undo;
    public bool removeAll;
    // Start is called before the first frame update
    void Start()
    {
        father = new GameObject();
        father.name = "Father";
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        if (path == "")
        {
            path = Application.dataPath + "/ToMaya.json";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (createCurve && !temp)
        {

            trail.Clear();
            trail.widthMultiplier = lineWidth;
            trail.AddPosition(spawnpoint.position);
            trail.material.color = trailColor;
            trail.enabled = true;
        }
        else if (createCurve && temp)
        {

            trail.transform.position = spawnpoint.position;

        }
        else if (!createCurve && temp)
        {
            trail.enabled = false;
            GameObject c = new GameObject();
            c.name = "line";
            c.AddComponent<LineRenderer>().positionCount = trail.positionCount;
            c.GetComponent<LineRenderer>().widthMultiplier = lineWidth;
            c.GetComponent<LineRenderer>().sharedMaterial = trail.material;
            c.GetComponent<LineRenderer>().material.color = trail.material.color;

            for (int i = 0; i < trail.positionCount; i++)
            {
                c.GetComponent<LineRenderer>().SetPosition(i, trail.GetPosition(i));
            }
            trail.Clear();
            m_Array.Add(c.GetComponent<LineRenderer>());
            c.transform.SetParent(father.transform);


            trail.Clear();

        }
        temp = createCurve;


        if (removeAll)
        {
            foreach (LineRenderer item in m_Array)
            {
                Destroy(item.gameObject);
            }
            m_Array.Clear();
            removeAll = false;
        }



        if (Input.GetKeyDown(writeJSonInput))
        {
            Writer();
        }


        if (undo)
        {
            Undo();
            undo = false;
        }
    }


    public void Undo()
    {
        if (m_Array.Count - 1 != -1)
        {
            Destroy(m_Array[m_Array.Count - 1].gameObject);
            m_Array.RemoveAt(m_Array.Count - 1);
        }
    }



    public void Writer()
    {
        str = null;

        for (int i = 0; i < m_Array.Count; i++)
        {
            for (int j = 0; j < m_Array[i].positionCount; j++)
            {

                if (m_Array[i].GetComponent<LineRenderer>().positionCount - 1 == j)
                {
                    Color c = m_Array[i].GetComponent<LineRenderer>().sharedMaterial.color;
                    string colorString = c.r.ToString().Replace(",", ".") + ", " + c.g.ToString().Replace(",", ".") + ", " + c.b.ToString().Replace(",", ".");

                    str += m_Array[i].GetComponent<LineRenderer>().GetPosition(j) + "\r\n" + "COLOR" + "\r\n" + colorString + "\r\n" + "BREAK" + "\r\n";
                }
                else
                {
                    str += m_Array[i].GetComponent<LineRenderer>().GetPosition(j) + "\r\n";
                }
            }
        }

        if (str != null)
        {
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("RGBA", "");
            File.WriteAllText(path + "/ToMaya.json", str);

        }
    }


}
