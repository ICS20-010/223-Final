using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitTrigger : MonoBehaviour
{
  [SerializeField] private GameObject exitPanel;
  [SerializeField] private Light exitLight;
  [SerializeField] private Material openMaterial;
  public bool exitLocked = true;
  private static int exitNum = 1;
  public int exitId = 0;

  // Start is called before the first frame update
  void Awake()
  {
    Messenger.AddListener(GameEvents.EXIT_OPENED, exitOpen);

    exitId = exitNum;
    exitNum += 1;
  }

  public void resetExitCount()
  {
    exitNum = 1;
  }

  void exitOpen()
  {
    Renderer renderer = exitPanel.GetComponent<Renderer>();
    exitLight.color = new Color(0.2078431f, 0.5921569f, 0.7490196f, 1);
    renderer.material = openMaterial;
    exitLocked = false;
  }

  public void setLocked(bool isExitLocked)
  {
    if (!isExitLocked)
    {
      exitOpen();
    }
  }

  // Update is called once per frame
  private void OnDestroy()
  {
    Messenger.RemoveListener(GameEvents.EXIT_OPENED, exitOpen);
  }
}
