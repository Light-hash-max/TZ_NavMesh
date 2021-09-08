using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfWinners : MonoBehaviour
{
    public Text[] winners;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < winners.Length; i++)
        {
            winners[i].text = PlayerPrefs.GetString(i.ToString());
        }
    }
}
