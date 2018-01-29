using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(Shader.Find("Unlit/Texture"), "RenderType");
    }

    void Update()
    {
        Vector3 position = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.position = position;
    }
}