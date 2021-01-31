using UnityEngine;

public class PersistantData : MonoBehaviour
{
    public bool brando;
    private static PersistantData _instance;

    public static PersistantData Instance { get { return _instance; } }


    private void Awake()
    {
        brando = true;
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void Choose(bool choseBrando)
    {
        brando = choseBrando;
        FindObjectOfType<mainmenu>().PlayGame();
    }
}
