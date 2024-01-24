using System.IO;
namespace clases;

public class colorConverter
{

    private static colorConverter instance = null;
    public Dictionary<string, string> colores = new Dictionary<string, string>();
    private static String fileName = ".\\files\\color.csv";

    /// <summary>
    /// Genera objeto colorConverter
    /// </summary>
    /// <exception cref="IOException">
    /// Lanza en caso de falla de lectura
    /// </exception>
    public colorConverter()
    {
        string fileDir = Path.GetFullPath(fileName);

        if (File.Exists(fileDir))
        {

            StreamReader reader = new StreamReader(fileName);

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String[] values = line.Split(",");
                colores.Add(values[0], values[1]);

            }
            reader.Close();
        }

    }

    /// <summary>
    /// Retorna unico objeto colorConverter
    /// </summary>
    /// <exception cref="IOException">
    /// Lanza en caso de falla de lectura
    /// </exception>
    public static colorConverter getInstance()
    {
        if (instance == null)
        {
            instance = new colorConverter();
        }
        return instance;
    }

    /// <summary>
    /// Retorna código de color Hex
    /// </summary>
    /// <param name="colorTexto"></param>
    /// <returns>String con Hex code del color enviado</returns>
    public string GetColor(string colorTexto)
    {

        string color = ""; ;
        colores.TryGetValue(colorTexto, out color);
        return color;
    }



}