using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blot : MonoBehaviour
{
    private Vector3 rayOffSet = new Vector3(0, 0.01f, 0);
    Transform tr;
    public Vector2 startSize, endSize;
    public float maxDistance = 2;

    float currentDistance;
    float distanceRatio;
    Vector3 avatarPosition;
    private Renderer rend;

    public bool active = true;

    void Start()
    {
        tr = transform;
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        rend.enabled = !GameManager.instance.currentAvatar.onGround;

        if (active)
        {
            avatarPosition = GameManager.instance.currentAvatar.tr.position;
            currentDistance = Vector3.Distance(avatarPosition, tr.position);

            if (currentDistance >= maxDistance)
                distanceRatio = 1;
            else
                distanceRatio = currentDistance / maxDistance;
            tr.localScale = Vector2.Lerp(startSize, endSize, distanceRatio);
        }

    }

    void LateUpdate()
    {
        tr.position = GameManager.instance.currentAvatar.hit.point + rayOffSet;
    }
}
