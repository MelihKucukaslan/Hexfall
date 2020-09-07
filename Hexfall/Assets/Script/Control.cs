using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class Control : MonoBehaviour
{
    public bool isTouched = false;
    GameObject lastObje;
    GridManager gridManager;
    public TMP_Text stepText;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }


    //////// Tuş kontrollerinin yapıldığı sınıf
    //////// Noktaya tıklanırsa yada seçilirse üçlü altıgen grubun sağa ve sola döndürme işlemi yapılır
    ////// Noktanın solundaki herhangi bir altıngene tıklanırsa sola aksi halinde sağa döner
    private void Update()
    {
        stepText.text = "MOVES" + Point.stepCount.ToString();

        if (Input.GetMouseButtonDown(0) && gridManager.state == GridManager.GameState.GAMING)
        {
            Vector2 raycastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(raycastPos, Vector2.zero);
            if (hit.collider != null && gridManager.state == GridManager.GameState.GAMING)
            {

                if (hit.collider.tag == "Point")
                {


                    if (isTouched)
                    {
                        lastObje.GetComponent<Point>().DropObje();
                        isTouched = false;
                    }

                    if (!isTouched)
                    {
                        lastObje = hit.transform.gameObject;
                        hit.collider.GetComponent<Point>().SelectObje();
                        isTouched = true;
                    }

                }

                if (hit.collider.tag == "Hexagon")
                {

                    if (isTouched)
                    {
                        if (lastObje.transform.position.x < hit.transform.position.x)
                        {
                            lastObje.GetComponent<Point>().TurnDirection("right");
                        }
                        else
                        {
                            lastObje.GetComponent<Point>().TurnDirection("left");
                        }

                    }

                }

            }
        }

    }







}