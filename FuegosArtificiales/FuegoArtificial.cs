using System.Numerics;
using Raylib_cs;
namespace FuegosArtificiales;

public class FuegoArtificial(
    Vector2 posicion,
    Vector2 velocidad,
    Color color,
    float velRadio,
    int numParts,
    float vida)
{
    private Vector2 _posicion = posicion;
    public Vector2 Velocidad = velocidad;
    public bool Explotado = false;
    public readonly List<Particula> Particulas = [];
    private static readonly Vector2 G = new Vector2(0.0f, 500.0f);
    private static readonly Random Random = new Random();
    private readonly Queue<Vector2> _traza = new Queue<Vector2>();
    private const int LongitudTraza = 100;

    public void Actualizar(float dt)
    {
        if (!Explotado)
        {
            // Física del cohete
            Velocidad += G * dt;
            _posicion += Velocidad * dt;

            // Explotar cuando la velocidad vertical se hace positiva (llegó al punto más alto)
            if (Velocidad.Y >= 0)
            {
                Explotar(velRadio, numParts, vida);
            }
        }
        else
        {
            // Actualizar partículas
            foreach (var particula in Particulas)
            {
                particula.Actualizar(dt);
            }
        }
    }

    private void Explotar(float velRadio, int numParts, float vida)
    {
        // Crear partículas UNA SOLA VEZ
        for (int i = 0; i < numParts; i++)
        {
            float velX = -velRadio + (float)Random.NextDouble() * (2 * velRadio);
            float velY = -velRadio + (float)Random.NextDouble() * (2 * velRadio);

            int r = Random.Next(128, 256); // Entre 128 y 255
            int g = Random.Next(128, 256);
            int b = Random.Next(128, 256);
            Particulas.Add(new Particula(
                _posicion,
                new Vector2(velX, velY),
                new Color(r, g, b, 255),
                vida
            ));

        }
        Explotado = true;
    }

    public void Dibujar()
    {
        if (!Explotado)
        {
            // Añadir posición a la traza
            _traza.Enqueue(_posicion);
            if (_traza.Count > LongitudTraza)
                _traza.Dequeue();

            // Dibujar traza
            int i = 0;
            foreach (Vector2 traza in _traza)
            {
                float alpha = (float)i / _traza.Count;
                Color colorTraza = new Color(color.R, color.G, color.B, (byte)(alpha * 150));
                Raylib.DrawCircleV(traza, 1.0f, colorTraza);
                i++;
            }

            Raylib.DrawCircleV(_posicion, 2.0f, color);
        }
        else
        {
            // Dibujar partículas
            foreach (var particula in Particulas)
            {
                particula.Dibujar();
            }
        }
    }

    public bool EstaMuerto()
    {
        return Explotado && Particulas.All(p => p.EstaMuerta());
    }
}