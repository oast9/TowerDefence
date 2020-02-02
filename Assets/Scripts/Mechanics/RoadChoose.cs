using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChoose : MonoBehaviour
{

     Ray ray;
     RaycastHit hit;
    public Renderer rend; //цвет ячейки

    // Start is called before the first frame update
    void Start()
    {
        
    }
     
     void Update()
     {
         ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
         if(Physics.Raycast(ray, out hit))
         {
             rend.material.color = Color.blue;
         }
     }

     private void OnMouseDown() {
         Debug.Log("Я работаю");
         
     }
}
