using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Point : MonoBehaviour
{
    [SerializeField]
    List<GameObject> hexagons = new List<GameObject>();
    bool isStart;
    float maxDistance = 1.5f;
    public static int stepCount;
    Control control;
    GridManager gridManager;

    string checkHex;
    int turnCounter = 0;

    private void Awake()
    {
        StartCoroutine(Delay(2f));
    }
    private void Start()
    {
        
        stepCount = 0;
        control = FindObjectOfType<Control>();
        gridManager = FindObjectOfType<GridManager>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);

        StartCoroutine(FindHexagonsWithDelay(.1f));
    }

    IEnumerator FindHexagonsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindHexagons();
        }
    }

    //////// 0.1 saniyede bir her 3 altıgenin ortasında ki görünmez nokta, çevresindeki altıgenleri tespit eder ve listeler
    ///////// ControlHex fonksiyonu çağırılır patlama var mı kontrol edilir
    void FindHexagons()
    {
        hexagons.Clear();

        Hexagon[] allHex = GameObject.FindObjectsOfType<Hexagon>();

        for (int i = 0; i < allHex.Length; i++)
        {
            float distance = (allHex[i].transform.position - this.transform.position).sqrMagnitude;
            if (distance < maxDistance)
            {

                hexagons.Add(allHex[i].gameObject);
    
            }

        }

        StartCoroutine(ControlHex());

    }
    ///////// Çevresinde ki 3 altıgenin idleri aynıysa yani renkleri, altıgenlerin isDestrol bool değerini true yapar. Altıngenler yok olur.
    IEnumerator ControlHex()
    {

        if (hexagons.Count == 3)
        {
            int i = 0;
            if (hexagons[i].GetComponent<Hexagon>().gameObject != null && hexagons[i + 1].GetComponent<Hexagon>().gameObject != null && hexagons[i + 2].GetComponent<Hexagon>().gameObject != null)
            {
                if (hexagons[i].GetComponent<Hexagon>().id == hexagons[i + 1].GetComponent<Hexagon>().id && hexagons[i + 1].GetComponent<Hexagon>().id == hexagons[i + 2].GetComponent<Hexagon>().id)
                {
                    for (int j = 0; j < hexagons.Count; j++)
                    {
                        if (isStart && (gridManager.state != GridManager.GameState.SPAWNING))
                        {


                            if (!hexagons[j].GetComponent<Hexagon>().isDestroy)
                            {
                                hexagons[j].GetComponent<Hexagon>().isDestroy = true;
                                StartCoroutine(Drop());
                                yield return new WaitForSeconds(.1f);
                            }



                        }
                        if (!isStart)
                        {
                            hexagons[j].GetComponent<Hexagon>().ChangeColor();

                        }


                    }

                    yield return new WaitForSeconds(.5f);////.2f


                }
            }
        }

        yield return new WaitForSeconds(5f);

    }
    private void Update() {
        if(gridManager.state != GridManager.GameState.GAMING){
            StartCoroutine(Drop());
        }
    }

/////// Seçim yapıldığında listeye alınan 3 altıgeni child objesi yapar ve outline çizdirir
    public void SelectObje()
    {

        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].transform.parent = this.transform;
            hexagons[i].GetComponent<SpriteRenderer>().sortingOrder = 2;
            hexagons[i].GetComponent<DrawOutline>().Outline();
        }

    }

