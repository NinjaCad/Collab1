using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    Vector3 screenPos;
    Vector3 worldPos;
    Vector3 mousePos;
    Vector2 mousePos2D;
    Vector3 dir;
    float angle;
    
    void Update()
    {
        screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + 1;
        worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        mousePos = new Vector3((Mathf.Round(worldPos.x)), (Mathf.Round(worldPos.y)), 0);
        
        dir = mousePos - this.transform.position;
        angle = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        transform.position = transform.parent.position;
        transform.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
    }
}
