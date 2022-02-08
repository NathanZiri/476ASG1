using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public  GameObject[] players;
    // Start is called before the first frame update
    public GameObject Tagger;
    public GameObject[] Hiders;
    void Start()
    {
        Hiders = new GameObject[players.Length - 1];
        AssignRoles();
    }

    void AssignRoles(){
        int selectPlayer = Random.Range(0, players.Length - 1);
        int assign = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (i == selectPlayer)
            {
                players[i].GetComponent<FreezeTagBehaviour>().SetTagger();
                Tagger = players[i];
            }
            else
            {
                players[i].GetComponent<FreezeTagBehaviour>().SetFleer();
                Hiders[assign] = players[i];
                assign++;
            }
            
        }
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<FreezeTagBehaviour>().assignPlayers(Tagger, Hiders);
        }
    }

    public void ReAssign(string newTagger)
    {
        GameObject temp;
        for (int i = 0; i < Hiders.Length; i++)
        {
            if (Hiders[i].name.Equals(newTagger))
            {
                temp = Hiders[i];
                Hiders[i] = Tagger;
                Tagger = temp;
            }
        }
        
        Tagger.GetComponent<FreezeTagBehaviour>().SetTagger();
        for (int i = 0; i < Hiders.Length; i++)
        {
            Hiders[i].GetComponent<FreezeTagBehaviour>().SetFleer();
        }
    }
}
