package clases;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class TableHTML {

	private static String templateDir = ".\\files\\template.html";
	private File newFile;
	private List<Gaveta> gavetas;
	private colorConverter colorConv;
	private List<String> template;
	private static int TEMP1 = 9;
	private static int NCOL = 3;

	public TableHTML(File newFile, ArrayList<Gaveta> gavetas) throws IOException {

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
	public boolean copyTemplate() throws IOException {

		File temp = new File(templateDir);
		template = new ArrayList<String>();

		if (temp.exists()) {
			BufferedReader reader = new BufferedReader(new FileReader(temp));
			String line;
			while ((line = reader.readLine()) != null) {
				template.add(line);
			}
			reader.close();
			return true;
		}
		return false;
	}

	/**
	 * Genera un HTML en base al contenido de las lista gavetas
	 * 
	 * @throws IOException
	 */
	public void writeTable() throws IOException {

		BufferedWriter writer = new BufferedWriter(new FileWriter(newFile));
		int size = gavetas.size();
		int rest = size % NCOL;
		String jump = writeJump(NCOL).toString();

		if (rest != 0) {
			for (int i = 0; i < 3 - rest; i += 1) {
				gavetas.add(new Gaveta("", 0, "", "BLANCA"));
			}
		}

		size = gavetas.size();
		for (int i = 0; i < TEMP1; i++) {
			writer.append(template.get(i) + "\n");
		}

		writer.append("<table>\r\n");
		for (int i = 0; i < size; i += NCOL) {
			List<Gaveta> rowGaveta = gavetas.subList(i, i + NCOL);

			String group = writeGroup(rowGaveta, rowGaveta.size()).toString();
			writer.append(group);

			if (i + 3 < size) {
				writer.append(jump);
			}
		}

		writer.append("</table>");
		for (int i = TEMP1; i < template.size(); i++) {
			writer.append("\n" + template.get(i));
		}

		writer.close();
	}

	/**
	 * Retorna el código html de una seria de filas en base al contenido de gavetas
	 * pasadas como parámetros
	 * 
	 * @param gavetas
	 * @param length
	 * @return
	 */
	public StringBuilder writeGroup(List<Gaveta> gavetas, int length) {

		StringBuilder header = writeHeader(gavetas, length);
		int max = 0;

		for (int i = length - 1; i >= 0; i--) {
			Gaveta gaveta = gavetas.get(i);
			int cantidad = gaveta.getCantidad();
			if (cantidad > max) {
				max = cantidad;
			}
		}
		StringBuilder body = writeBody(gavetas, length, max);

		return header.append(body);
	}

	private StringBuilder writeHeader(List<Gaveta> gavetas, int length) {

		StringBuilder header = new StringBuilder("");
		header.append("  <tr>\r\n");

		for (int i = 0; i < length; i++) {
			Gaveta gaveta = gavetas.get(i);
			header.append("<td>" + gaveta.getNombre() + "</td>\r\n");
		}

		header.append("  </tr>\r\n");
		return header;
	}

	private StringBuilder writeBody(List<Gaveta> gavetas, int length, int maxSize) {

		StringBuilder body = new StringBuilder("");

		for (int i = 0; i <= maxSize; i++) {
			StringBuilder row = new StringBuilder("");
			row.append("  <tr>\r\n");
			for (int j = 0; j < length; j++) {

				Gaveta gaveta = gavetas.get(j);
				if (gaveta.getCantidad() >= i) {
					String color = colorConv.getColor(gaveta.getGavetanombre());
					row.append("		<td bgcolor = " + color + ">" + "  " + "</td>\r\n");
				} else {
					row.append("		<td bgcolor = " + "#FFFFFF" + ">" + "  " + "</td>\r\n");
				}
			}
			row.append("  </tr>\r\n");
			body.append(row);
		}
		return body;
	}

	private StringBuilder writeJump(int length) {
		StringBuilder jump = new StringBuilder("");

		jump.append("  <tr>\r\n");
		for (int i = 1; i <= length; i++) {
			jump.append("		<td>" + "  " + "</td>\r\n");
		}
		jump.append("  </tr>\r\n");
		return jump;
	}

}
