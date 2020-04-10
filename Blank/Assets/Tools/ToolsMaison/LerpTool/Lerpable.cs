using UnityEngine;
/// <summary>
/// Manages different types of lerping functions.
/// Tool for level design.
/// 
/// Keep it clean and simple!
/// Maxime Phaneuf
/// April 2019
/// </summary>
public class Lerpable : MonoBehaviour
{
    protected bool isLerping;
    float result;
    Mathl.LerpCurve lerp;
    float currentTime;
    public bool active;

    public void Init(Mathl.LerpCurve lerp)
    {
        this.lerp = lerp;
    }

    public void Lerp()
    {
        if (isLerping)
        {
            //Increment time since last frame.
            if (active)
                AddDeltaTime();
            //Check if lerp is over.
            CheckDuration();
            //Lerp for this frame.
            UpdateLerp();
        }
    }

    #region Private functions
    void UpdateLerp()
    {
        if (isLerping)
            Lerping();
    }

    public void AddDeltaTime()
    {
        currentTime += Time.deltaTime;
    }

    void CheckDuration()
    {
        if (currentTime >= lerp.duration)
            StopLerp();
    }

    void Lerping()
    {
        result = Mathl.Lerp(lerp, TimeOverDuration());
    }

    float TimeOverDuration()
    {
        return currentTime / lerp.duration;
    }
    #endregion

    /// <summary>
    /// Start lerp at current time.
    /// </summary>
    public void StartLerp()
    {
        currentTime = 0;
        isLerping = true;
    }

    /// <summary>
    /// Stop current lerp.
    /// </summary>
    public void StopLerp()
    {
        result = 0;
        isLerping = false;
    }

    public bool IsLerping()
    {
        return isLerping;
    }

    public void IsLerping(bool isLerping)
    {
        this.isLerping = isLerping;
    }

    /// <summary>
    /// Uses this lerpable result to lerp between two vectors. 
    /// </summary>
    public Vector3 Lerp(Vector3 start, Vector3 end)
    {
        return Vector3.Lerp(start, end, result);
    }

    /// <summary>
    /// Uses this lerpable result to lerp between two colors. 
    /// </summary>
    public Color Lerp(Color start, Color end)
    {
        return Color.Lerp(start, end, result);
    }

    /// <summary>
    /// Uses this lerpable result to lerp between two float values. 
    /// </summary>
    public float Lerp(float start, float end)
    {
        return Mathf.Lerp(start, end, result);
    }

    /// <summary>
    /// Uses this lerpable result to lerp on 3 points for a quadratic bezier curve.
    /// </summary>
    public Vector3 Lerp(Vector3 start, Vector3 mid, Vector3 end)
    {
        return Mathl.QuadraticBezierPoint(start, mid, end, result);
    }
}

