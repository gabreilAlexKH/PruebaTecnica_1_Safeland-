using System;
using System.Text;
namespace clases;


public class TableHTML
{

	private static String templateDir = ".\\files\\template.html";
	private string newFile;
	private List<Gaveta> gavetas;
	private List<String> template;
	private colorConverter colorConv;
	private static int TEMP1 = 9;
	private static int NCOL = 3;


	public TableHTML(string newFile, List<Gaveta> gavetas)
	{

		this.newFile = newFile;
		this.gavetas = gavetas;
		this.colorConv = colorConverter.getInstance();
	}

	/**
	 * Copia el contenido de una template de html
	 * 
	 * @return
	 * @throws IOException
	 */
	public bool copyTemplate()
	{

		template = new List<String>();

		if (File.Exists(templateDir)) {
			
			StreamReader reader = new StreamReader(templateDir);
			String line;
			while ((line = reader.ReadLine()) != null) {
				template.Add(line);
			}
			reader.Close();
			return true;
		}
		return false;
	}

	public StringBuilder writeGroup(List<Gaveta> gavetas, int length)
	{

		StringBuilder header = writeHeader(gavetas);
		int max = 0;

		foreach (Gaveta gaveta in gavetas)
		{

			int cantidad = gaveta.Cantidad;
			if (cantidad > max)
			{
				max = cantidad;
			}
		}
		StringBuilder body = writeBody(gavetas, max);

		return header.Append(body);
	}

	private StringBuilder writeHeader(List<Gaveta> gavetas)
	{

		StringBuilder header = new StringBuilder("");
		header.Append("  <tr>\r\n");

		foreach (Gaveta gaveta in gavetas)
		{
			header.Append("<td>" + gaveta.Nombre + "</td>\r\n");
		}

		header.Append("  </tr>\r\n");
		return header;
	}

	private StringBuilder writeBody(List<Gaveta> gavetas, int maxSize)
	{

		StringBuilder body = new StringBuilder("");

		for (int i = 0; i <= maxSize; i++)
		{
			StringBuilder row = new StringBuilder("");
			row.Append("  <tr>\r\n");
			foreach (Gaveta gaveta in gavetas)
			{

				if (gaveta.Cantidad >= i)
				{
					String color = colorConv.GetColor(gaveta.GavetaNombre);
					row.Append("		<td bgcolor = " + color + ">" + "  " + "</td>\r\n");
				}
				else
				{
					row.Append("		<td bgcolor = " + "#FFFFFF" + ">" + "  " + "</td>\r\n");
				}
			}
			row.Append("  </tr>\r\n");
			body.Append(row);
		}
		return body;
	}


	private StringBuilder writeJump(int length)
	{
		StringBuilder jump = new StringBuilder("");

		jump.Append("  <tr>\r\n");
		for (int i = 1; i <= length; i++)
		{
			jump.Append("		<td>" + "  " + "</td>\r\n");
		}
		jump.Append("  </tr>\r\n");
		return jump;
	}

	public void writeTable() {

		StreamWriter writer = new StreamWriter(newFile);

		int size = gavetas.Count;
		int rest = size % NCOL;
		string jump = writeJump(NCOL).ToString();

		if (rest != 0) {
			for (int i = 0; i < 3 - rest; i += 1) {
				gavetas.Add(new Gaveta("", 0, "", "BLANCA"));
			}
		}

		size = gavetas.Count;
		size = gavetas.Count;
		for (int i = 0; i < TEMP1; i++) {
			writer.Write(template[i] + "\n");
		}
		
		writer.Write("<table>\r\n");
		for (int i = 0; i < size; i += NCOL) {
			
			List<Gaveta> rowGaveta = gavetas.GetRange(i, NCOL);

			String group = writeGroup(rowGaveta, rowGaveta.Count).ToString();
			writer.Write(group);

			if (i + 3 < size) {
				writer.Write(jump);
			}
		}

		writer.Write("</table>");
		for (int i = TEMP1; i < template.Count; i++) {
			writer.Write("\n" + template[i]);
		}
		writer.WriteLine();
		writer.Close();
	}





}