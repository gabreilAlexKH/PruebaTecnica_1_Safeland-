using System.Security.Cryptography.X509Certificates;
using System.Collections;
using DocumentFormat.OpenXml;

using clases;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;

public class Program
{

    private static colorConverter colors = null;
    private static List<Gaveta> gavetas = new List<Gaveta>();
    private static MySQLCon con = null;
    private static String fileHTLM = ".\\files\\invent.html";


    public static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
        SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
        if (cell.CellValue == null)
        {
            return null;
        }
        string value = cell.CellValue.InnerXml;
        if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
        {
            return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
        }
        else
        {
            return null;
        }
    }

    public static void printFile(Stream exelFile)
    {

        SpreadsheetDocument file = SpreadsheetDocument.Open(exelFile, false, new OpenSettings());
        WorkbookPart book = file.WorkbookPart;
        IEnumerable<Sheet> sheets = file.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        string relationshipId = sheets.First().Id.Value;
        WorksheetPart worksheetPart = (WorksheetPart)file.WorkbookPart.GetPartById(relationshipId);
        Worksheet workSheet = worksheetPart.Worksheet;
        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        IEnumerable<Row> rows = sheetData.Descendants<Row>();

        foreach (Row row in rows)
        {
            Cell cell = row.Descendants<Cell>().ElementAt(0);
            string value = GetCellValue(file, cell);
            if (value != null)
            {
                Console.WriteLine(value);
            }
        }
    }


    public static void readFile(Stream exelFile, List<Gaveta> gavetas)
    {

        SpreadsheetDocument file = SpreadsheetDocument.Open(exelFile, false, new OpenSettings());
        WorkbookPart book = file.WorkbookPart;
        IEnumerable<Sheet> sheets = file.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
        string relationshipId = sheets.First().Id.Value;
        WorksheetPart worksheetPart = (WorksheetPart)file.WorkbookPart.GetPartById(relationshipId);
        Worksheet workSheet = worksheetPart.Worksheet;
        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
        IEnumerable<Row> rows = sheetData.Descendants<Row>();

        foreach (Row row in rows)
        {
            Cell cell = row.Descendants<Cell>().ElementAt(0);
            string value = GetCellValue(file, cell);
            if (value != null)
            {
                Gaveta gaveta = Gaveta.formString(value);
                gavetas.Add(gaveta);
            }
        }
    }

    public static void readDatabase(MySQLCon con, List<Gaveta> gavetas)
    {

        List<Gaveta>.Enumerator gavetaEnumerator = gavetas.GetEnumerator();
        while (gavetaEnumerator.MoveNext())
        {
            Gaveta gaveta = gavetaEnumerator.Current;
            string nombre = con.getNombreByCodigo(gaveta.Codigo);
            gaveta.GavetaNombre = nombre.ToUpper();
        }

    }

    public static void printInventario(List<Gaveta> gavetas)
    {
        List<Gaveta>.Enumerator gavetaEnumerator = gavetas.GetEnumerator();

        while (gavetaEnumerator.MoveNext())
        {
            Gaveta gaveta = gavetaEnumerator.Current;
            for (int i = gaveta.Cantidad; i > 0; i--)
            {
                Console.WriteLine($"{gaveta.Nombre} - {gaveta.GavetaNombre}");
            }
            Console.WriteLine();
        }
    }

    public static bool checkExelFile(string file)
    {

        if (File.Exists(file))
        {

            var extension = Path.GetExtension(file);
            return extension == ".xlsx";
        }

        return false;
    }

    public static void Main()
    {


        try
        {
            con = MySQLCon.getInstance();
            Console.WriteLine("Conexión con base de datos lista");
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine("No se pudo establecer conexion de la base de datos no existe \n");
        }

        try
        {
            colors = colorConverter.getInstance();
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error al cargar colores");
        }

        if (colors != null && con != null)
        {
            Console.WriteLine("INGRESE RUTA DEL ARCHIVO:");
            string fileName = Console.ReadLine().Trim();



            try
            {

                if (checkExelFile(fileName))
                {

                    FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                    Console.WriteLine("\nVISUALISANDO ARCHIVO");
                    printFile(file);

                    Console.WriteLine("\nPRESIONE ENTER PARA CONTINUAR");
                    Console.ReadLine();

                    readFile(file, gavetas);
                    file.Close();
                    readDatabase(con, gavetas);
                    con.closeConection();

                    Console.WriteLine("\nIMPIMIENDO INVENTARIO");
                    printInventario(gavetas);


                    // Generar HTML
                    Console.WriteLine("\nQUIERE IMPRIMIR? Y/N");
                    String conf = Console.ReadLine().ToUpper();

                    if (conf.Equals("Y"))
                    {

                        TableHTML tableGenerator = new TableHTML(fileHTLM, gavetas);
                        tableGenerator.copyTemplate();
                        tableGenerator.writeTable();

                        string path = Path.GetFullPath(fileHTLM);
                        Console.WriteLine($"html guardado en : {path} \n");
                    }


                }
                else
                {
                    Console.WriteLine("Archivo no compatible");
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al leer archivo");
                Console.WriteLine(ex);
            }
            finally
            {

            }
        }





    }
}

