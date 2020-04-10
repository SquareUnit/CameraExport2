using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;

public class ParseXML : MonoBehaviour
{
    //https://www.youtube.com/watch?v=JMn0tTt4DO0
    // Start is called before the first frame update
    void Start()
    {
        //retirer les /* */ pour voir un resultat dans debug
        
        //Detecte le dictionnaire choisi
        List<Dictionary<string, string>> allTextDic = parseFile();
        //Choisis la case a afficher
        Dictionary<string, string> dic = allTextDic[0];
        //choisis les elements qui constituent la case
        Debug.Log(dic["interlocuteur"]+ dic["dialogue"]); // permet de mettre les phrases sur la même ligne quand il peut
       // Debug.Log(dic["lineTwo"]);
    }

    public List<Dictionary<string, string>> parseFile()
    {
        //determine le nom du fichier Xml a loader
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("narration");
        //permet d'aller chercher des sous-titres dans le doc en les stockant dans une variable
        var doc = XDocument.Parse(txtXmlAsset.text);
        //Determine tous les dictionaires, soit le header principal
        var allDict = doc.Element("narration").Elements("chapitre");
        //permet de stocker les strings du dictionnaires dans une variable
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        //A chaque loop, il va faire ce qui suit
        foreach (var oneDict in allDict)
        {
            //Determine dans la variable twoStrings qu'il contiendra des elements du header conversation
            var personnage = oneDict.Elements("interlo");
            var paroles = oneDict.Elements("blabla");
            //determine le premier element du header
            XElement element1 = personnage.ElementAt(0);
            //determine le second
            XElement element2 = paroles.ElementAt(0);
            //on s'assure de remplacer les headers  par des vides pour eviter de les faire s'afficher en texte inutilement
            string first = element1.ToString().Replace("<interlo>", "").Replace("</interlo>", "");
            string second = element2.ToString().Replace("<blabla>", "").Replace("</blabla>", "");
            //Permettra l'utilisation de tag lineOne et lineTwo pour parler des elements respectifs
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("interlocuteur", first);
            dic.Add("dialogue", second);
            //s'assure d'afficher ce qu'on a stocke dans dic, soit lineOne et line Two
            allTextDic.Add(dic);
        }
        //affichera tout ce qui a ete stocke dans allTextDic, soit lineOne et lineTwo dans ce cas-ci
        return allTextDic;
    }
}
