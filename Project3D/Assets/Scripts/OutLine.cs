using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine : MonoBehaviour
{
    Material outline;

    Renderer renderers;
    List<Material> materialList = new List<Material>();

    public void ShaderOn()
    {
        renderers = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }

    public void shaderOff()
    {
        Renderer renderer = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderer.sharedMaterials);
        materialList.Remove(outline);

        renderer.materials = materialList.ToArray();
    }

    void Start()
    {
        outline = new Material(Shader.Find("Custom/Outline"));
    }
}