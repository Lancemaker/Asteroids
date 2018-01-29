using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Highscores : MonoBehaviour {
    string playerName;
    int score;
    public InputField field;
    public GameObject InputSystem;
    public GameObject HighscoreTable;
    public RectTransform cell;
    //highscores
    List<string> names = new List<string>();
    List<string> highScores = new List<string>();
    string rawNames;
    string rawScores;

    private void Start()
    {
        rawNames = PlayerPrefs.GetString("names");
        rawScores = PlayerPrefs.GetString("highScores");
    }


    int StringToInt(string n)
    {
        try
        {
            int number = System.Int32.Parse(n);
            return number;
        }
        catch (System.FormatException e)
        {
            Debug.Log(e.Message);
        }
        return 0;
    }

    public void SetScore(int s)
    {
        score = s;
    }

    public void setname()
    {
        playerName = field.text;
        HighscoreTable.SetActive(true);
        InputSystem.SetActive(false);
        Save();
    }

    List<string> StringToList(string s) {
        List<string> list = new List<string>();
        string st = s;
        list = st.Split(',').ToList();
        return list;
    }

    string ListToString(List<string> list)
    {
        string s="";
        foreach (string current in list)
        {
            if (s == "")
            {
                s = current;
            }
            else
            {
                s += "," + current;
            }            
        }
        return s;
    }


    void testList(List<string> list) {
        foreach (string s in list)
        {
            Debug.Log(s);
        }
    }

    
    void Save()
    {
        if(rawNames == ""&& rawScores == "")
        {
            PlayerPrefs.SetString("names",playerName);
            PlayerPrefs.SetString("highScores",score.ToString());
            cell.GetComponent<Text>().text = playerName + " " + score;
            PlayerPrefs.Save();
            Debug.Log("saved");
            
        }
        else
        {
            Debug.Log("current:");
            Debug.Log(rawNames + " " + rawScores);
            //populating the lists
            names = StringToList(rawNames);
            highScores = StringToList(rawScores);
            //comparing the new score with the old ones and inserting it in the proper location
            for (int i = 0; i < highScores.Count; i++)
            {
                if (StringToInt(highScores[i])<score || i+1==highScores.Count)
                {
                    highScores.Insert(i, score.ToString());
                    names.Insert(i, playerName);
                    break;
                }
                if (i>=9)
                {
                    break;
                }
            }
            //packing the lists into strings and stuffing into the playerprefs.
            string finalNames = ListToString(names);
            string finalScores = ListToString(highScores);
            ShowHighscores(names,highScores);
            PlayerPrefs.SetString("names", finalNames);
            PlayerPrefs.SetString("highScores", finalScores);
            PlayerPrefs.Save();
            Debug.Log("saved");
        }
    }
    
    void ShowHighscores(List<string> n, List<string> hs)
    {
        Text cellText = cell.GetComponent<Text>();
        for (int i = 0; i < n.Count; i++)
        {            
            cellText.text += n[i] +" "+ hs[i]+ "\r\n";
        }
        
    }

}
