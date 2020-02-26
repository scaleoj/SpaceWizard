using System;
using UnityEngine;

namespace _Game.Scripts.GameFlow.Grid
{
   [Serializable]
   public class TileAttribute
   {
      public TileAttribute(GameObject node, int x, int y)
      {
         Node = node;
         GridX = x;
         GridY = y;
         G = 0;
         H = 0;
      }

      public TileAttribute Parent;

      public GameObject node;

      public GameObject Node
      {
         get => node;
         set => node = value;
      }

      public int f;
   
      public int F => G+H;


      //how far away from Start Position
      public int G { get; set; }


      //how far away from End Position
      public int H { get; set; }

      public int gridX;
   
      public int GridX
      {
         get => gridX;
         set => gridX = value;
      }

      public int gridY;
   
      public int GridY
      {
         get => gridY;
         set => gridY = value;
      }



   }
}
