using System.IO;
namespace clases;

public class colorConverter
{

    private static colorConverter instance = null;
    public Dictionary<string, string> colores = null;
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
        StreamReader reader = new StreamReader(fileName);
        colores = new Dictionary<string, string>();

        String line;

        try
        {
            while ((line = reader.ReadLine()) != null)
            {
                String[] values = line.Split(",");
                colores.Add(values[0], values[1]);
            }

        }
        catch (IOException ex)
        {
            colores.Clear();
            throw ex;
        }
        finally
        {
            reader.Close();
        }
    }

    public static bool CheckFile()
    {
        string fileDir = Path.GetFullPath(fileName);
        return File.Exists(fileDir);
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