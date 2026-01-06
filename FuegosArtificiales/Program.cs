using Raylib_cs;
using System.Numerics;

namespace FuegosArtificiales;

class Program
{
    static void Main(string[] args)
    {
        const float G = 500.0f;
        const int Width = 1024;
        const int Height = 720;

        Raylib.InitWindow(Width, Height, "Fuegos Artificiales");
        Raylib.SetTargetFPS(60);

        List<FuegoArtificial> fuegos = new List<FuegoArtificial>();
        Random random = new Random();

        while (!Raylib.WindowShouldClose())
        {
            float dt = Raylib.GetFrameTime();

            // Click para crear nuevo fuego artificial
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Vector2 mousePos = Raylib.GetMousePosition();

                // Calcular velocidad inicial para llegar a mousePos
                float vx = -((Width / 2.0f - mousePos.X) * G) / MathF.Sqrt(2 * (Height - mousePos.Y) * G);
                float vy = -MathF.Sqrt(2 * (Height - mousePos.Y) * G);

                int r = random.Next(128, 256); // Entre 128 y 255
                int g = random.Next(128, 256);
                int b = random.Next(128, 256);

                fuegos.Add(new FuegoArtificial(
                    posicion: new Vector2(Width / 2, Height),
                    velocidad: new Vector2(vx, vy),
                    color: new Color(r, g, b, 255),
                    velRadio: 50.0f,
                    numParts: 50,
                    vida: 200.0f
                ));
            }

            // Actualizar todos los fuegos artificiales
            foreach (var fuego in fuegos)
            {
                fuego.Actualizar(dt);
            }

            // Eliminar fuegos muertos
            fuegos.RemoveAll(f => f.EstaMuerto());

            // Dibujar
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            foreach (var fuego in fuegos)
            {
                fuego.Dibujar();
            }

            Raylib.DrawText("Click para lanzar fuegos artificiales", 10, 10, 20, Color.White);
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}