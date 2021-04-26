using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagName : MonoBehaviour
{

    private Transform mainCamera;

    public TextMeshPro textMeshCharName;
    public string charName;


    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main.transform;
        textMeshCharName = gameObject.GetComponentInChildren<TextMeshPro>();
        textMeshCharName.text = charName;

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }
}
