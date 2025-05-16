using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle_color : MonoBehaviour
{
    public OutlineOnView _rayManager;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private GameObject[] bottles;

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("bottle");
        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(renderer.material); // インスタンス化
                originalColors[bottle] = renderer.material.GetColor("_BaseColor");
            }
        }
    }

    void Update()
    {
        GameObject hit = _rayManager.hitObject;

        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color targetColor = (hit == null || bottle == hit)
                    ? originalColors[bottle]
                    : Color.gray;

                renderer.material.SetColor("_BaseColor", targetColor);
            }
        }
    }
}
