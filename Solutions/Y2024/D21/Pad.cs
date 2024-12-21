using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D21;

public class Pad
{
   public Dictionary<Vec2D, char> PosMap { get; }
   public Dictionary<char, Vec2D> KeyMap { get; }
   
   private Pad(Dictionary<Vec2D, char> posMap)
   {
      PosMap = posMap;
      KeyMap = posMap.ToDictionary(
         keySelector:     kvp => kvp.Value,
         elementSelector: kvp => kvp.Key);
   }
   
   public static Pad Parse(string flat, int cols, char skip)
   {
      var keys = new Dictionary<Vec2D, char>();
      var rows = flat.Length / cols;
      
      for (var y = 0; y < rows; y++)
      for (var x = 0; x < cols; x++)
      {
         var pos = new Vec2D(x, y);
         var key = flat[y * cols + x];

         if (key != skip)
         {
            keys[pos] = key;
         }
      }
      
      return new Pad(keys);
   }
}