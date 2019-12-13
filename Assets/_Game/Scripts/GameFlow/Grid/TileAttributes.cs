using UnityEngine;

namespace _Game.Scripts.GameFlow.Grid
{
   public class TileAttributes: MonoBehaviour
   {

      public TileAttributes Parent;

      public GameObject node;

      public GameObject Node
      {
         get => node;
         set => node = value;
      }
      
      [Header("Walkable State")] [SerializeField]
      public bool walkable;
   
      public bool Walkable
      {
         get => walkable;
         set => walkable = value;
      }

      [Header("A* Variables")]

      public int f;
   
      public int F => G+H;


      //how far away from Start Position
      [SerializeField] public int g;
   
      public int G
      {
         get => g;
         set => g = value;
      }

   
      //how far away from End Position
      [SerializeField] public int h;
   
      public int H
      {
         get => h;
         set => h = value;
      }

      [Header("Grid Coordinates")]
      [SerializeField] public int gridX;
   
      public int GridX
      {
         get => gridX;
         set => gridX = value;
      }

      [SerializeField] public int gridY;
   
      public int GridY
      {
         get => gridY;
         set => gridY = value;
      }



   }
}
