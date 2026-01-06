using System.Numerics;
using Raylib_cs;

namespace FuegosArtificiales;

public class Particula
{
    private static readonly Vector2 G = new Vector2(0.0f, 10.0f);
    private Vector2 _posicion;
    private Vector2 _velocidad;
    private readonly Color _color;
    private float _vida;
    private readonly Queue<Vector2> _traza = new Queue<Vector2>();

    public Particula(Vector2 posicion, Vector2 velocidad, Color color, float vida)
    {
        _posicion = posicion;
        _velocidad = velocidad;
        _color = color;
        _vida = vida;
    }

    public void Actualizar(float dt)
    {
        _velocidad += G * dt;
        _posicion += _velocidad * dt;
        _vida -= dt * 60; // Reducir vida (ajustado para que sea consistente con framerate)
    }


    public void Dibujar()
    {
        if (EstaMuerta()) return;

        // Añadir a traza
        _traza.Enqueue(_posicion);

        // Longitud de traza proporcional a la vida restante
        int longitudMaxima = (int)(_vida / 1.5f); // Ajusta el divisor para controlar cuánto se reduce

        // Eliminar posiciones viejas si excede la longitud máxima
        while (_traza.Count > longitudMaxima && _traza.Count > 1)
        {
            _traza.Dequeue();
        }

        // Dibujar traza
        int i = 0;
        foreach (Vector2 pos in _traza)
        {
            float alpha = (float)i / _traza.Count;
            Color colorTraza = new Color(_color.R, _color.G, _color.B, (byte)(alpha * 150));
            Raylib.DrawCircleV(pos, 1.0f, colorTraza);
            i++;
        }

        Raylib.DrawCircleV(_posicion, 2.0f, _color);
    }

    public bool EstaMuerta()
    {
        return _vida <= 0;
    }
}