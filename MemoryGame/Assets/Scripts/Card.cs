using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int id;
    [SerializeField]
    private GameObject front;
    [SerializeField]
    private GameObject back;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardAction()
    {
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>().Game(id);
    }

    public void ShowFace()
    {
        back.SetActive(false);
        front.SetActive(true);
    }

    public void HideFace()
    {
        back.SetActive(true);
        front.SetActive(false);
    }

    public bool IsShowing()
    {
        if (back.activeSelf == false && front.activeSelf == true)
            return true;
        else
            return false;
    }

    public void Init(int name)
    {
        id = name;
    }
}