/////// Başka nokta seçildiğinde yada altıgenler patlatıldığında, seçilen altıgenlerin outlineni siler ve parentını null yapar
 IEnumerator Drop()
    {

        for (int i = hexagons.Count; i < -1; i--)
        {
            hexagons[i].GetComponent<DrawOutline>().DeleteOutline();
        }
        gameObject.GetComponent<DrawOutline>().DeleteOutline();
        DropObje();
        yield return new WaitForSeconds(.5f);
    }

    public void DropObje()
    {
        for (int i = 0; i < hexagons.Count; i++)
        {
            hexagons[i].GetComponent<DrawOutline>().DeleteOutline();
            hexagons[i].transform.parent = null;
            hexagons[i].GetComponent<SpriteRenderer>().sortingOrder = 0;

        }
    }

    public void TurnDirection(string _direction)
    {
        StartCoroutine(Turn(_direction));

    }
   

   ///////// Sağa yada sola dönme fonksiyonu
   ////// Listedeki 3 altıgenin sağa ve sola göre konumlarını değiştirir
    IEnumerator Turn(string _direction)
    {
        int x0, x1, x2, y0, y1, y2;
        string deneme = _direction;


        hexagons = hexagons.OrderBy(go => go.transform.position.y).ToList();
        try
        {
            if (hexagons[0].transform.position.x < hexagons[1].transform.position.x)
            {
                ///sağında klasik dönme 
                checkHex = "normal";
            }
            else
            {
                /////solunda tam tersi dönme  
                checkHex = "reverse";
            }
        }
        catch (System.Exception)
        {

        }



        x0 = hexagons[0].GetComponent<Hexagon>().row;
        x1 = hexagons[1].GetComponent<Hexagon>().row;
        x2 = hexagons[2].GetComponent<Hexagon>().row;

        y0 = hexagons[0].GetComponent<Hexagon>().col;
        y1 = hexagons[1].GetComponent<Hexagon>().col;
        y2 = hexagons[2].GetComponent<Hexagon>().col;


        if (checkHex == "normal")
        {
            if (_direction == "right")
            {
                checkHex = "reverse";
            }
            else
            {
                checkHex = "normal";
            }
        }
        else
        {
            if (_direction == "right")
            {
                checkHex = "normal";
            }
            else
            {
                checkHex = "reverse";
            }
        }



        if (checkHex == "normal")
        {

            hexagons[2].GetComponent<Hexagon>().row = x0;
            hexagons[2].GetComponent<Hexagon>().col = y0;

            hexagons[1].GetComponent<Hexagon>().row = x2;
            hexagons[1].GetComponent<Hexagon>().col = y2;

            hexagons[0].GetComponent<Hexagon>().row = x1;
            hexagons[0].GetComponent<Hexagon>().col = y1;

            yield return new WaitForSeconds(.4f);
            if (hexagons[0].GetComponent<Hexagon>().isDestroy || hexagons[1].GetComponent<Hexagon>().isDestroy || hexagons[2].GetComponent<Hexagon>().isDestroy)
            {

                stepCount++;
                Debug.Log("patlama başarılı");

                StartCoroutine(Drop());
                control.isTouched = false;
                turnCounter = 0;
                yield break;

            }
            else if (turnCounter < 2)
            {
                turnCounter++;
                yield return StartCoroutine(Turn(_direction));
            }
            else
            {
                turnCounter = 0;
                yield break;
            }

        }


        else
        {
            hexagons[2].GetComponent<Hexagon>().row = x1;
            hexagons[2].GetComponent<Hexagon>().col = y1;

            hexagons[1].GetComponent<Hexagon>().row = x0;
            hexagons[1].GetComponent<Hexagon>().col = y0;

            hexagons[0].GetComponent<Hexagon>().row = x2;
            hexagons[0].GetComponent<Hexagon>().col = y2;

            yield return new WaitForSeconds(.4f);

            if (hexagons[0].GetComponent<Hexagon>().isDestroy || hexagons[1].GetComponent<Hexagon>().isDestroy || hexagons[2].GetComponent<Hexagon>().isDestroy)
            {

                stepCount++;
                Debug.Log("patlama başarılı");

                StartCoroutine(Drop());
                control.isTouched = false;
                turnCounter = 0;
                yield break;

            }
            else if (turnCounter < 2)
            {
                turnCounter++;
                yield return StartCoroutine(Turn(_direction));
            }
            else
            {
                turnCounter = 0;
                yield break;
            }





        }

    }



       IEnumerator Delay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        isStart = true;

    }


}