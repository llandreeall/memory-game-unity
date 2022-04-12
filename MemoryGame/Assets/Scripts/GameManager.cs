using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public class Pair
    {
        public int count;
        public Sprite sp;

        public Pair(int c, Sprite s)
        {
            count = c;
            sp = s;
        }
    }

    [SerializeField]
    public int noOfCards;
    public Pair[] spriteCounts;

    [SerializeField]
    Transform board;

    [SerializeField]
    GameObject card;

    [SerializeField]
    Sprite background;

    [SerializeField]
    List<Sprite> fronts = new List<Sprite>();

    [SerializeField]
    Sprite[] puzzles;

    [SerializeField]
    Text score;

    public List<GameObject> deck = new List<GameObject>();

    bool first = false, second = false;
    string firstName, secondName;
    int firstID, secondID;
    int points = 0;
    int tries;
   
    private void Awake()
    {
        Pair[] aux = new Pair[noOfCards / 2];
        board = GameObject.Find("Board").transform;
        puzzles = Resources.LoadAll<Sprite>("Sprites/PaperCards/AllCards");
        string usedNumbers = "-";
        for (int i = 0; i < noOfCards / 2; i++)
        {
            int random = Random.Range(0, puzzles.Length);
            while(usedNumbers.Contains("-" + random.ToString() + "-"))
            {
                random = Random.Range(0, puzzles.Length);
            }
            usedNumbers += random.ToString() + "-";
            aux[i] = new Pair(0,puzzles[random]);
        }
        spriteCounts = aux;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < noOfCards; i++)
        {
            GameObject newCard = Instantiate(card);
            newCard.name = "" + i;
            newCard.transform.SetParent(board, false);
            int random;
            random = Random.Range(0, spriteCounts.Length);
            while (spriteCounts[random].count == 2)
            {
                random = Random.Range(0, spriteCounts.Length);
            }
            spriteCounts[random].count++;
            //newCard.transform.GetChild(0).GetComponent<Image>().sprite = spriteCounts[random].sp;
            fronts.Add(spriteCounts[random].sp);
            newCard.transform.GetChild(1).GetComponent<Image>().sprite = background;
            newCard.transform.GetChild(0).gameObject.SetActive(false);
            newCard.GetComponent<Card>().Init(i);
            deck.Add(newCard);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCardsFromScene()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Card");
        for(int i = 0; i < list.Length; i++)
        {
            deck.Add(list[i]);
        }
    }

    public List<GameObject> GetCardList()
    {
        return deck;
    }

    private IEnumerator WrongTry()
    {
        yield return new WaitForSeconds(2.0f);
        deck[firstID].GetComponent<Card>().HideFace();
        deck[secondID].GetComponent<Card>().HideFace();
        first = false;
        second = false;
        firstID = -1;
        secondID = -1;
        firstName = "-";
        secondName = "-";
    }

    private IEnumerator GoodTry()
    {
        yield return new WaitForSeconds(2.0f);
    }

    public void Game(int id)
    {
        
        if(!first && deck[id].GetComponent<Card>().IsShowing() == false)
        {
            first = true;
            firstID = id;
            deck[firstID].GetComponent<Card>().ShowFace();
            deck[firstID].transform.GetChild(0).GetComponent<Image>().sprite = fronts[firstID];
            firstName = fronts[firstID].name;
        } else
        if (!second && deck[id].GetComponent<Card>().IsShowing() == false)
        {
            second = true;
            secondID = id;
            deck[secondID].GetComponent<Card>().ShowFace();
            deck[secondID].transform.GetChild(0).GetComponent<Image>().sprite = fronts[secondID];
            secondName = fronts[secondID].name;
            
            if(firstName == secondName)
            {
                
                StartCoroutine(GoodTry());
                first = false;
                second = false;
                firstID = -1;
                secondID = -1;
                firstName = "-";
                secondName = "-";
                points++;
                score.text = "" + points;
            } else
            {
                
                StartCoroutine(WrongTry());
                
            }
        }
    }

    public void Restart()
    {
        fronts = new List<Sprite>();
        Pair[] aux = new Pair[noOfCards / 2];
        string usedNumbers = "-";
        for (int i = 0; i < noOfCards / 2; i++)
        {
            int random = Random.Range(0, puzzles.Length);
            while (usedNumbers.Contains("-" + random.ToString() + "-"))
            {
                random = Random.Range(0, puzzles.Length);
            }
            usedNumbers += random.ToString() + "-";
            aux[i] = new Pair(0, puzzles[random]);
        }
        spriteCounts = aux;

        for (int i = 0; i < noOfCards; i++)
        {
            GameObject newCard = deck[i];
            newCard.GetComponent<Card>().HideFace();
            int random;
            random = Random.Range(0, spriteCounts.Length);
            while (spriteCounts[random].count == 2)
            {
                random = Random.Range(0, spriteCounts.Length);
            }
            spriteCounts[random].count++; 
            fronts.Add(spriteCounts[random].sp);
            
            first = false;
            second = false;
            firstID = -1;
            secondID = -1;
            firstName = "-";
            secondName = "-";
            points=0;
            score.text = "" + points;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

}
