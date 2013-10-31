using UnityEngine;

public class Button : MonoBehaviour
{

  public  delegate void   ButtonPressed(int x, int y);
  public  static   event  ButtonPressed ButtonPress;

  private bool            _state;
  private static   int    _signOfButton;
  
  void Start ()
	{
   _signOfButton = 0;
   _state = false;
	}

  private void Awake()
  {
    GameField.GameOver += ButtonState;
  }

  private void OnDestroy()
  {
    GameField.GameOver -= ButtonState;
  }

  public void ButtonState(bool state)
  {
    _state = state;
  }

  void OnMouseUp()
  {
    if (!_state)
    {
      //Debug.Log("Button pressed");
      _signOfButton++;
      
      transform.localPosition -= new Vector3(0, 0.2f, 0);
      var x = (int)transform.localPosition.x;
      var y = (int) transform.localPosition.z;
      ButtonPress(x, y);
      if ((_signOfButton%2) != 0)
      {
        renderer.material.color = new Color(0, 1f, 0);
      }
      else
      {
        renderer.material.color = new Color(1f, 0, 0);
      }
      _state = !_state;
    }
  }
}
