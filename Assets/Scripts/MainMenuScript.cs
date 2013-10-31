using UnityEngine;


public class MainMenuScript : MonoBehaviour
{
  private const int MaxLengthOfName = 15;

  public GUIStyle   welcomeLabel;
  public GUISkin    customSkin;
  public Rect       mainMenuRect;
  public Rect       playGameRect;
  public Rect       resumeGameRect;
  public Rect       optionsRect;
  public Rect       quitRect;
  //public Rect       Messager;

  public string     FirstPlayer = "First Player";
  public string     SecondPlayer = "Second Player";
  private int       _sideOfField = 3;

  private bool      _optionsMode = false;
  private bool      _menuMode = true;
  private bool      _gameMode = false;
  private string    _message = "";

  public int SideOfField
  {
    get { return _sideOfField; }
    set { _sideOfField = value; }
  }


  private void Awake()
  {
    DontDestroyOnLoad(this);
    GameField.OnMessager += OnMessager;
    GameField.GameOver += SetMenuOn;
  }

  private void OnDestroy()
  {
    GameField.OnMessager -= OnMessager;
    GameField.GameOver -= SetMenuOn;
  }

  private void OnMessager(string message)
  {
    Debug.Log(message);
    _message = message;
    
  }

  private void Start()
  {
    OnMessager("Tic-Tac-Toe");
  }

  private void SetMenuOn(bool state)
  {
    _menuMode = state;
    _optionsMode = false;
  }

  private void OnGUI()
  {
    GUI.Label(new Rect(Screen.width / 2 - 50, 0, 100, 20), _message, welcomeLabel);

    
    if (Input.GetKey(KeyCode.Escape))
    {
      _menuMode = true;
      _optionsMode = false;
      Time.timeScale = 0;
    }
    if(_gameMode)
    {
      
     if (GUI.Button(mainMenuRect, "Menu"))
     {
       if(!_menuMode)
       {
        Debug.Log("Menu pressed on");
        _menuMode = true;
        _optionsMode = false;
       Time.timeScale = 0;
       }
       else
         {
          Debug.Log("Menu pressed off");
          _menuMode = false;
          _optionsMode = false;
          Time.timeScale = 1;
         }
     }
     
     
    }

    if (_menuMode)
    {
      if (!_optionsMode)
      {

        
        GUI.skin = customSkin;
        if (GUI.Button(playGameRect, "New Game"))
        {
          _menuMode = false;
          _gameMode = true;
          Time.timeScale = 1;
          Application.LoadLevel("game");

        }
        if (!_gameMode)
        {

        }
        else
        {
          if (GUI.Button(resumeGameRect, "Exit to menu"))
          {

            _gameMode = false;

            Destroy(this);
            Application.LoadLevel(0);
          }
        }

        if (GUI.Button(optionsRect, "Options")&&(!_gameMode))
        {
          _optionsMode = true;
        }
        if (GUI.Button(quitRect, "Quit"))
        {
          Application.Quit();
        }
      }
      else
      {
        //GUI.Label(new Rect(Screen.width/2, 0, 100, 20), "Options", welcomeLabel);
        OnMessager("Options");
        GUI.skin = customSkin;

        GUI.Label(new Rect(100, 50, 200, 20), "Side of gamefield");
        SideOfField = (int) GUI.HorizontalSlider(new Rect(100, 75, 200, 20), SideOfField, 3, 7);
        GUI.Label(new Rect(305, 70, 50, 20), SideOfField.ToString());

        GUI.Label(new Rect(100, 100, 100, 20), "First player:");
        FirstPlayer = GUI.TextField(new Rect(210, 100, 100, 20), FirstPlayer, MaxLengthOfName);
        
        GUI.Label(new Rect(100, 125, 100, 20), "Second player:");
        SecondPlayer = GUI.TextField(new Rect(210, 125, 100, 20), SecondPlayer, MaxLengthOfName);

        if (GUI.Button(new Rect(20, 190, 100, 30), "OK"))
        {
          _optionsMode = false;
        }
      }
    }
  }
}
