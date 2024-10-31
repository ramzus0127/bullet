using UnityEngine;

[ExecuteAlways]
public class TileSnap : MonoBehaviour
{
    void Update()
    {
        if (Application.isPlaying)
        {
			enabled = false;
			return;
        }

        if (Physics.Raycast(transform.position + Vector3.up * 20, Vector3.down, out var hitInfo))
        { 
            transform.position = hitInfo.collider.transform.position;
        }
    }
}
