using System;

[System.Serializable]
public class SaveData
{
    public Fase[] fases; 
    public TempoData tempo;

    public SaveData(int numeroFases)
    {
        fases = new Fase[numeroFases];
        for (int i = 0; i < numeroFases; i++)
        {
            fases[i] = new Fase();
        }
        tempo = new TempoData();
    }
}

[System.Serializable]
public class Fase
{
    public bool completou;
    public int turnos;
    public float tempo;

    public Fase() { }

    public Fase(bool completou, int turnos, float tempo)
    {
        this.completou = completou;
        this.turnos = turnos;
        this.tempo = tempo;
    }
}

[System.Serializable]
public class TempoData
{
    public int fase;
    public int estrela;
    public int tempo;
    public int turnos;
}
