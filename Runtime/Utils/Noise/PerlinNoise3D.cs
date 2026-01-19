using System.Runtime.CompilerServices;

namespace IRG.Utils
{  
 //Based on Ken Perlin's noise.
 public static class PerlinNoise3D
 {
  private static readonly byte PCount = 255;

  private static readonly byte[] P =
  {
   151, 160, 137, 91, 90, 15,
   131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
   190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
   88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
   77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244,
   102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
   135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
   5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
   223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
   129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
   251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
   49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
   138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180,

   151, 160, 137, 91, 90, 15,
   131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
   190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33,
   88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
   77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244,
   102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
   135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123,
   5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
   223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9,
   129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
   251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107,
   49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
   138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
  };

  public static double Get(float x, float y, float z)
  {
   int x0 = (int)x;
   int y0 = (int)y;
   int z0 = (int)z;

   //Get distance vectors to each coord.
   double dx0 = x - x0;
   double dy0 = y - y0;
   double dz0 = z - z0;

   double dx1 = dx0 - 1d;
   double dy1 = dy0 - 1d;
   double dz1 = dz0 - 1d;

   //Get int position to each coord.
   x0 &= PCount;
   y0 &= PCount;
   z0 &= PCount;

   int x1 = x0 + 1;
   int y1 = y0 + 1;
   int z1 = z0 + 1;

   //Get random permutation to each point of the cube.
   byte p000 = P[P[P[x0] + y0] + z0];
   byte p001 = P[P[P[x0] + y0] + z1];
   byte p010 = P[P[P[x0] + y1] + z0];
   byte p011 = P[P[P[x0] + y1] + z1];
   byte p100 = P[P[P[x1] + y0] + z0];
   byte p101 = P[P[P[x1] + y0] + z1];
   byte p110 = P[P[P[x1] + y1] + z0];
   byte p111 = P[P[P[x1] + y1] + z1];

   double g000 = Grad(p000, dx0, dy0, dz0);
   double g001 = Grad(p001, dx0, dy0, dz1);
   double g010 = Grad(p010, dx0, dy1, dz0);
   double g011 = Grad(p011, dx0, dy1, dz1);
   double g100 = Grad(p100, dx1, dy0, dz0);
   double g101 = Grad(p101, dx1, dy0, dz1);
   double g110 = Grad(p110, dx1, dy1, dz0);
   double g111 = Grad(p111, dx1, dy1, dz1);

   double smoothDistanceX = PerlinFade(dx0);
   double smoothDistanceY = PerlinFade(dy0);
   double smoothDistanceZ = PerlinFade(dz0);

   return Lerp(
    Lerp(Lerp(g000, g100, smoothDistanceX), Lerp(g010, g110, smoothDistanceX), smoothDistanceY),
    Lerp(Lerp(g001, g101, smoothDistanceX), Lerp(g011, g111, smoothDistanceX), smoothDistanceY),
    smoothDistanceZ);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static double PerlinFade(double d) => d * d * d * (d * (d * 6f - 15f) + 10f);

  private static double Grad(byte hash, double x, double y, double z)
  {
   return (hash & 0xF) switch
   {
    0x0 => x + y,
    0x1 => -x + y,
    0x2 => x - y,
    0x3 => -x - y,
    0x4 => x + z,
    0x5 => -x + z,
    0x6 => x - z,
    0x7 => -x - z,
    0x8 => y + z,
    0x9 => -y + z,
    0xA => y - z,
    0xB => -y - z,
    0xC => y + x,
    0xD => -y + z,
    0xE => y - x,
    0xF => -y - z,
    _ => 0 //never happens
   };
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static double Lerp(double a, double b, double t) => (b - a) * t + a;
 }
}
