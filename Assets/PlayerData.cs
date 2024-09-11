[System.Serializable]
public class PlayerData
{
    public string name;
    public string score;
    public string salad_name;

    public PlayerData(string name, string score, string salad_name)
    {
        this.name = name;
        this.score = score;
        this.salad_name = salad_name;
    }

    public override string ToString()
    {
        return $"{name},{score},{salad_name}";
    }

    // Optional: Parse from a CSV string
    public static PlayerData FromString(string csvString)
    {
        string[] parts = csvString.Split(',');
        return new PlayerData(parts[0], parts[1], parts[2]);
    }
}
