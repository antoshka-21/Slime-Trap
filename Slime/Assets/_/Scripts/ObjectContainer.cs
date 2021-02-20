using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int count;
    [SerializeField] private Transform path;
    [SerializeField] private string folderName;



    public List<GameObject> objs { get; private set; } = new List<GameObject>();



    private void Awake()
    {
        var folder = path;

        if (folderName != "")
        {
            Transform existingFolder = path.Find(folderName);

            if (existingFolder != null)
            {
                folder = existingFolder;
            }

            else
            {
                folder = new GameObject(folderName).transform;
                folder.parent = path;
            }
        }

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(prefab, folder);
            obj.SetActive(false);
            objs.Add(obj);
        }
    }



    public void GetAllObjects()
    {
        if (objs != null)
        {
            for (int i = 0; i < count; i++)
            {
                if (!objs[i].activeInHierarchy)
                {
                    objs[i].SetActive(true);
                }
            }
        }
    }



    public GameObject GetObject(bool active = true)
    {
        if (objs != null)
        {
            for (int i = 0; i < count; i++)
            {
                if (!objs[i].activeInHierarchy)
                {
                    objs[i].SetActive(active);
                    return objs[i];
                }
            }
        }

        return null;
    }
}
