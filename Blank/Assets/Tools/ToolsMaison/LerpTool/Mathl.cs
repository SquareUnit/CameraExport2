using UnityEngine;
/// <summary>
/// Contains lerp functions with different types of curves.
/// Uses Lerp function throught LerpCurve objet.
/// 
/// Keep it clean and simple!
/// Maxime Phaneuf
/// April 2019
/// </summary>
public static class Mathl
{
    public enum Curve { Linear,
            SmoothStart, SmoothStart2, SmoothStart3, SmoothStart4, SmoothStart5,
            SmoothStop, SmoothStop2, SmoothStop3, SmoothStop4, SmoothStop5,
            SmoothStep, SmoothStep2, SmoothStep3, SmoothStep4, SmoothStep5,
            Sinusoid }
    public const float MIN_DURATION = .1f;
    public const float MAX_DURATION = 300;
    
    [System.Serializable]
    public class LerpCurve
    {
        public Curve curve;
        [Range(MIN_DURATION, MAX_DURATION)]public float duration = 1;
        /// <summary>
        /// Representation of a lerp curve with attributes.
        /// </summary>
        public LerpCurve(Curve curve, float duration)
        {
            this.curve = curve;
            this.duration = duration;
        }
    }

    /// <summary>
    /// Returns value of lerp curve with given t.
    /// </summary>
    public static float Lerp(LerpCurve lc, float t)
    {
        if (lc.curve >= Curve.SmoothStart && lc.curve <= Curve.SmoothStart5)
            return Lerp(SmoothStart(lc, t));
        else if (lc.curve >= Curve.SmoothStop && lc.curve <= Curve.SmoothStop5)
            return Lerp(SmoothStop(lc, t));
        else if (lc.curve >= Curve.SmoothStep && lc.curve <= Curve.SmoothStep5)
            return Lerp(SmoothStep(lc, t));
        else if(lc.curve == Curve.Sinusoid)
            return Lerp(Sinusoid(t));
        return Lerp(t);
    }
        
    static float Lerp(float t)
    {
        return Mathf.Lerp(0, 1, t);
    }

    #region SmoothStart
    static float SmoothStart(LerpCurve lc, float t)
    {
        if (lc.curve == Curve.SmoothStart2)
            return SmoothStart2(t);
        else if (lc.curve == Curve.SmoothStart3)
            return SmoothStart3(t);
        else if (lc.curve == Curve.SmoothStart4)
            return SmoothStart4(t);
        else if (lc.curve == Curve.SmoothStart5)
            return SmoothStart5(t);
        return t;
    }

    static float SmoothStart2(float t)
    {
        return t * t;
    }

    static float SmoothStart3(float t)
    {
        return t * t * t;
    }

    static float SmoothStart4(float t)
    {
        return t * t * t * t;
    }

    static float SmoothStart5(float t)
    {
        return t * t * t * t * t;
    }
    #endregion

    #region SmoothStop
    static float SmoothStop(LerpCurve lc, float t)
    {
        if (lc.curve == Curve.SmoothStop2)
            return SmoothStop2(t);
        else if (lc.curve == Curve.SmoothStop3)
            return SmoothStop3(t);
        else if (lc.curve == Curve.SmoothStop4)
            return SmoothStop4(t);
        else if (lc.curve == Curve.SmoothStop5)
            return SmoothStop5(t);
        return t;
    }

    static float SmoothStop2(float t)
    {
        return 1 - ((1 - t) * (1 - t));
    }

    static float SmoothStop3(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t));
    }

    static float SmoothStop4(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t));
    }

    static float SmoothStop5(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t) * (1 - t));
    }
    #endregion

    #region SmoothStep
    static float SmoothStep(LerpCurve lc, float t)
    {
        if (lc.curve == Curve.SmoothStep2)
            return SmoothStep2(t);
        else if (lc.curve == Curve.SmoothStep3)
            return SmoothStep3(t);
        else if (lc.curve == Curve.SmoothStop4)
            return SmoothStep4(t);
        else if (lc.curve == Curve.SmoothStop5)
            return SmoothStep5(t);
        return Mathf.SmoothStep(0, 1, t); 
    }

    static float SmoothStep2(float t)
    {
        return Mathf.SmoothStep(0, 1, t * t);
    }

    static float SmoothStep3(float t)
    {
        return Mathf.SmoothStep(0, 1, t * t * t);
    }

    static float SmoothStep4(float t)
    {
        return Mathf.SmoothStep(0, 1, t * t * t * t);
    }

    static float SmoothStep5(float t)
    {
        return Mathf.SmoothStep(0, 1, t * t * t * t * t);
    }
    #endregion

    static float Sinusoid(float t)
    {
        float a = (Mathf.PI / 2) * t ;
        float s = Mathf.Sin(a);
        return s + 1 / 2;
    }

    public static Vector3 QuadraticBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p1;
        p += 2 * u * t * p2;
        p += tt * p3;
        return p;
    }
}
