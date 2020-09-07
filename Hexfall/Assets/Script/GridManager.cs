using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GridManager : MonoBehaviour
{
    public enum GameState { DESTROY, SPAWNING, GAMING }
    public GameState state = GameState.DESTROY;

    public int rows, cols;
    float xOffSet = 0.609f, yOffSet = 0.35f;
    float pointOffSet = 0.381f;

    public GameObject gameOverMenu;
    public bool isNextBomb;
    [SerializeField]
    public GameObject[,] allHex;


    void Awake()
    {
        state = GameState.DESTROY;
        allHex = new GameObject[rows, cols];

    }
    private void Start()
    {
        Time.timeScale=1;
        GenerateGrid();
        StartCoroutine(FindHexDelay(5f));
    }

    //////Oyun bitti mi 5 saniyede bir kontrol ediliyor
    IEnumerator FindHexDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindHex();
        }
    }

    private void FixedUpdate()
    {
        IsRefill();

    }
    //////ilk altıgenleri ve kontrol noktlarının üretildiği yer
    void GenerateGrid()
    {
        ////////altıgenler
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                float xPos = x;
                if (y % 2 == 1)
                {
                    xPos += xOffSet;
                }
                xPos += 0.22f * x;

                GameObject hex = ObjectGetter.hexagon.GetObje();
                hex.transform.position = new Vector2(xPos, y * yOffSet);
                hex.SetActive(true);
                hex.name = "Hex" + x + "_" + y;


                hex.GetComponent<Hexagon>().row = x;
                hex.GetComponent<Hexagon>().col = y;
                allHex[x, y] = hex;


            }


        }


        //////////kontrol noktaları

        int counter = 0, y1 = 0;
        float xPos2 = 0f;
        for (int x = 0; x < rows * 2 - 1; x++)
        {
            if (x % 2 == 0)
            {

                if (y1 % 2 == 1)
                {

                    xPos2 = 1.227f * counter;
                }
                else
                {
                    xPos2 = 1.223f * counter;
                }
                counter++;

            }

            for (int y = 0; y < cols * 2; y++)
            {
                float xPos = x;
                y1 = y;
                if (y % 2 == 1)
                {
                    xPos += xOffSet;
                }
                xPos += 0.22f * x;

                if (y >= 1 && y < cols - 1)
                {
                    GameObject point = (GameObject)Instantiate(Resources.Load("Point"));

                    xPos -= xPos2;


                    if (x % 2 == 1)
                    {


                        if (y % 2 == 1)
                        {
                            xPos -= 0.474f;
                            point.transform.position = new Vector2(xPos - pointOffSet, y * yOffSet);


                        }
                        else
                        {
                            xPos -= 0.753f;
                            point.transform.position = new Vector2(xPos + pointOffSet, y * yOffSet);

                        }
                    }
                    else
                    {
                        if (y % 2 == 1)
                        {
                            point.transform.position = new Vector2(xPos - pointOffSet, y * yOffSet);

                        }
                        else
                        {
                            point.transform.position = new Vector2(xPos + pointOffSet, y * yOffSet);

                        }
                    }


                    point.name = "Point " + x + "_" + y;
                    point.transform.parent = this.transform;
                }
            }
        }


    }


   
    ////////// Yok edilen altıgenler yerine, yenisinin üretildiği kod bloğu
    IEnumerator RefillGrid()
    {
        yield return new WaitForSeconds(.2f);
        if (state == GameState.SPAWNING)
        {
            for (int x = 0; x < rows; x++)
            {

                for (int y = 0; y < cols; y++)
                {
                    if(state == GameState.DESTROY){
                        yield break;
                    }
                    if (allHex[x, y] == null)
                    {
                        float xPos = x;
                        if (y % 2 == 1)
                        {
                            xPos += xOffSet;
                        }
                        xPos += 0.22f * x;
                        GameObject hex = ObjectGetter.hexagon.GetObje();
                        hex.transform.position = new Vector2(xPos, y * yOffSet + 3f);

                        if (isNextBomb)
                        {
                            hex.GetComponent<Hexagon>().isBomb = true;
                            isNextBomb = false;
                        }


                        hex.SetActive(true); ;
                        hex.name = "Hex" + x + "_" + y;
                        //    hex.transform.parent = this.transform;
                        hex.GetComponent<Hexagon>().row = x;
                        hex.GetComponent<Hexagon>().col = y;
                        allHex[x, y] = hex;


                    }
                    yield return new WaitForSeconds(.01f);
                }
                yield return new WaitForSeconds(.01f);
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator WaitRefill()
    {
        state = GameState.SPAWNING;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(RefillGrid());


    }
/////eğer ızgara doluysa oyun moduna geçilir yoksa altıgenlerin konumları güncellenir ve yeni altıgenler üretilir
    void IsRefill()
    {

        for (int x = 0; x < rows; x++)
        {

            for (int y = 0; y < cols; y++)
            {
                if (allHex[x, y] == null)
                {
                    StartCoroutine(DestroyHex());
                }
            }
        }
        state = GameState.GAMING;


    }

    ////// ızgaranın en yukarsında ki altıgenler yok edilmişse, yeni altıgen üretmek için fonksiyon çağırılır
    IEnumerator CheckGrid()
    {

        yield return new WaitForSeconds(.5f);

        for (int x = 0; x < rows; x++)
        {
            if (allHex[x, cols - 1] == null || allHex[x, cols - 2] == null)
            {
                StartCoroutine(WaitRefill());

            }

        }
        yield return new WaitForSeconds(.5f);

    }

    /////// yok edilen altıgenlerden sonra diğer altıgenlerin konumu güncellenir ve CheckGrid fonksiyonu çağırılır
    /////// matris içersinde boş konum bulursa üzerindeki altıgenlerin indeksini azaltır.
    public IEnumerator DestroyHex()
    {
        state = GameState.DESTROY;
        yield return new WaitForSeconds(.2f);
        int nullCount = 0;
        for (int x = 0; x < rows; x++)
        {

            for (int y = 0; y < cols; y += 2)
            {
                if (allHex[x, y] == null)
                {
                    nullCount += 2;
                    
                }
                else if (nullCount > 0)
                {
                    allHex[x, y].GetComponent<Hexagon>().col -= nullCount;
                    allHex[x, y] = null;
                    state = GameState.DESTROY;
                    
                }

            }
            nullCount = 0;
            for (int y = 1; y < cols; y += 2)
            {
                if (allHex[x, y] == null)
                {
                    nullCount += 2;

                }
                else if (nullCount > 0)
                {
                    allHex[x, y].GetComponent<Hexagon>().col -= nullCount;
                    allHex[x, y] = null;
                }

            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.5f);
        StartCoroutine(CheckGrid());


    }


///////// olası hamle kalıp kalmadığını kontrol ettiğim fonksiyon
////////// önce 2 tane yan yana aynı renk altıgenleri bulur. Sonra CheckGameOver fonksiyonunda konumu daha küçük olana göre patlama durumunu kontrol eder.
///////// eğer tüm altıgenler true değerini döndürürse olası hamle kalmamış demektir oyum biter
     void FindHex()
    {
        Hexagon[] allHexagon = GameObject.FindObjectsOfType<Hexagon>();

        for (int j = 0; j < allHexagon.Length - 1; j++)
        {

            for (int i = 0; i < allHexagon.Length - 1; i++)
            {
                float distance = (allHexagon[i].transform.position - allHexagon[j].transform.position).sqrMagnitude;
                if (distance < 1f)
                {
                    if (allHexagon[i].id == allHexagon[j].id)
                    {

                        bool isOver = CheckGameOver(allHexagon[i].gameObject, allHexagon[j].gameObject);

                        if (!isOver)
                        {

                            return;
                        }

                    }
                }

            }
        }
        Debug.Log("Oyun bitti");
        gameOverMenu.SetActive(true);
    }

    
    ///////// olası tüm hamleler kontrol edilir
    public bool CheckGameOver(GameObject hex1, GameObject hex2)
    {
        int x1, y1, x2, y2;
        int valX, valY;
        int colorId = hex1.GetComponent<Hexagon>().id;

        x1 = hex1.GetComponent<Hexagon>().row;
        y1 = hex1.GetComponent<Hexagon>().col;

        x2 = hex2.GetComponent<Hexagon>().row;
        y2 = hex2.GetComponent<Hexagon>().col;

        if (x1 < x2)
        {
            valX = x1;
            valY = y1;
        }
        else if (x2 < x1)
        {
            valX = x2;
            valY = y2;
        }
        else
        {
            if (y1 < y2)
            {
                valX = x1;
                valY = y1;
            }
            else
            {
                valX = x2;
                valY = y2;
            }
        }

        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX, valY + 1].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }

        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX, valY - 2].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX, valY + 3].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX + 1, valY - 3].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX, valY + 4].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX + 1, valY - 2].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX + 1, valY + 3].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        try
        {
            if (allHex[valX, valY].GetComponent<Hexagon>().id == allHex[valX + 1, valY].GetComponent<Hexagon>().id)
            {
                return false;
            }
        }
        catch (System.Exception)
        {


        }
        return true;
    }



   

}
