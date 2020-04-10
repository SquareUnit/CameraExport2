using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string ID;
    
    public List<Line> lines = new List<Line>();
    [System.Serializable]
    public class Line
    {
        public string type;
        public string speaker;
        public string text;
        
        public Line(string[] raw)
        {
            type = raw[1];
            speaker = raw[2];
            text = raw[3];
        }
    }

}
