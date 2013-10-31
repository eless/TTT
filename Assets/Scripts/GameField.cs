using System;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;

public class GameField : MonoBehaviour
{

  public delegate void  OnMessagerWriter(string message);
  public static   event OnMessagerWriter OnMessager;

  public static   event Action<bool> GameOver; 
 
  public    GameObject      Btn;
  public    GameObject      Surface;
  public    char[,]         Field { get; set; }
  public    Camera          camera;

  //private   int        numbOfBut;


  private         string  _firstPlayerName;
  private         string  _secondPlayerName;

  private         int     _countOfSteps;
  private         int     _sideOfField = 9;
  
  private void Awake()
  {
    Button.ButtonPress += ButtonPressed;
  }

  private void OnDestroy()
  {
    Button.ButtonPress -= ButtonPressed;
  }

  void Start()
  {
    _sideOfField = GameObject.Find("MainMenu").GetComponent<MainMenuScript>().SideOfField;
    _firstPlayerName = GameObject.Find("MainMenu").GetComponent<MainMenuScript>().FirstPlayer;
    _secondPlayerName = GameObject.Find("MainMenu").GetComponent<MainMenuScript>().SecondPlayer;
    Debug.Log("Start new game");
    OnMessager("Run: " + _firstPlayerName);
    
    _countOfSteps = 0;

    //Construct the game field and array SideOfField x SideOfField
    camera.transform.localPosition = new Vector3((_sideOfField - 1) / 2.0f, _sideOfField * 1.3f, (_sideOfField - 1) / 2.0f + 0.1f);
    Surface = (GameObject)Instantiate(Surface, new Vector3(0, 0.5f, 0), Quaternion.identity);
    Surface.transform.localPosition += new Vector3((_sideOfField - 1) / 2.0f, 0, (_sideOfField - 1) / 2.0f);
    Surface.transform.localScale += new Vector3(_sideOfField, 0, _sideOfField);
    //Debug.Log("x,y,z:" + Surface.transform.localPosition.x + ", " + Surface.transform.localPosition.y + " ," + Surface.transform.localPosition.z);
   
    Field = new char[_sideOfField, _sideOfField];
    for (int i = 0; i < _sideOfField; i++)
      for (int j = 0; j < _sideOfField; j++)
      {
        //Construct buttons
        Btn = (GameObject) Instantiate(Btn, new Vector3(i, 1.0f, j), Quaternion.identity);
        
        // Initialize gamefield to space-char " "
        Field.SetValue(' ', i, j);
      }
  }

  private string Winner(char sign)
  {
    if (sign == 'X')
    {
      return _firstPlayerName;
    }
    else return _secondPlayerName;
  }

  private void ButtonPressed(int x, int y)
  {
    _countOfSteps++;
    char sign;
    if ((_countOfSteps % 2) != 0)
    {
      sign = 'X';
      OnMessager("Run: " + _secondPlayerName);
    }
    else
    {
      sign = 'O';
      OnMessager("Run: " + _firstPlayerName);
    }
    Field.SetValue(sign, x, y);

    if (StepCheck(x, y, sign))
    {
      OnMessager("Winner: " + Winner(sign));
      GameOver(true);
    }
    else if (_countOfSteps == _sideOfField * _sideOfField)
    {
      OnMessager("It's dead heat! :)");
      GameOver(true);
    }
  }
 
  private bool StepCheck(int n, int m, char sign)
  {
    int coordN = 0;
    int coordM = 0;
    int coordNM = 0;
    int coordMN = 0;

    for (int i = 0; i < 5; i++)
    {
      int iterN = n - 2 + i;
      int iterM = m - 2 + i;

      if ((iterN < _sideOfField) && (iterN >= 0))
      {

        if (((char)Field.GetValue(iterN, m)) == sign)
        {
          //Debug.Log("-Sign:" + sign + " X:" + (iterM) + " Y:" + (iterN));
          if (coordN == 2) return true;
          else
            coordN++;
        }
        else coordN = 0;
      }

      if ((iterM < _sideOfField) && (iterM >= 0))
      {
        if (((char)Field.GetValue(n, iterM)) == sign)
        {
          //Debug.Log("|Sign:" + sign + " X:" + (iterM) + " Y:" + (iterN));
          if (coordM == 2) return true;
          else
            coordM++;
        }
        else coordM = 0;
      }

      if ((iterM < _sideOfField) && (iterM >= 0) && (iterN < _sideOfField) && (iterN >= 0))
      {

        if (((char)Field.GetValue(iterN, iterM)) == sign)
        {
          //Debug.Log("/Sign:" + sign + " X:" + (iterM) + " Y:" + (iterN));
          if (coordNM == 2) return true;
          else
            coordNM++;
        }
        else coordNM = 0;
      }
      iterN = n + 2 - i;
      if ((iterM < _sideOfField) && (iterM >= 0) && (iterN < _sideOfField) && (iterN >= 0))
      {
        if (((char)Field.GetValue(iterN, iterM)) == sign)
        {
          //Debug.Log("\\Sign:" + sign + " X:" + (iterM) + " Y:" + (iterN));
          if (coordMN == 2) return true;
          else
            coordMN++;
        }
        else coordMN = 0;

      }
    }
    return false;
  }

}
