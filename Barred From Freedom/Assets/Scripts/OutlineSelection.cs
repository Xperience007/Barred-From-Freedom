using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class OutlineSelection : MonoBehaviour
{
    private Outline selection;
    private TextMeshPro text;

    //Creates dictionary to store pairs of objects and selectable text
    private Dictionary<Transform, TextMeshPro> textMap = new Dictionary<Transform, TextMeshPro>();

    [System.Serializable] 
    public struct SelectableText {
        public Transform selectableObj;
        public TextMeshPro textObj;
    }

    [SerializeField]
    private SelectableText[] selectableTexts;

    private void Start() {
        selection = GetComponent<Outline>();
        
        foreach(var pair in selectableTexts) {
            textMap[pair.selectableObj] = pair.textObj;
            pair.textObj.gameObject.SetActive(false);
        }
    }

    private void Update() {
        Teleport tp = FindObjectOfType<Teleport>();
        ItemMenu item = FindObjectOfType<ItemMenu>();
        if (tp != null && !tp.inHealRoom && gameObject.CompareTag("Selectable"))
        {
            // Disable outlines and text for healing items if not in the heal room
            if (selection != null)
            {
                selection.enabled = false;
            }
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
            return;
        }
        
        if (tp != null && tp.inHealRoom && (gameObject.CompareTag("SelectableHeal") || gameObject.CompareTag("SelectablePower") || gameObject.CompareTag("SelectableSpawn"))) {
            if (selection != null)
            {
                selection.enabled = false;
            }
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
            return;
        }

        if (selection == null) {
            return;
        }

        if (!item.inMenu && !PauseMenu.GameIsPaused)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //Checks for objects on the selectable layer
            int layer = LayerMask.GetMask("Selectable");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                if (hit.transform.CompareTag("Selectable") || hit.transform.CompareTag("SelectableHeal") || hit.transform.CompareTag("SelectablePower") || hit.transform.CompareTag("SelectableSpawn"))
                {
                    Outline newSelect = hit.transform.GetComponent<Outline>();

                    TextMeshPro newText = null;

                    if (textMap.TryGetValue(hit.transform, out newText))
                    {
                        if (newSelect != null && newSelect != selection)
                        {
                            if (selection != null)
                            {
                                selection.enabled = false;
                            }

                            if (text != null)
                            {
                                text.gameObject.SetActive(false);
                            }
                        }
                    }

                    if (tp == null || (tp.inHealRoom || !hit.transform.CompareTag("Selectable")))
                    {
                        if (newSelect != null)
                        {
                            newSelect.enabled = true;
                        }

                        if (newText != null)
                        {
                            newText.gameObject.SetActive(true);

                            //Makes text face the camera
                            newText.transform.LookAt(Camera.main.transform);
                            newText.transform.Rotate(0, 180, 0);
                        }
                    }
                    if (tp == null || (tp.inHealRoom && (hit.transform.CompareTag("SelectableHeal") || hit.transform.CompareTag("SelectablePower") || hit.transform.CompareTag("SelectableSpawn"))))
                    {
                        if (newSelect != null)
                        {
                            newSelect.enabled = false;
                        }

                        if (newText != null)
                        {
                            newText.gameObject.SetActive(false);

                            //Makes text face the camera
                            newText.transform.LookAt(Camera.main.transform);
                            newText.transform.Rotate(0, 180, 0);
                        }
                    }

                    if (text != null && newText != text)
                    {
                        text.gameObject.SetActive(false);
                        text = null;  // Clear previous text reference
                    }

                    selection = newSelect;
                    text = newText;
                }

                else
                {
                    if (selection != null)
                    {
                        selection.enabled = false;
                        selection = null;
                    }

                    if (text != null)
                    {
                        text.gameObject.SetActive(false);
                        text = null;
                    }
                }
            }
            else
            {
                selection.enabled = false;
                if (text != null)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }
    }
}

