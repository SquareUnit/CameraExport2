using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public RandomObject[] randomizerArray;

    private int chanceSum;
    private int numberPicked;
    [HideInInspector] public string namePicked;

    private void Start()
    {
        SortRandomizerArray();
        chanceSum = 0;

        for (int i = 0; i < randomizerArray.Length; i++)
        {
            //Enleve les anims avec 0 weight
            if (randomizerArray[i].weight > 0)
            {
                //Assigne la valeur de chance qui sera utilise lors du tirage
                randomizerArray[i].chance = randomizerArray[i].weight + chanceSum;
                chanceSum = randomizerArray[i].chance;
            }
            else
                randomizerArray[i].chance = 0;
        }
    }

    /// <summary> Pige un objet au hasard avec le weight, sans avoir le meme objet 2 fois de suite. </summary>
    public string Pick()
    {
        numberPicked = Random.Range(1, chanceSum);

        for (int i = 0; i < randomizerArray.Length; i++)
        {
            if (numberPicked <= randomizerArray[i].chance)
            {
                namePicked = randomizerArray[i].name;
                return namePicked;
            }
        }
        return null;
    }

    /// <summary> Sort le tableau de la plus petite valeur de weight a la plus grande </summary>
    public void SortRandomizerArray()
    {
        int position;
        RandomObject temp;

        for (int i = 0; i < randomizerArray.Length - 1; i++)
        {
            position = i;

            for (int j = i + 1; j < randomizerArray.Length; j++)
            {
                if (randomizerArray[j].weight < randomizerArray[position].weight)
                    position = j;
            }

            //Swap?
            if (position != i)
            {
                temp = randomizerArray[position];
                randomizerArray[position] = randomizerArray[i];
                randomizerArray[i] = temp;
            }
        }
    }

}

[System.Serializable]
public class RandomObject
{
    [Tooltip("Nom exact du parametre")]
    public string name;
    public int weight; //Weight assigned by the user
    [HideInInspector] public int chance; // Chance value to applied internally 
}
