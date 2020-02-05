using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour
{
    [SerializeField] private Text value;

    public void ChangeStat(int stat)
    {
        value.text = stat.ToString();
    }
}
