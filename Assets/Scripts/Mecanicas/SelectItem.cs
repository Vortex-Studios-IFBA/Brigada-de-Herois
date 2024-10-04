using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectItem : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;
    // Start is called before the first frame update
    Transform _selection;
    // Update is called once per frame
    void Update()
    {
        if(_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            if(!_selection.CompareTag("Projetor") && !_selection.CompareTag("PC"))
            {
                        var selectionKey = _selection.GetComponent<KeyGet>();
                selectionKey.showName.gameObject.SetActive(false);
            }
            
            selectionRenderer.material = defaultMaterial;
            
            _selection = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            var selection = hit.transform;
            if(selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                var selectionKey = selection.GetComponent<KeyGet>();
                if(selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                    selectionKey.showName.gameObject.SetActive(true);
                }
                if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    if(selectionKey.doorName != null)
                    {
                        print("Pegou chave "+ selectionKey.doorName);
                        GameObject.Find(selectionKey.doorName).GetComponent<target>().locked = false;
                        selectionKey.DestroySelf();
                        
                    }
                }
                _selection = selection;
            }
            if(selection.CompareTag("Projetor"))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                var selectionSlide = selection.GetComponent<DataShowController>();
                if(selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }
                if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    selectionSlide.video.Play();
                }
                _selection = selection;
            }
            if(selection.CompareTag("PC"))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightMaterial;
                }
                if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                }
                _selection = selection;
            }
        }
    }
}
