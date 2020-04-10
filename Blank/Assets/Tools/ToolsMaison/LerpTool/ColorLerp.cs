using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    Lerpable lerpable;
    public Material start, end;
    public Mathl.LerpCurve curve;
    public bool isLooping = true;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        lerpable = GetComponent<Lerpable>();
        mr = GetComponent<MeshRenderer>();
        lerpable.Init(curve);
        lerpable.StartLerp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lerpable.IsLerping() && isLooping)
        {
            Color c = start.color;
            start.color = end.color;
            end.color = c;
            lerpable.StartLerp();
        }
        else
            mr.material.color = lerpable.Lerp(start.color, end.color);
        
    }
}
