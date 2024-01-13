package clases;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.HashMap;
import java.util.Map;

public class colorConverter {

	private static colorConverter instance = null;
	public Map<String, String> colores = new HashMap<String, String>();
	private static String fileName = ".\\files\\color.csv";

	public colorConverter() throws IOException {

		BufferedReader reader = null;
		Path path1 = Paths.get(fileName);
		reader = new BufferedReader(new FileReader(path1.toAbsolutePath().toString()));

		String line;
		while ((line = reader.readLine()) != null) {
			String[] values = line.split(",");
			colores.put(values[0], values[1]);

		}

		reader.close();
	}

	public static colorConverter getInstance() throws IOException {
		if (instance == null) {
			instance = new colorConverter();
		}
		return instance;
	}

	/**
	 * Retorna código de color Hex
	 * 
	 * @param colorTexto
	 * @return
	 */
	public String getColor(String colorTexto) {

		return colores.get(colorTexto);
	}

}
