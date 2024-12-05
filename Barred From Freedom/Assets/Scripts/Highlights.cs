using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlights : MonoBehaviour
{
    //List of selectable tags
    [SerializeField] private string[] selectableTags = {"SelectableHeal", "SelectablePower", "SelectableSpawn", "Selectable"};

    // Start is called before the first frame update
    private void Start()
    {
        foreach (string tag in selectableTags)
        {
            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Selectable");
            foreach (GameObject clickable in clickables)
            {
                if (clickable.GetComponent<Outline>() == null)
                {
                    clickable.AddComponent<Outline>();
                    clickable.GetComponent<Outline>().enabled = false;
                }

                if (clickable.GetComponent<OutlineSelection>() == null)
                {
                    clickable.AddComponent<OutlineSelection>();
                }
            }
        }
    }
}