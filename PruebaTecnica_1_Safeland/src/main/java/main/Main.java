package main;

import clases.Gaveta;
import clases.MySQLCon;
import clases.TableHTML;
import clases.colorConverter;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.Scanner;

import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

class Main {

	private static Scanner scanner;
	private static String fileHTLM = ".\\files\\invent.html";

	public static void main(String[] args) {

		MySQLCon con = null;
		colorConverter colors = null;
		FileInputStream file = null;
		XSSFWorkbook workbook = null;
		scanner = new Scanner(System.in);
		ArrayList<Gaveta> gavetas = new ArrayList<Gaveta>();

		try {
			con = MySQLCon.getInstance();
			System.out.println("Conexión con base de datos lista");
		} catch (ClassNotFoundException e) {
			System.out.printf("El driver \"%s\" de la base de datos no existe \n", "com.mysql.cj.jdbc.Driver");
		} catch (SQLException e) {
			System.out.println("Error de conexión con base de datos");
		}

		try {
			colors = colorConverter.getInstance();
		} catch (IOException e) {
			System.out.println("Error al cargar colores");
		}

		if (con != null && colors != null) {
			System.out.println("INGRESE RUTA DEL ARCHIVO:");
			String fileName = scanner.nextLine().trim();

			try {

				Path path1 = Paths.get(fileName);
				file = new FileInputStream(new File(path1.toAbsolutePath().toString()));
				workbook = new XSSFWorkbook(file);

				System.out.println("\nVISUALISANDO ARCHIVO:");
				printFile(workbook);

				System.out.println("\nPRESIONE ENTER PARA CONTINUAR");
				try {
					System.in.read();
					scanner.nextLine();
				} catch (Exception e) {
				}

				readFile(workbook, gavetas);
				readDatabase(con, gavetas);

				System.out.println("\nIMPIMIENDO INVENTARIO");
				printInventario(gavetas);

				// Generar HTML
				System.out.println("\nQUIERE IMPRIMIR? Y/N");
				String conf = scanner.nextLine();

				if (conf.equals("Y") || conf.equals("y")) {

					File newFile = new File(fileHTLM);

					TableHTML tableGenerator = new TableHTML(newFile, gavetas);
					tableGenerator.copyTemplate();
					tableGenerator.writeTable();

					Path path2 = Paths.get(fileHTLM);
					System.out.printf("html guardado en : %s \n", path2.toAbsolutePath().toString());

				}

			} catch (FileNotFoundException e) {
				System.out.printf("El archivo \"%s\" no existe \n", fileName);

			} catch (IOException e) {

				System.out.println("Error de lectura");
			}

			if (!con.closeConection()) {
				System.out.println("Conexion con base de datos no puedo ser terminanda");
			}
			;

			if (workbook != null) {

				try {
					workbook.close();
				} catch (IOException e) {
					System.out.printf("El archivo \"%s\" no puede ser cerrado \n", fileName);
				}

			}

		}
	}

	/**
	 * Imprime contenido del archivo de Exel de un sola columna
	 * 
	 * @param file
	 * @throws IOException
	 */
	public static void printFile(XSSFWorkbook file) throws IOException {

		XSSFSheet sheet = file.getSheetAt(0);
		Iterator<Row> rowIterator = sheet.iterator();
		while (rowIterator.hasNext()) {
			Row row = rowIterator.next();
			Cell cell = row.getCell(0);
			String line = cell.getStringCellValue();
			System.out.println(line);
		}
	}

	/**
	 * Lee el contenido del archivo de Exel de un sola columna y llena una lista de
	 * Gaveta
	 * 
	 * @param file
	 * @param gavetas
	 * @throws IOException
	 */
	public static void readFile(XSSFWorkbook file, ArrayList<Gaveta> gavetas) throws IOException {

		XSSFSheet sheet = file.getSheetAt(0);
		Iterator<Row> rowIterator = sheet.iterator();
		while (rowIterator.hasNext()) {
			Row row = rowIterator.next();
			Cell cell = row.getCell(0);
			String line = cell.getStringCellValue();
			if (line.length() != 0) {
				Gaveta gaveta = Gaveta.formString(line);
				gavetas.add(gaveta);
			}

		}
	}

	public static void readDatabase(MySQLCon con, ArrayList<Gaveta> gavetas) {

		Iterator<Gaveta> gavetaIterator = gavetas.iterator();
		while (gavetaIterator.hasNext()) {
			Gaveta gaveta = gavetaIterator.next();
			String nombre = con.getNombreByCodigo(gaveta.getGaveta());
			gaveta.setGavetanombre(nombre.toUpperCase());
		}
	}

	public static void printInventario(ArrayList<Gaveta> gavetas) {
		Iterator<Gaveta> gavetaIterator = gavetas.iterator();

		while (gavetaIterator.hasNext()) {
			Gaveta gaveta = gavetaIterator.next();
			for (int i = gaveta.getCantidad(); i > 0; i--) {
				System.out.printf("%s - %s \n", gaveta.getNombre(), gaveta.getGavetanombre());
			}
			System.out.println();
		}
	}
}