using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardCard : MonoBehaviour
{
   public TextMeshProUGUI _name;
   public TextMeshProUGUI _score;

    public PlayerData playerData;
    public void Init(PlayerData player){
        playerData = player;

        _name.text = playerData.name;
        _score.text = playerData.score;
    }
}
